using System;               // Exception 
using System.Collections;   // ArrayList, HashTable
using System.Data;
using System.Windows.Forms;

namespace PileDriver
{
    /// <summary>
    /// A token is a category of lexemes.
    /// Some tokens are keywords like "MODULE". Some tokens are symbols; the token of type
    ///    PLUS is the symbol "+". Some tokens are a large set; some tokens of type
    ///    INTEGER are "73" and "255".
    /// The enumerations below include all the tokens that we need to recognize.
    /// 
    /// Note that the Modula-2 User's Manual offers both # and <> to mean NOT EQUAL 
    ///    (pp. 96, 97, and 110). Our code does not use #. We only use <>.
    /// 
    /// Many errors are reported by TOKENTYPE which is just a number (like 34 for ID).
    /// Therefore, I added enumeration numbers for tokens in the left column as a
    ///    convenience during debugging.
    ///    
    /// Author: Tom Fuller
    /// Date:   January 6, 2007
    /// identifier:(L|_)(L|D|_)*
    /*identifier:(L|_)(L|D|_)*
    int_num:D+
    real_num:0.D+|D+.D*
    string:"[^"]*"
    minus:-
    dot:.
    dot_dot:..
    semi_colon:(;)
    colon:(:)
    grtr_than:>
    plus:\+
    mult:\*
    assign::=
    less_than:<
    grtr_than_eq:>=
    slash:/
    less_than_eq:<=
    not_eq:<>
    left_brack:\[
    right_brack:\]
    equal:=
    whitespace:\ |\n|\t*/
    /// 
    /// </summary>
    public class Token
    {
        public enum TOKENTYPE
        {
            /* 0*/
            ERROR = 0, AND, ARRAY, BEGIN,
            /* 4*/
            CARDINAL, CONST, DIV, DO,
            /* 8*/
            ELSE, END, EXIT, FOR, IF,
            /*13*/
            INTEGER, LOOP, MOD, MODULE,
            /*17*/
            NOT, OF, OR, PROCEDURE,
            /*21*/
            REAL, THEN, TYPE, VAR,
            /*25*/
            WHILE, WRCARD, WRINT, WRLN,
            /*29*/
            WRREAL, RDCARD, RDINT, RDREAL, WRSTR,

            /*34*/
            ID, CARD_NUM, REAL_NUM, INT_NUM,

            /*38*/
            STRING,

            /*39*/
            ASSIGN, COLON, COMMA, DOT,
            /*43*/
            DOT_DOT, EQUAL, LEFT_BRACK, LEFT_PAREN,
            /*47*/
            MINUS, MULT, NOT_EQ, PLUS,
            /*51*/
            RIGHT_BRACK, RIGHT_PAREN, SEMI_COLON, SLASH,
            /*55*/
            GRTR_THAN, GRTR_THAN_EQ, LESS_THAN, LESS_THAN_EQ,

            /* The two comment tokens are not presently used */
            /*59*/
            COMMENT_BEG, COMMENT_END,

            /*61*/
            EOF
        };

        /*********************************************************************************
         There are 34 reserved keywords we need to recognize in Modula-2 source code files.
            Note that enumerated types 0 - 33 above correspond precisely to these keywords. 
            Some number types are not used except by very ambitious students: CARDINAL, REAL.
            Some statements are also reserved for the fearless: WHILE, DO, FOR.
        *********************************************************************************/

        public static int c_iKeywordCount = 34;


        /*********************************************************************************
          The Token class itself stores information about a single token.
         *********************************************************************************/
        public TOKENTYPE m_tokType;    /* what token (i.e. class of lexemes)          */
        public string m_strName;    /* lexeme name: reserved word, identifier      */
        public int m_iLineNum;   /* the (source file)line containing the token  */

        // default constructor, not used
        public Token()
        {
            m_tokType = Token.TOKENTYPE.ERROR;
            m_strName = "blank token";
            m_iLineNum = 0;
        }

        // lexing error, gives lexeme and line number
        public Token(string inName, int inLine)
        {
            m_tokType = Token.TOKENTYPE.ERROR;
            m_strName = inName;
            m_iLineNum = inLine;
        }

