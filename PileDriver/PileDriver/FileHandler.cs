using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.IO;
namespace PileDriver
{
    public enum FileCreate
    {
        SymFile,
        TokFile,
        Cmd,
        ASM,
        SINC,
        PINC
    };
    class FileHandler
    {
        private Hashtable FileList;
        private string FileNamenoX;

        private static FileHandler fHModel;
        private static Object fHModel_lock = typeof(FileHandler);

        public string FileName
        {
            get { return FileNamenoX; }
        }
        /*Constructor: FileHandler
         * Pre:     FileList, the hashtable to store directories and file names has been declared
         * Post:    3 default directory types (SRC_D, and SRC_F) have been added to the FileList as
         *          keys with null values
         * */
        private FileHandler()
        {
            FileList = new Hashtable();
            FileList.Add(DIR_TYPE.SRC_D, "C:\\Users\\Deebo\\Documents\\College\\Spring 2013\\Compilers\\TEST_MOD");
            FileList.Add(DIR_TYPE.MA, null);
            FileList.Add(DIR_TYPE.SRC_F, null);
            FileList.Add(DIR_TYPE.TKN_F, null);
           

        }//FileHandler

        /*Method:  Instance
         * Pre:    The FileHandler single instance and lock has been created.
         * Post:   The FileHandler class has been given a new lock if it has not been instantiated
         *         else return the same instance. 
         * */
        static public FileHandler Instance()
        {
            lock (fHModel_lock)
            {
                if (fHModel == null)
                {
                    fHModel = new FileHandler();
                    return fHModel;
                }

                return fHModel;
            }
        }//Instance

        /*Method:   LoadDialog
         * Pre:     The function has passed in an appropriate directory type.
         * Post:    A directory type and its directory/filepath has been added to the 
         *          FileList hashtable
         * */
        public void LoadDialog(DIR_TYPE loadtype)
        {
            FolderBrowserDialog File = new FolderBrowserDialog();
            if (File.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //add filepath info for selected directory or file.
                    FileList[loadtype] = File.SelectedPath;


                }
                catch (Exception ex)
                {
                    MessageBox.Show("The was an error opening the source directory" + ex.Message);
                }
            }
        } //LoadDialog()

        /*Method:   LoadFileDialog
         * Pre:     N/A
         * Post:    A new file name has been added to the FileList under under the SRC_F FileList
         *          entry.
         * */
        public void LoadFileDialog()
        {
            OpenFileDialog ofdFile = new OpenFileDialog();
            ofdFile.InitialDirectory = (string)FileList[DIR_TYPE.SRC_D];
            ofdFile.Filter = "mod files (*.mod)|*.mod";
            if (ofdFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //add filepath info for selected directory or file.
                    FileList[DIR_TYPE.SRC_F] = ofdFile.SafeFileName;
                    FileNamenoX = ((string)FileList[DIR_TYPE.SRC_F]).Substring(0, ((string)FileList[DIR_TYPE.SRC_F]).IndexOf('.'));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("The was an error opening the source file" + ex.Message);
                }
            }
        }//LoadFileDialog

        /*Method:   RtnFilePath
         * Pre:     an appropriate DIR_TYPE has been passed and the isFileName parameter is 
         *          either true or false
         * Post:    the string "cannot find directory path" is returned if there is no value for the
         *          passed in DIR_TYPE
         *          the value of SRC_D and SRC_F (a complete file path) is passed if the DIR_type has
         *          a value and isFileName is true
         *          otherwise the value of the passed in DIR_TYPE is returned
         * */
        public string RtnFilePath(DIR_TYPE DirType, bool isFullFilePath = false)
        {
            //return a string for a directory in the file handler.
            if (FileList[DirType] == null)
            {
                return "Cannot find directory path";
            }
            else if (isFullFilePath)
            {
                return (string)FileList[DIR_TYPE.SRC_D] + "\\" + (string)FileList[DIR_TYPE.SRC_F];
            }
            else return (string)FileList[DirType];
        }// RtnFilePath

        /*Method:  CreateFolder
        * Pre:     N/A
        * Post:    A folder in the source directory has been created.
        * */
        public void CreateFolder()
        {
            string filenoext = (string)FileList[DIR_TYPE.SRC_F];
            try
            {
                if (FileList[DIR_TYPE.TKN_F] == null)
                {
                    
                    filenoext = filenoext.Remove(filenoext.LastIndexOf('.'));
                    string pathString = System.IO.Path.Combine((string)FileList[DIR_TYPE.SRC_D], filenoext + "_" + "PileDriven");
                    FileList[DIR_TYPE.TKN_F] = pathString;
                    System.IO.Directory.CreateDirectory(pathString);
                    string asm = pathString + "\\" + filenoext + " ASM";
                    FileList[DIR_TYPE.ASM] = asm;
                }
                else
                {
                    filenoext = filenoext.Remove(filenoext.LastIndexOf('.'));
                    string pathString = System.IO.Path.Combine((string)FileList[DIR_TYPE.SRC_D], filenoext + "_" + "PileDriven");
                    FileList[DIR_TYPE.TKN_F] = pathString;
                    System.IO.Directory.CreateDirectory(pathString);
                }
            }
            catch(NullReferenceException)
            {
                MessageBox.Show("Please load a source file");
            }
        }//CreateFolder

        public void WriteToFile(FileCreate type,string text)
        {
            if (type == FileCreate.TokFile)
                System.IO.File.WriteAllText(FileList[DIR_TYPE.TKN_F] + "\\TokenList.txt", text);
            else if (type == FileCreate.SymFile)
                System.IO.File.WriteAllText(FileList[DIR_TYPE.TKN_F] + "\\SymbolT.txt", text);
            else if(type == FileCreate.Cmd)
            {
             File.WriteAllText(FileList[DIR_TYPE.TKN_F] +"\\"+FileNamenoX+".cmd",text);
            }
            else if (type == FileCreate.ASM)
            {
                System.IO.File.WriteAllText(FileList[DIR_TYPE.TKN_F] + "\\"+FileNamenoX+".asm", text);
            }
            else if (type == FileCreate.SINC)
            {
                File.WriteAllText(FileList[DIR_TYPE.TKN_F] + "\\strings.inc", text);
            }
            else if (type == FileCreate.PINC)
            {
                File.WriteAllText(FileList[DIR_TYPE.TKN_F] + "\\proclist.inc", text);
            }
        }
    }
}
