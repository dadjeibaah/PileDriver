using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace PileDriver
{
    class SourceFile
    {
        private StreamReader srSrcReader;
        private string sSrcCde;
        private int Linenum;
        private int charpos;
        private bool newline;
        
        //accessor methods for SourceCode and line number
        public string accSrcCde
        {
            get { return sSrcCde; }
        }
        public int Line
        {
            get { return Linenum; }
        }
        public int CharacterPos
        {
            get { return charpos; }
        }
        //Default Constructor
        public SourceFile()
        {
        }

        /*Method:   SourceFile Constructor
         * Pre:     A valid file path is being passed into the function.
         * Post:    All member variables have been initialized.
         *          sSrcCde (source code string) has been intialized
         * */
        public SourceFile(string FilePath)
        {
            srSrcReader = new StreamReader(FilePath);
            Linenum = 1;
            ReadSrcCde();
            charpos = -1;
            newline = false;
        }//SourceFile

        /*Method:   ReadSrcCde
         *          Reads in the source code from the StreamReader and adds it to a the string
         *          sSrcCde
         * Pre:     the member variable srSrcReader must be initialized
         * Post:    The source code has been added and the StreamReader is reset
         * */
        public void ReadSrcCde()
        {
            while (srSrcReader.Peek() >= 0)
            {
                sSrcCde += srSrcReader.ReadLine() + "\r\n";
            }

            sSrcCde += (char)65535;
            srSrcReader.DiscardBufferedData();
            srSrcReader.BaseStream.Position = 0;
            srSrcReader.Close();
        }//ReadSrcCde

        /*Method:   GetNextChar
        *          The functions gets one character at a time from the source code stream
        * Pre:     N/A
        * Post:    The string value SPACE is return if \r\n or a space character is detected.
        *          The detected character is returned as a string
         *         the string EOF is returned if the StreamReader has reached the end of file.
        * */
        public char GetNextChar(bool WSPA = true)  //WhiteSpace Avoidance for comments and strings
        {
            
            if (sSrcCde != null)
            {
              if(charpos < sSrcCde.Length-1)
                {
                    charpos++;
                    char nextchar = sSrcCde[charpos];
                    if (newline)
                    {
                        Linenum++;
                        newline = false;
                    }
                    if (WSPA)
                    {
                        if (nextchar == '\r' && sSrcCde[charpos + 1] == '\n')
                        {

                            charpos += 1;
                            newline = true;
                            return ' ';
                        }
                        if (nextchar == '\t')
                        {

                            return ' ';
                        }
                    }
                    
                    return nextchar;
                    
                }
              
                    /*if (srSrcReader.Peek() >= 0)
                    {
                        char[] inchar = new char[1];
                        srSrcReader.Read(inchar, 0, 1);

                        if (inchar[0] == '\r' && srSrcReader.Peek() == '\n')
                        {
                        
                            srSrcReader.Read(inchar, 0, 1);
                            Linenum++;
                            return "SPACE";
                        }
                        else if (inchar[0] == ' ')
                        {
                            return "SPACE";
                        }
                        else return inchar[0].ToString();
                    }*/

                   
            }
            StreamRst();
            return ' ';
         }//GetNextChar

        public void StreamRst()
        {
            try
            {
                Linenum = 1;
                charpos = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fatal error: " + ex.Message);
            }
        }

        public void PushBackChar()
        {
            if (sSrcCde[charpos - 1] == '\n')
                charpos -= 3;
            else charpos -= 1;
        }

        public char Peek()
        {
            
            char peekchar = sSrcCde[charpos + 1];
            if (peekchar < 0)
            {
                return (char)65535;
            }
            return peekchar;
            
        }
        
        public string ExtractString(int characpos, int endpos)
        {
            
            return sSrcCde.Substring(characpos, endpos);
        }

    }
}