        // normal constructor with tokentype, lexeme and line number
        public Token(Token.TOKENTYPE tok, string inName, int inLine)
        {
            m_tokType = tok;
            m_strName = inName;
            m_iLineNum = inLine;
        }

        public override string ToString()
        {

            return string.Format("{3,3}: Token {0,-10}: {1,-15} {2,30}", (int)m_tokType,
                m_tokType.ToString(), m_strName, m_iLineNum);
        }

        public string[] ToStringArray()
        {
            string[] values = new string[4];
            values[0] = m_iLineNum.ToString();
            values[1] = ((int)m_tokType).ToString();
            values[2] = m_tokType.ToString();
            values[3] = m_strName;

            return values;
        }
    } // class Token

    /// <summary>
    /// Tokenizer returns tokens (lexemes).
    /// This is implemented as a singleton pattern (Design Patterns in C#)
    /// </summary>
    public class Tokenizer
    {
        private DataTable m_TableOfTokens;

        public DataTable TableOfTokens
        {
            get { return m_TableOfTokens; }

        }
        // The Modula-2 source file
        SourceFile m_SourceFile;

        // The hashtable of Modula-2 keywords
        Hashtable m_htKeywords;

        // The single object instance for this class.
        private static Tokenizer c_tokenizer;

        // To prevent access by more than one thread. This is the specific lock 
        //    belonging to the Tokenizer Class object.
        private static Object c_tokenizerLock = typeof(Tokenizer);

        /*This state table was created on hackingoff.com, only the state table was copied no code*/
        private int[][] statetable = {new int[] { 1,  2, -1, -1}, //state 0
                                      new int[] { 3,  4,  5, -1}, //state 1
                                      new int[] { 3,  4,  5, -1}, //state 2
                                      new int[] { 3,  4,  5, -1}, //state 3
                                      new int[] { 3,  4,  5, -1}, //state 4
                                      new int[] { 3,  4,  5, -1}}; //state 5

        private Hashtable statekeys;

        // Instead of a constructor, we offer a static method to return the only
        //    instance.
        private Tokenizer() { } // private constructor so no one else can create one.

        static public Tokenizer GetTokenizer()
        {
            lock (c_tokenizerLock)
            {
                // if this is the first request, initialize the one instance
                if (c_tokenizer == null)
                {
                    // create the single object instance
                    c_tokenizer = new Tokenizer();

                    // Load the keywords
                    c_tokenizer.LoadKeywords();
                }

                // return a reference to the only instance
                return c_tokenizer;
            }
        }

        /// <summary>
        /// Set the name of the Modula-2 source code file (including path).
        ///
        /// PRE:  A source file is available and has the given name.
        /// POST: The SourceFile object is created with the given name. 
        ///    The source file is not yet opened nor is its existence verified.
        /// </summary>
        public void SetSourceFileName(string inName)
        {
            m_SourceFile = new SourceFile(inName);
            m_TableOfTokens = new DataTable();
            m_TableOfTokens.Columns.AddRange(new DataColumn[]{
                new DataColumn("TK"),
                new DataColumn("Line"),
                new DataColumn("Type"),
                new DataColumn("Lexeme")
            });
            Reset();
        }

        /// <summary>
        /// Reset the lexing process.
        /// PRE:  A source file is known.
        /// POST: The SourceFile object is reset.
        /// </summary>
        public void Reset()
        {
            // set the file position back to the starting point
            m_SourceFile.StreamRst();
        }

        /// <summary>
        /// Load M2 keywords.
        /// PRE:  An array of keywords is available in Token.
        /// POST: The hashtable is loaded and the correct token type is
        ///    associated with each entry.
        /// </summary>
        private void LoadKeywords()
        {
            m_htKeywords = new Hashtable();
            //This initiates the variables or possible edges for the state table.
            statekeys = new Hashtable();
            statekeys["Letter"] = 0;
            statekeys["_"] = 1;
            statekeys["Num"] = 2;
            statekeys["NC"] = 3;


            // load the keyword hashtable
            for (int i = 0; i < Token.c_iKeywordCount; i++)
                // For each entry, we add the string as the *key*,
                //    and the loop control variable i (cast as a TOKENTYPE) as the *value*.
                m_htKeywords.Add(((Token.TOKENTYPE)i).ToString(), (Token.TOKENTYPE)i);
        }

