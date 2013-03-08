using System;
using System.Collections;   // ArrayList

namespace PileDriver
{
    /// <summary>
    /// Emitter is a singleton class that creates assembler files. It works with the
    /// FileDefaultManager to keep track of what assembler code to write to what file.
    /// A number of assembler files are used during compilations:
    ///    string.inc
    ///    PV999999Main_thf.inc
    ///    PV000001towers.inc
    ///    etc.
    /// </summary>
    public class Emitter
    {
        
        // #########################################################################################
        // ASSEMBLER METHODS   ASSEMBLER METHODS   ASSEMBLER METHODS   ASSEMBLER METHODS     
        // #########################################################################################

        /// <summary>
        /// PRE:  The desired output on the top of the run-time stack.
        /// POST: The integer is displayed.
        /// </summary>
        /// 
        private static Emitter Emitt;
        private static Object emitt_lock = typeof(Emitter);
        private static FileHandler c_fm;
        private int c_iMainMemoryUse;
        private string main;
        private string includestr;
        private string procliststr;
        private int procindex;
        private int str_counter;

        public string Main
        {
            get { return main; }
            set { main = value; }
        }
        public static Emitter GetEmitt()
        {
            lock (emitt_lock)
            {
                if (Emitt == null)
                {
                    Emitt = new Emitter();
                    
                }
                return Emitt;
            }
        }

        private Emitter()
        {
            c_fm = FileHandler.Instance();
            procliststr = "; ProcList Include File for PileDriver\r\n\r\n";
            includestr = "; String Include File for PileDriver\r\n\r\n"
                    + "str0      DB  'Press any key to continue...',0\r\n";
            str_counter = 1;
            procindex = 0;
        }
        public void WRINT()
        {
        }

        public void WRLN()
        {
            AWriteOp("\t nwln");
        }

        public void WRSTR(string strIn)
        {
            includestr += String.Format("str{0}     DB      '{1}',0\r\n", str_counter, strIn);
          AWriteOp(String.Format("PutStr str{0}\r\n", str_counter));
            str_counter++;
        }

        public void AWriteOp(string str)
        {
            procliststr += str+"\r\n";
        }

    
        // #########################################################################################
        // A R I T H M E T I C               A R I T H M E T I C               A R I T H M E T I C      
        // #########################################################################################

        /// <summary>
        /// PRE:  The second operand is on the top of the stack. The first is next on the stack.
        /// POST: The sum is on the top of the stack.
        /// </summary>
        public void AddOperation()
        {
        }

        
        // #########################################################################################
        // FILE HANDLER METHODS   FILE HANDLER METHODS   FILE HANDLER METHODS   FILE HANDLER METHODS   
        // #########################################################################################

        /// <summary>
        /// PRE:  The parse is complete.
        ///    c_iMainMemoryUse stores the amount of memory needed by the main procedure.
        /// POST: A string is created and the assembler "shell" is written to it.
        /// </summary>
        public string MainAFile() // 
        {
            string strHead = "COMMENT |\r\nTITLE PileDriver output: " + c_fm.RtnFilePath(DIR_TYPE.SRC_D) + "\r\n| Created: ";

            // create a time stamp
            DateTime dt = DateTime.Now;
            strHead += dt.ToString("F") + "\r\n";
            strHead += ".MODEL SMALL\r\n" // use the small (16-bit) memory model
                + ".486\r\n"          // this allows 32-bit arithmetic (EAX, EDX registers, etc.)
                + ".STACK 1000H\r\n"  // plenty of stack space: 4096 bytes
                + ".DATA\r\n"         // begin DATA section

                //definition of a char for "Press the any key to continue:" (John Broere 2002)
                + "end_ch  DB  ?          ; John Broere 2002 idea to 'pause' at end.\r\n\r\n"

                // The following file must be created later in the same directory
                //    to contain string constants of the form:
                + ";===== string constants inserted here: ======\r\n"
                + "INCLUDE strings.inc\r\n\r\n"

                + ".CODE\r\n"
                + "INCLUDE io.mac\r\n"
                + "main PROC\r\n"
                + ".STARTUP\r\n"
                + "push    EBP            ; save EBP since we use it\r\n"
                + "sub     SP, " + string.Format("{0}", 4)
                + "         ; Room for main proc local vars\r\n"
                + "mov     BP,SP          ; set the stack pointer as the base pointer\r\n"
                + "call " + main + "\r\n"

                // adds a "pause" to the end of the program - thanks to John Broere 2002 !

                // note that Kevin added str0 to the string collection,
                //    and he added a character (end_ch) in the data segment above.
                + "nwln\r\n"
                + "PutStr  str0\r\n"
                + "GetCh   end_ch\r\n"

                // end the program
                + "pop     EBP            ; restore EBP\r\n"
                + ".EXIT\r\n"
                + "main    ENDP           ; end of assembly outermost function\r\n\r\n"
                + "; The following procedures must be included.\r\n"
                + "INCLUDE proclist.inc   ; lines like 'INCLUDE V000000main.inc'\r\n"
                + "END\r\n";

            return strHead;
        }

