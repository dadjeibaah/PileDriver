using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PileDriver
{

    class Parser
    {
        private Emitter CodeEmitt;
        private static Parser Parseobj;
        private static Object parse_lock = typeof(Parser);
        Token currToken;
        Token NextToken;
        Tokenizer tokenizerobj = Tokenizer.GetTokenizer();
        SymbolTable table = SymbolTable.Instance();

        //Singleton Implementer for parser
        public static Parser GetParser()
        {
            lock (parse_lock)
            {
                if (Parseobj == null)
                {
                    Parseobj = new Parser();
                }
                return Parseobj;
            }
        }

        //Constructor
        private Parser()
        {
            CodeEmitt = Emitter.GetEmitt();
           
        }

        /*Method:   CompileSource
         * Pre:     The lexer has been reset and is ready to give tokens.
         * Post:    Processes all tokens and reflects changes in Emitter
         * */
        public void CompileSource()
        {
            
            tokenizerobj.Reset();
            currToken = tokenizerobj.GetNextToken();
            

            ProgramMod();
            CodeEmitt.WriteAFiles();
            
        }//CompileSource

        /*Method:   Expected
         * Pre:     A token type is sent into the method.
         * Post:    A boolean that returns true if the current token matches the token specified.
         * */
        private void Expected(Token.TOKENTYPE expected)
        {
            NextToken = currToken;
            if (currToken.m_tokType == Token.TOKENTYPE.EOF)
            {
                return;
            }
            if (currToken.m_tokType == expected)
            {
                currToken = tokenizerobj.GetNextToken();
            }
            else throw new Exception(String.Format("Parse Error: Error in line {0}, Somewhere near {1}", currToken.m_iLineNum, currToken.m_strName));
        }//Expected

        /*Method:   ProgramMod
         * Pre:     SourceFile is ready for parsing
         * Post:    File has finished parsing
         * */
        private void ProgramMod()
        {
            string main = "";
            //currToken = tokenizerobj.GetNextToken();
            Expected(Token.TOKENTYPE.MODULE);
            main = currToken.m_strName;
            table.AddMaintoTable(currToken.m_strName);
            CodeEmitt.MainProcPreamble(currToken.m_strName);
            Expected(Token.TOKENTYPE.ID);
            
            
            Expected(Token.TOKENTYPE.SEMI_COLON);
            Block();
            Expected(Token.TOKENTYPE.ID);
            ATTRIBUTE entry = new ATTRIBUTE();
            bool bPresent = false;
            table.FindInScope("P" + main, out entry, out bPresent);
            if (!bPresent)
            {
                throw new Exception(String.Format("Parse Error: Error in line {0}, Somewhere near {1}", currToken.m_iLineNum, currToken.m_strName));
            }
            Expected(Token.TOKENTYPE.DOT);
            CodeEmitt.MainProcPostamble(main);
            Expected(Token.TOKENTYPE.EOF);
            /*
            if (Expected(Token.TOKENTYPE.MODULE))
            {
                currToken = tokenizerobj.GetNextToken();
                if (Expected(Token.TOKENTYPE.ID))
                {
                    main = currToken.m_strName;
                    table.AddMaintoTable(currToken.m_strName);
                    CodeEmitt.MainProcPreamble(currToken.m_strName);
                    currToken = tokenizerobj.GetNextToken();
                    if (Expected(Token.TOKENTYPE.SEMI_COLON))
                    {
                        if (Block())
                        {
                            currToken = tokenizerobj.GetNextToken();
                            Expected(Token.TOKENTYPE.ID);
                            ATTRIBUTE entry = new ATTRIBUTE();
                            bool bPresent = false;
                            table.FindInScope("P" + main, out entry, out bPresent);
                            if (!bPresent)
                            {
                                return false;
                            }
                            currToken = tokenizerobj.GetNextToken();
                            if(Expected(Token.TOKENTYPE.DOT))
                            {
                                
                                CodeEmitt.MainProcPostamble(main);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
             * */
        }//CompileSource

        public void statementSequence()
        {


            if (NextToken.m_tokType == Token.TOKENTYPE.WRSTR)
            {
                Expected(Token.TOKENTYPE.LEFT_PAREN);
                
            }
            /*
            while (currToken.m_tokType != Token.TOKENTYPE.END)
            {
                if (currToken.m_tokType == Token.TOKENTYPE.WRSTR)
                {
                    currToken = tokenizerobj.GetNextToken();
                    if (currToken.m_tokType == Token.TOKENTYPE.LEFT_PAREN)
                    {
                        currToken = tokenizerobj.GetNextToken();
                        if (currToken.m_tokType == Token.TOKENTYPE.STRING)
                        {
                            CodeEmitt.WRSTR(currToken.m_strName);
                            currToken = tokenizerobj.GetNextToken();
                            if (currToken.m_tokType == Token.TOKENTYPE.RIGHT_PAREN)
                            {
                                currToken = tokenizerobj.GetNextToken();
                                if (currToken.m_tokType == Token.TOKENTYPE.SEMI_COLON)
                                    currToken = tokenizerobj.GetNextToken();
                            }
                        }
                    }
                }

                if (currToken.m_tokType == Token.TOKENTYPE.WRLN)
                {
                    CodeEmitt.WRLN();
                    currToken = tokenizerobj.GetNextToken();
                    if (currToken.m_tokType == Token.TOKENTYPE.SEMI_COLON)
                        currToken = tokenizerobj.GetNextToken();
                }
            }**/
             
        }
        private void Block()
        {
            Expected(Token.TOKENTYPE.BEGIN);
            statementSequence();
            Expected(Token.TOKENTYPE.END);
            /*
            currToken = tokenizerobj.GetNextToken();e
            if(Expected(Token.TOKENTYPE.BEGIN))
            {
                currToken = tokenizerobj.GetNextToken();
                statementSequence();
                if (Expected(Token.TOKENTYPE.END))
                {
                    return true;
                }
                
            }
            return false;
             * */
        }
    }
}