        /// <summary>
        /// This is the principal function of this class.
        ///
        /// PRE:  A source file must be available.
        ///       The source file is open and positioned at the next token (or white space).
        /// POST: The Token object is created, loaded with correct information, and returned.
        /// </summary>
        /// <returns>Token loaded with the correct data.
        /// If tokenizing has failed, the TOKENTYPE is T_ERROR</returns>

        public Token GetNextToken()
        {
            Char cNextChar = ' ';
            int CurrTokPos;
            int EndTokPos = 1;
            do
            {
                if ((!Char.IsWhiteSpace(cNextChar)))
                    break;
                cNextChar = m_SourceFile.GetNextChar();
            } while (Char.IsWhiteSpace(cNextChar));



            if (cNextChar == (char)65535) { return new Token(Token.TOKENTYPE.EOF, "end of source file", m_SourceFile.Line); }

            switch (cNextChar)
            {
                case ',':
                    return new Token(Token.TOKENTYPE.COMMA, cNextChar.ToString(), m_SourceFile.Line);
                case '+':
                    return new Token(Token.TOKENTYPE.PLUS, cNextChar.ToString(), m_SourceFile.Line);
                case '-':
                    return new Token(Token.TOKENTYPE.MINUS, cNextChar.ToString(), m_SourceFile.Line);
                case ';':
                    return new Token(Token.TOKENTYPE.SEMI_COLON, cNextChar.ToString(), m_SourceFile.Line);
                case '=':
                    return new Token(Token.TOKENTYPE.EQUAL, cNextChar.ToString(), m_SourceFile.Line);
                case '*':
                    return new Token(Token.TOKENTYPE.MULT, cNextChar.ToString(), m_SourceFile.Line);
                case '/':
                    return new Token(Token.TOKENTYPE.SLASH, cNextChar.ToString(), m_SourceFile.Line);
                case '[':
                    return new Token(Token.TOKENTYPE.LEFT_BRACK, cNextChar.ToString(), m_SourceFile.Line);
                case ']':
                    return new Token(Token.TOKENTYPE.RIGHT_BRACK, cNextChar.ToString(), m_SourceFile.Line);
                case ')':
                    return new Token(Token.TOKENTYPE.RIGHT_PAREN, cNextChar.ToString(), m_SourceFile.Line);
                case '<':
                    {
                        CurrTokPos = m_SourceFile.CharacterPos;
                        EndTokPos = 1;
                        if (m_SourceFile.Peek() == '=')
                        {
                            m_SourceFile.GetNextChar();
                            EndTokPos++;
                            return new Token(Token.TOKENTYPE.LESS_THAN_EQ, m_SourceFile.ExtractString(CurrTokPos, EndTokPos), m_SourceFile.Line);
                        }
                        else if (m_SourceFile.Peek() == '>')
                        {
                            m_SourceFile.GetNextChar();
                            EndTokPos++;
                            return new Token(Token.TOKENTYPE.NOT_EQ, m_SourceFile.ExtractString(CurrTokPos, EndTokPos), m_SourceFile.Line);
                        }
                        else return new Token(Token.TOKENTYPE.LESS_THAN, m_SourceFile.ExtractString(CurrTokPos, EndTokPos), m_SourceFile.Line);
                    }

                case '>':
                    {
                        CurrTokPos = m_SourceFile.CharacterPos;
                        EndTokPos = 1;
                        if (m_SourceFile.Peek() == '=')
                        {
                            m_SourceFile.GetNextChar();
                            EndTokPos++;
                            return new Token(Token.TOKENTYPE.GRTR_THAN_EQ, m_SourceFile.ExtractString(CurrTokPos, EndTokPos), m_SourceFile.Line);
                        }
                        return new Token(Token.TOKENTYPE.GRTR_THAN, m_SourceFile.ExtractString(CurrTokPos, EndTokPos), m_SourceFile.Line);
                    }
                case ':':
                    {
                        CurrTokPos = m_SourceFile.CharacterPos;
                        EndTokPos = 1;
                        if (m_SourceFile.Peek() == '=')
                        {
                            m_SourceFile.GetNextChar();
                            EndTokPos++;
                            return new Token(Token.TOKENTYPE.ASSIGN, m_SourceFile.ExtractString(CurrTokPos, EndTokPos), m_SourceFile.Line);
                        }
                        return new Token(Token.TOKENTYPE.COLON, m_SourceFile.ExtractString(CurrTokPos, EndTokPos), m_SourceFile.Line);
                    }
                case '"':
                    {
                        Token rtnTKN = LoadStringToken();
                        if (rtnTKN != null)
                            return rtnTKN;
                    }
                    break;
                case '(':
                    {
                        if (m_SourceFile.Peek() == '*')
                        {
                            if (RemoveComment())
                            {
                                return new Token(Token.TOKENTYPE.COMMENT_END, "comment", m_SourceFile.Line);
                            }
                        }
                        return new Token(Token.TOKENTYPE.LEFT_PAREN, cNextChar.ToString(), m_SourceFile.Line);


                    }
                case '.':
                    {
                        CurrTokPos = m_SourceFile.CharacterPos;
                        EndTokPos = 1;
                        if (m_SourceFile.Peek() == '.')
                        {
                            m_SourceFile.GetNextChar();
                            EndTokPos++;
                            return new Token(Token.TOKENTYPE.DOT_DOT, m_SourceFile.ExtractString(CurrTokPos, EndTokPos), m_SourceFile.Line);
                        }
                        else if (Char.IsNumber(m_SourceFile.Peek()))
                        {
                            m_SourceFile.GetNextChar();
                            EndTokPos++;
                            while (Char.IsNumber(m_SourceFile.Peek()))
                            {
                                m_SourceFile.GetNextChar();
                                EndTokPos++;
                            }
                            return new Token(Token.TOKENTYPE.REAL_NUM, "0" + m_SourceFile.ExtractString(CurrTokPos, EndTokPos), m_SourceFile.Line);
                        }
                        else return new Token(Token.TOKENTYPE.DOT, m_SourceFile.ExtractString(CurrTokPos, EndTokPos), m_SourceFile.Line);

                    }
                default:
                    break;
            }

            if (cNextChar == '_')
            {
                return LoadIDToken(cNextChar.ToString());
            }
            else if (Char.IsLetter(cNextChar))
            {
                return LoadIDToken("Letter");
            }

            if (Char.IsNumber(cNextChar))
            {
                return LoadNumToken();
            }

            // We only get here by mistake! Throw an exception.
            string strMsg = string.Format("Non-tokenizable character '{0}' at source line {1}.",
                cNextChar, m_SourceFile.Line);

            return new Token(Token.TOKENTYPE.ERROR, strMsg, m_SourceFile.Line);


        } // GetNextToken