        public void AddtoStringsInc(string input)
        {
            includestr += String.Format("str{0}     DB      '{1}',0\r\n",str_counter,input);
            str_counter++;
        }

        public void AddtoProc()
        {
        }
        /// <summary>
        /// PRE:  The name of the procedure is passed. We have already called 
        ///    EnterNewProcScope which tracks the current scope number. Note that this
        ///    array of procedure strings must remain parallel to the array of procedures
        ///    maintained by SymbolTable.
        /// POST: The preamble is emitted. This includes creating the assembly string
        ///    and increasing the procedure index.
        ///    
        /// Note the special version for the main procedure
        /// </summary>
        public void ProcPreamble(string strProcName)
        {
            main = String.Format("P{0}{1}", procindex, strProcName);
            procliststr += String.Format("P{0}{1}  PROC\r\n\r\n", procindex, strProcName);
        }

        public void MainProcPreamble(string strProc)
        {
            
            ProcPreamble(strProc);
        }

        /// <summary>
        /// PRE:  The name of the procedure and 
        ///    the amount of memory needed in the stack is passed.
        ///    SymbolTable.ExitProcScope() has been called to re-establish 
        ///    the correct new scope.
        /// POST: The postamble is emitted to the current string.
        ///    The procedure index is returned to the correct value
        ///    (by querying SymbolTable).
        ///    
        /// Note the special version for the main procedure.
        /// </summary>
        public void ProcPostamble(string strProcName, int iMemUse)
        {
        }
        
        /// <summary>
        /// PRE:  The amount of memory needed in the stack is passed.
        ///    SymbolTable.ExitProcScope() has been called to re-establish 
        ///    the correct new scope.
        /// POST: The postamble is emitted to the current string.
        ///    The procedure index is returned to the correct value
        ///    (by querying SymbolTable). 
        ///    
        ///    Since this is the end of main (parsing is complete),
        ///    we now know the amount of memory needed by the main procedure.
        ///    We retain this for use while writing the outermost assembler file.
        /// </summary>
        /// 
       
        public void MainProcPostamble(string strProcName, int iMemUse = 0)
        {
            procliststr += String.Format("\t ret\r\n");
            procliststr += String.Format("P{0}{1} ENDP ",procindex, strProcName);
            procindex++;
          //  c_iMainMemoryUse = iMemUse;
           // ProcPostamble(c_fm.MAIN_PROC, iMemUse);
        }

        /// <summary>
        /// PRE:  The assembler files have all been "written" to strings.
        /// POST: The files are written to the disk.
        /// </summary>
        public void WriteAFiles()
        {
            // write the outermost "shell" assembler file
            c_fm.WriteToFile(FileCreate.ASM,MainAFile());
            c_fm.WriteToFile(FileCreate.SINC, includestr);
            c_fm.WriteToFile(FileCreate.PINC, procliststr);

            // do more stuff

            // Create the command file and invoke it to complete the assembly
            BuildCmdFile();
        }

        /// <summary>
        /// PRE:  The parse is complete.
        /// POST: The command file is created for remaining steps of the assembly process
        ///    (compilation and linking to create an execcutable).
        ///    This command file is then run to complete the compilation.
        /// </summary>
        void BuildCmdFile()
        {
            string strMakeFile =
                "REM ===== PileDriver: auto-created command file ======\r\n"
                + "set path=%path%;" + c_fm.RtnFilePath(DIR_TYPE.MA) + "\r\n"

                // set the directory
                + "cd " + c_fm.RtnFilePath(DIR_TYPE.TKN_F) + "\r\n"

                // copy files needed for the compiling and linking (respectively)
                + "copy " + c_fm.RtnFilePath(DIR_TYPE.MA, false) + "\\io.mac .\r\n"
                + "copy " + c_fm.RtnFilePath(DIR_TYPE.MA, false) + "\\io.obj .\r\n"

                // assemble to create the object file
                + c_fm.RtnFilePath(DIR_TYPE.MA, false) + "\\ml /c " + c_fm.FileName+".asm\r\n"

                // link the files to create the executable
                + c_fm.RtnFilePath(DIR_TYPE.MA,false) + "\\link16 "
                + c_fm.FileName+ ".obj io.obj, "
                + c_fm.FileName + ".exe, "
                + c_fm.FileName + ".map, , , \r\n\r\n" // Yes, the three commas are necessary!

                // add a pause, so we can see the results of the assembly and linking
                // Thanks to John Broere 2002 !
                + "@ PAUSE\r\n\r\n"
                + "cls"
                + c_fm.FileName + ".exe";
                 
            // Write the command string to the proper file
            c_fm.WriteToFile(FileCreate.Cmd, strMakeFile);

            // Invoke the file just created. This uses the static method in our SystemCommand class.
            //    If an error occurs it will throw the appropriate exception.
            SystemCommand.SysCommand(c_fm.RtnFilePath(DIR_TYPE.TKN_F) +"\\" + c_fm.FileName + ".cmd");
        }/**/
    }
}
