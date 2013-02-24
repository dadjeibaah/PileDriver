using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace PileDriver
{
    //These are the types of filepath information that are kept in the 
    //if a function would like to request information about a file path
    //they would have to pass in one of these three types to get the appropriate
    //filepath
    public enum DIR_TYPE
    {
        MA,         //Type of file path for MASM directory
        SRC_D,      //Type of file path for source code directory
        SRC_F,       //Type of "file path" for source code file.
        TKN_F
    };

    class Facade
    {
        FileHandler fhHandler;
        SourceFile srcFile;
        Tokenizer Lexer;
        SymbolTable symtable;
        

        public Facade()
        {
            fhHandler = FileHandler.Instance();
            srcFile = new SourceFile();
            Lexer = Tokenizer.GetTokenizer();
            symtable = SymbolTable.Instance();
        }


        /*Method:   FileLoad
         * Pre:     the appropriate directory type has been established in the paremeter
         * Post:    the file path for the directory is returned.
         * */
        public string FileLoad(DIR_TYPE loadtype)
        {
            fhHandler.LoadDialog(loadtype);
            return fhHandler.RtnFilePath(loadtype, false);
        }//FileLoad

        /*Method:   LoadSourceFile
         * Pre:     N/A
         * Post:    return a string array with the source code in the first entry and the file name
         *          in the second entry.
         * */
        public string[] LoadSourceFile()
        {
            string[] rtnStr = new string[2]{"null","null"};
            try
            {

                fhHandler.LoadFileDialog();
                srcFile = new SourceFile(fhHandler.RtnFilePath(DIR_TYPE.SRC_D, true));
                rtnStr[0] = srcFile.accSrcCde;
                rtnStr[1] = fhHandler.RtnFilePath(DIR_TYPE.SRC_F, false);
                fhHandler.CreateFolder();
            }
            catch { MessageBox.Show("System error, contact manufacturer to resolve issues"); }
            return rtnStr;
        }//LoadSourceFile

        /*Method:   NextCharAndLine
          * Pre:     N/A
          * Post:    A string array that contains the char from source and the current line number
          * */
        public string[] NextCharAndLine()
        {
            string[] rtnStr = new string[2];
            if(srcFile != null)
            {
                char incoming = srcFile.GetNextChar();
                if (incoming == (char)65535)
                {
                    rtnStr[0] = "EOF";
                }
                else rtnStr[0] = incoming.ToString();
                
                rtnStr[1] = srcFile.Line.ToString();
            }
            return rtnStr;
            
        }//GetNextCharWindow
        public void ResetGNC()
        {
            srcFile.StreamRst();
        }

        public string DisplayTokens()
        {
            try
            {
                
                Lexer.SetSourceFileName(fhHandler.RtnFilePath(DIR_TYPE.SRC_D, true));
                string text = Lexer.ListTokens();
                fhHandler.WriteToFile(FileCreate.TokFile, text);
                return text;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fatal Error: " + ex.Message);
            }
             return "";
        }
        public DataTable DisplayTable()
        {
            return Lexer.TableOfTokens;
        }

        public void CreateSymTableFile()
        {
            fhHandler.WriteToFile(FileCreate.SymFile,symtable.DumpSymTable());
        }
        public bool isOutSourceDirLoaded()
        {
            if (fhHandler.RtnFilePath(DIR_TYPE.TKN_F, false).Contains("Cannot"))
                return false;
            else return true;
        }

    }
}