        /// <summary>
        /// Method to scan an integer.
        /// PRE:  One digit has been scanned.
        /// POST: The full integer has been scanned and the token returned.
        /// </summary>
        Token LoadNumToken()
        {
            int CurrTknPos = m_SourceFile.CharacterPos;
            int EndTknPos = 1;

            while (Char.IsNumber(m_SourceFile.Peek()))
            {
                m_SourceFile.GetNextChar();
                EndTknPos++;
            }
            if (m_SourceFile.Peek() == '.')
            {
                m_SourceFile.GetNextChar();
                EndTknPos++;
                while (Char.IsNumber(m_SourceFile.Peek()))
                {
                    m_SourceFile.GetNextChar();
                    EndTknPos++;
                }
                return new Token(Token.TOKENTYPE.REAL_NUM, m_SourceFile.ExtractString(CurrTknPos, EndTknPos), m_SourceFile.Line);
            }
            else return new Token(Token.TOKENTYPE.INT_NUM, m_SourceFile.ExtractString(CurrTknPos, EndTknPos), m_SourceFile.Line);


        } // LoadNumToken

        /// <summary>
        /// Method to scan a string constant (like "Hello world").
        /// PRE:  A double quote mark has been scanned.
        /// POST: The full string constant has been scanned and the token returned.
        /// </summary>
        Token LoadStringToken()
        {
            int CurrTknPos = m_SourceFile.CharacterPos;
            int EndTknPos = 1;
            bool stringfound = true;

            while (m_SourceFile.Peek() != '"')
            {
                char check = m_SourceFile.GetNextChar(false);
                if (check == (char)65535)
                {
                    stringfound = false;
                    break;
                }
                EndTknPos++;

            }
            if (stringfound)
            {
                m_SourceFile.GetNextChar(false);
                EndTknPos++;
                string final = m_SourceFile.ExtractString(CurrTknPos, EndTknPos);
                final = final.TrimStart('"');
                final = final.TrimEnd('"');
                return new Token(Token.TOKENTYPE.STRING, final, m_SourceFile.Line);
            }
            else return null;
        } // LoadStringToken

        /// <summary>
        /// Remove a comment (* comment *).
        /// PRE:  (* has been scanned.
        /// POST: The full comment has been removed.
        /// </summary>
        bool RemoveComment()
        {
            char test = m_SourceFile.GetNextChar();
            while (test != (char)65535)
            {
                if (test == '*')
                {
                    if (m_SourceFile.Peek() == ')')
                    {
                        m_SourceFile.GetNextChar();
                        return true;
                    }

                }
                test = m_SourceFile.GetNextChar();
            } return false;

        } // RemoveComment

        /// <summary>
        /// Method to scan an identifier.
        /// PRE:  One letter has been scanned.
        /// POST: The full identifier has been scanned and the token returned. This method
        ///    correctly handles keywords by searching for them in the keyword table.
        /// </summary>
        Token LoadIDToken(string key)
        {
            int CurrTknPos = m_SourceFile.CharacterPos;
            int EndPos = 0;
            int state = 0;
            string selector = key;
            state = statetable[state][(int)statekeys[selector]];
            while (state != -1)
            {

                char value = m_SourceFile.GetNextChar();

                if (Char.IsLetter(value))
                {
                    selector = "Letter";
                }
                else if (value == '_')
                {
                    selector = value.ToString();
                }
                else if (Char.IsNumber(value))
                {
                    selector = "Num";
                }
                else selector = "NC";
                state = statetable[state][(int)statekeys[selector]];
                EndPos++;

            }

            string word = m_SourceFile.ExtractString(CurrTknPos, EndPos);
            m_SourceFile.PushBackChar();
            if (m_htKeywords.ContainsKey(word))
                return new Token((Token.TOKENTYPE)m_htKeywords[(string)word], word, m_SourceFile.Line);
            else return new Token(Token.TOKENTYPE.ID, word, m_SourceFile.Line);

        } // LoadIDToken

        /// <summary>
        /// PRE:  The source file has been set.
        /// POST: A string is returned that includes all tokens from the source file
        ///    attractively formatted. Exceptions are wisely handled and 
        ///    informatively shared with the user.
        /// </summary>
        public string ListTokens()
        {

            m_TableOfTokens.Clear();


            string rtnStr = "Token List\r\n\r\n";

            int tkncount = 0;
            bool canContinue = true;
            rtnStr += string.Format("{0,3} {1,-10} {2,-15} {3,30} \r\n", "TK",
                "Line", "Type", "Lexeme");
            rtnStr += "----------------------------------------------------------------------" + "\r\n";
            Token.TOKENTYPE test;
            do
            {
                Token tkn = GetNextToken();
                test = tkn.m_tokType;
                if (test == Token.TOKENTYPE.ERROR)
                {
                    canContinue = false;
                }
                else if (test == Token.TOKENTYPE.EOF)
                {
                    canContinue = false;
                }
                else if (test == Token.TOKENTYPE.COMMENT_END)
                {
                    continue;
                }
                string[] values = tkn.ToStringArray();
                
                m_TableOfTokens.Rows.Add(new object[] { tkncount,values[0],"Token "+values[1]+": "+values[2],values[3] });
                
                rtnStr += tkncount + "     " + tkn.ToString() + "\r\n";
                tkncount++;
            } while (canContinue);
            return rtnStr;

        } // ListTokens

    } // class Tokenizer
}