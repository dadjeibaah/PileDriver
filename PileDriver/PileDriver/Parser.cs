using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PileDriver
{
    internal class Parser
    {
        private Emitter CodeEmitt;
        private static Parser Parseobj;
        private static Object parse_lock = typeof(Parser);
        private Token currToken;
        private Token NextToken;
        private Tokenizer tokenizerobj = Tokenizer.GetTokenizer();
        private SymbolTable table = SymbolTable.Instance();
        private string[] orderop = new string[] { "(", "DIV", "MOD", "*", "+", "-", "NOT", ">", "<=", ">=", "<", "=", "<>", "AND", "OR" };    //Array for the order of operations
        private Queue<string> savedExp = new Queue<string>();

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
            table.Reset();
            CodeEmitt.EmitterReset();
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
            else OutputErrorEx();
        }

        private void OutputErrorEx()
        {
            throw new Exception(String.Format("Parse Error: Error in line {0}, Somewhere near {1}", currToken.m_iLineNum, currToken.m_strName));
        }

        //Expected
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
            CodeEmitt.MainMemoryUse = table.TotalMemory() + 4;
        }//ProgramMod

        /// <summary>
        /// Method: StatementSequence
        /// Reads the each token and translates each line into a number of instructions for the assembler
        /// eg (processes write strings statements write lines and so on.
        /// </summary>
        public void statementSequence()
        {
            if (currToken.m_tokType == Token.TOKENTYPE.WRSTR)
            {
                Expected(Token.TOKENTYPE.WRSTR);
                Expected(Token.TOKENTYPE.LEFT_PAREN);
                CodeEmitt.WRSTR(currToken.m_strName);
                Expected(Token.TOKENTYPE.STRING);
                Expected(Token.TOKENTYPE.RIGHT_PAREN);
                Expected(Token.TOKENTYPE.SEMI_COLON);
            }
            else if (currToken.m_tokType == Token.TOKENTYPE.WRLN)
            {
                CodeEmitt.WRLN();
                Expected(Token.TOKENTYPE.WRLN);
                Expected(Token.TOKENTYPE.SEMI_COLON);
            }
            else if (currToken.m_tokType == Token.TOKENTYPE.WRINT)
            {
                Expected(Token.TOKENTYPE.WRINT);
                Expression();
                CodeEmitt.WRINT();
                //Expected(Token.TOKENTYPE.RIGHT_PAREN);
                Expected(Token.TOKENTYPE.SEMI_COLON);
            }
            else if (currToken.m_tokType == Token.TOKENTYPE.ID)
            {
                ATTRIBUTE id4Assign;
                bool bisPresent;
                table.FindInScope(currToken.m_strName, out id4Assign, out bisPresent);
                if (!bisPresent)
                {
                    throw new Exception(String.Format("Parse Error: Error in line {0}, Somewhere near {1}", currToken.m_iLineNum, currToken.m_strName));
                }
                Expected(Token.TOKENTYPE.ID);
                if (currToken.m_tokType == Token.TOKENTYPE.LEFT_BRACK)
                {
                    Expected(Token.TOKENTYPE.LEFT_BRACK);
                    Expression();
                    CodeEmitt.SelectArrayElement(id4Assign.ArraySt, id4Assign.Offset);
                    Expected(Token.TOKENTYPE.RIGHT_BRACK);
                }

                Expected(Token.TOKENTYPE.ASSIGN);
                if (currToken.m_tokType == Token.TOKENTYPE.RDINT)
                {
                    CodeEmitt.RDINT();
                    Expected(Token.TOKENTYPE.RDINT);
                    Expected(Token.TOKENTYPE.LEFT_PAREN);
                    Expected(Token.TOKENTYPE.RIGHT_PAREN);
                }
                else
                {
                    Expression();
                }
                if (id4Assign.ATTR_T == ATTR_TYPE.ARRAY)
                {
                    CodeEmitt.AssignElement();
                }
                else
                {
                    CodeEmitt.AssignVariable(id4Assign.Offset.ToString());
                }
                Expected(Token.TOKENTYPE.SEMI_COLON);
            }
            else if (currToken.m_tokType == Token.TOKENTYPE.IF)
            {
                Expected(Token.TOKENTYPE.IF);
                Expression();
                Expected(Token.TOKENTYPE.THEN);
                CodeEmitt.FinalLogEval();
                CodeEmitt.StartIf();
                CodeEmitt.TrueSection();
                if (currToken.m_tokType != Token.TOKENTYPE.ELSE)
                {
                    while (currToken.m_tokType != Token.TOKENTYPE.ELSE)
                    {
                        statementSequence();
                        if (currToken.m_tokType == Token.TOKENTYPE.END)
                        {
                            goto END;
                        }
                    }

                    CodeEmitt.JmpToEnd();
                    Expected(Token.TOKENTYPE.ELSE);
                END:
                    CodeEmitt.FalseSection();
                    while (currToken.m_tokType != Token.TOKENTYPE.END)
                    {
                        statementSequence();
                    }
                }

                Expected(Token.TOKENTYPE.END);
                Expected(Token.TOKENTYPE.SEMI_COLON);
                CodeEmitt.EndLoop();
            }
            else if (currToken.m_tokType == Token.TOKENTYPE.LOOP)
            {
                table.IncrementScope();
                Expected(Token.TOKENTYPE.LOOP);
                CodeEmitt.WhileCond();
                while (currToken.m_tokType != Token.TOKENTYPE.IF)
                {
                    statementSequence();
                }
                Expected(Token.TOKENTYPE.IF);

                Expression();
                Expected(Token.TOKENTYPE.THEN);
                Expected(Token.TOKENTYPE.EXIT);
                Expected(Token.TOKENTYPE.SEMI_COLON);
                Expected(Token.TOKENTYPE.END);
                Expected(Token.TOKENTYPE.SEMI_COLON);
                CodeEmitt.FinalLogEval();
                CodeEmitt.CmpToEnd();
                //CodeEmitt.StartWhile();
                //CodeEmitt.WhileMain();
                while (currToken.m_tokType != Token.TOKENTYPE.END)
                {
                    statementSequence();
                }
                //CodeEmitt.WhileCond();
                // EvaluatePostFix(savedExp);

                CodeEmitt.EndWhile();
                Expected(Token.TOKENTYPE.END);
                Expected(Token.TOKENTYPE.SEMI_COLON);
                table.DecrementScope();
            }
        }//StatementSequence

        /// <summary>
        /// Method: Block
        /// processes the code that is before the begin line in the source code file. It
        /// handles things like variable declaration, constant declaration etc.
        /// </summary>
        private void Block()
        {
            while (currToken.m_tokType != Token.TOKENTYPE.BEGIN)
            {
                if (currToken.m_tokType == Token.TOKENTYPE.VAR)
                {
                    Expected(Token.TOKENTYPE.VAR);
                    Declaration();
                }
                else if (currToken.m_tokType == Token.TOKENTYPE.TYPE)
                {
                    Expected(Token.TOKENTYPE.TYPE);
                    TypeDeclaration();
                }
                else if (currToken.m_tokType == Token.TOKENTYPE.CONST)
                {
                    Expected(Token.TOKENTYPE.CONST);
                    ConstDeclaration();
                }
            }

            Expected(Token.TOKENTYPE.BEGIN);
            while (currToken.m_tokType != Token.TOKENTYPE.END)
            {
                statementSequence();
            }
            Expected(Token.TOKENTYPE.END);
        }//Block

        /// <summary>
        /// ConstDeclaration
        /// Specifically handles processing lines that contain constant declarations.
        /// This method creates symbols and assigns them to the symbol table and also
        /// reflects that in the assembler integer stack.
        /// </summary>
        private void ConstDeclaration()
        {
            while (currToken.m_tokType != Token.TOKENTYPE.VAR && currToken.m_tokType != Token.TOKENTYPE.TYPE)
            {
                ATTRIBUTE id4Assign;
                bool bisPresent;
                table.FindInScope(currToken.m_strName, out id4Assign, out bisPresent);
                if (bisPresent)
                {
                    throw new Exception(String.Format("Parse Error: Error in line {0}, Somewhere near {1}", currToken.m_iLineNum, currToken.m_strName));
                }
                Token IDToken = currToken;
                Expected(Token.TOKENTYPE.ID);
                Expected(Token.TOKENTYPE.EQUAL);
                id4Assign = new ATTRIBUTE(IDToken.m_strName, table.GetCurrScope(), Int32.Parse(currToken.m_strName), true);
                table.AddSymbol(id4Assign);
                CodeEmitt.AddtoIntStack(id4Assign.DisplayValue());
                CodeEmitt.AssignVariable(id4Assign.Offset.ToString());
                Expected(Token.TOKENTYPE.INT_NUM);
                Expected(Token.TOKENTYPE.SEMI_COLON);
            }
        }//ConstDeclaration

        /// <summary>
        /// TypeDeclaration
        /// Creates variables of type array and adds them to the symbol table.
        ///
        /// </summary>
        private void TypeDeclaration()
        {
            //Check for the type that is being declared for an ID
            ATTRIBUTE typeid;
            int[] value = new int[2];
            string idname = currToken.m_strName;
            Expected(Token.TOKENTYPE.ID);
            Expected(Token.TOKENTYPE.EQUAL);
            Expected(Token.TOKENTYPE.ARRAY);
            Expected(Token.TOKENTYPE.LEFT_BRACK);
            int startval;
            int endval;
            CheckArrayBound(currToken, out startval);
            Expected(Token.TOKENTYPE.DOT_DOT);
            CheckArrayBound(currToken, out endval);
            Expected(Token.TOKENTYPE.RIGHT_BRACK);
            Expected(Token.TOKENTYPE.OF);
            Expected(Token.TOKENTYPE.INTEGER);
            value[0] = startval;
            value[1] = endval;
            typeid = new ATTRIBUTE(idname, table.GetCurrScope(), value);
            table.AddSymbol(typeid);
            Expected(Token.TOKENTYPE.SEMI_COLON);
        }

        private void CheckArrayBound(Token tok, out int val)
        {
            if (currToken.m_tokType == Token.TOKENTYPE.ID)
            {
                ATTRIBUTE id4Assign;
                bool isResult;
                table.FindInScope(tok.m_strName, out id4Assign, out isResult);
                if (!isResult)
                {
                    throw new Exception(String.Format("Parse Error: Error in line {0}, Somewhere near {1}", currToken.m_iLineNum, currToken.m_strName));
                }
                Int32.TryParse(id4Assign.DisplayValue(), out val);
                Expected(Token.TOKENTYPE.ID);
            }
            else
            {
                Int32.TryParse(currToken.m_strName, out val);
                Expected(Token.TOKENTYPE.INT_NUM);
            }
        }

        /// <summary>
        /// Declaration
        /// This method creates variables and adds them to the symbol table and
        /// </summary>
        private void Declaration()
        {
            ArrayList identList = new ArrayList();
            while (currToken.m_tokType != Token.TOKENTYPE.COLON && currToken.m_tokType != Token.TOKENTYPE.BEGIN)
            {
                identList.Add(currToken);
                Expected(Token.TOKENTYPE.ID);
                if (currToken.m_tokType == Token.TOKENTYPE.COMMA)
                {
                    Expected(Token.TOKENTYPE.COMMA);
                    continue;
                }

                Expected(Token.TOKENTYPE.COLON);
                if (currToken.m_tokType == Token.TOKENTYPE.INTEGER)
                {
                    Expected(Token.TOKENTYPE.INTEGER);
                    foreach (Token ident in identList)
                    {
                        table.AddSymbol(new ATTRIBUTE(ident.m_strName, table.GetCurrScope(), ATTR_TYPE.INT));
                    }
                }
                else if (currToken.m_tokType == Token.TOKENTYPE.ID)
                {
                    ATTRIBUTE id4Assign;
                    bool isPresent;
                    table.FindInScope(currToken.m_strName, out id4Assign, out isPresent);
                    if (!isPresent)
                    {
                        OutputErrorEx();
                    }
                    foreach (Token typeident in identList)
                    {
                        table.AddSymbol(new ATTRIBUTE(typeident.m_strName, table.GetCurrScope(), id4Assign));
                    }
                    Expected(Token.TOKENTYPE.ID);
                }

                Expected(Token.TOKENTYPE.SEMI_COLON);
                identList.Clear();
            }
        }//Declaration

        /// <summary>
        /// This method is used to compare the precedence level between two operators.
        /// The operators are strings and the comparison is determined by comparing the positions
        /// of the operators in the order of operations array above.
        /// </summary>
        /// <param name="one">this is the first operator to be compared</param>
        /// <param name="two">second operator to be compared</param>
        /// <returns>
        /// Returns true if the first operand has a higher precedence than the second one.
        /// Returns false if the first operand does not have a higher precedence.
        /// </returns>
        private bool CheckOrderOp(string one, string two)
        {
            if (one.Equals("+") && two.Equals("-"))
                return false;
            else if (one.Equals("-") && two.Equals("+"))
                return false;
            else if (one.Equals("<>") && two.Equals("="))
                return false;
            else if (one.Equals("=") && two.Equals("<>"))
                return false;
            else if (Array.IndexOf(orderop, one) <= Array.IndexOf(orderop, two))
                return true;
            else return false;
        }//CheckOrderOp

        /// <summary>
        /// ConvertToPostFix
        /// This method goes through an expression in the source code and converts it to
        /// reflect a postfix representation.
        /// </summary>
        /// <returns>
        /// returns a queue collection that represents the expression in postfix notation.
        /// </returns>
        private Queue<string> ConvertToPostFix()
        {
            bool[] Negmap = new bool[] { true, false }; //bitmap to be used to detect negative numbers in an expression
            Stack<string> operatorStack = new Stack<string>();
            Queue<string> postfixStack = new Queue<string>();

            while (currToken.m_tokType != Token.TOKENTYPE.SEMI_COLON && currToken.m_tokType != Token.TOKENTYPE.THEN && currToken.m_tokType != Token.TOKENTYPE.RIGHT_BRACK)
            {
                if (currToken.m_tokType == Token.TOKENTYPE.MINUS && Negmap[0])
                {
                    string negvalue = currToken.m_strName;
                    Expected(Token.TOKENTYPE.MINUS);
                    if (currToken.m_tokType == Token.TOKENTYPE.ID || currToken.m_tokType == Token.TOKENTYPE.INT_NUM)
                    {
                        Negmap[0] = false;
                        Negmap[1] = false;
                        negvalue += currToken.m_strName;
                        Expected(currToken.m_tokType);
                        postfixStack.Enqueue(negvalue);
                    }
                }
                if (currToken.m_tokType == Token.TOKENTYPE.INT_NUM)
                {
                    postfixStack.Enqueue(currToken.m_strName);
                    Expected(Token.TOKENTYPE.INT_NUM);
                    Negmap[0] = false;
                    Negmap[1] = false;
                }
                else if (currToken.m_tokType == Token.TOKENTYPE.ID)
                {
                    ATTRIBUTE id;
                    bool isPresent;
                    table.FindInScope(currToken.m_strName, out id, out isPresent);

                    if (id.ATTR_T == ATTR_TYPE.ARRAY)
                    {
                        postfixStack.Enqueue(currToken.m_strName);
                        Expected(Token.TOKENTYPE.ID);
                        Expected(Token.TOKENTYPE.LEFT_BRACK);
                        Expression();
                        Expected(Token.TOKENTYPE.RIGHT_BRACK);
                    }
                    else
                    {
                        postfixStack.Enqueue(currToken.m_strName);
                        Expected(Token.TOKENTYPE.ID);
                    }
                    Negmap[0] = false;
                    Negmap[1] = false;
                }
                else if (currToken.m_tokType == Token.TOKENTYPE.PLUS || currToken.m_tokType == Token.TOKENTYPE.MINUS ||
                        currToken.m_tokType == Token.TOKENTYPE.DIV ||
                        currToken.m_tokType == Token.TOKENTYPE.MULT || currToken.m_tokType == Token.TOKENTYPE.MOD ||
                        currToken.m_tokType == Token.TOKENTYPE.LEFT_PAREN || currToken.m_tokType == Token.TOKENTYPE.GRTR_THAN ||
                        currToken.m_tokType == Token.TOKENTYPE.EQUAL || currToken.m_tokType == Token.TOKENTYPE.LESS_THAN_EQ ||
                        currToken.m_tokType == Token.TOKENTYPE.GRTR_THAN_EQ || currToken.m_tokType == Token.TOKENTYPE.LESS_THAN ||
                        currToken.m_tokType == Token.TOKENTYPE.NOT || currToken.m_tokType == Token.TOKENTYPE.NOT_EQ ||
                        currToken.m_tokType == Token.TOKENTYPE.OR || currToken.m_tokType == Token.TOKENTYPE.AND)
                {
                    //if they are no operators in the operator stack
                    if (!operatorStack.Any())
                    {
                        operatorStack.Push(currToken.m_strName);
                        Expected(currToken.m_tokType);
                    }
                    else
                    {
                        //if the current operator has a higher preference
                        if (CheckOrderOp(currToken.m_strName, operatorStack.Peek()))
                        {
                            operatorStack.Push(currToken.m_strName);
                            Expected(currToken.m_tokType);
                        }
                        else
                        {
                            //make sure that there are operators, that the current operator does not have a higher precedence and there is no parantheses
                            while (operatorStack.Any() && !CheckOrderOp(currToken.m_strName, operatorStack.Peek()) && !operatorStack.Peek().Equals("("))
                            {
                                postfixStack.Enqueue(operatorStack.Pop());
                            }

                            operatorStack.Push(currToken.m_strName);
                            Expected(currToken.m_tokType);
                        }
                    }
                    Negmap[0] = true;
                    Negmap[1] = false;
                }
                else if (currToken.m_tokType == Token.TOKENTYPE.LEFT_PAREN)
                {
                    Expected(Token.TOKENTYPE.LEFT_PAREN);
                    Queue<string> queue = ConvertToPostFix();
                    foreach (string str in queue)
                    {
                        postfixStack.Enqueue(str);
                    }
                }
                if (currToken.m_tokType == Token.TOKENTYPE.RIGHT_PAREN)
                {
                    if (operatorStack.Any())
                    {
                        while (!operatorStack.Peek().Equals("("))
                        {
                            postfixStack.Enqueue(operatorStack.Pop());
                        }
                        operatorStack.Pop();
                    }
                    Expected(Token.TOKENTYPE.RIGHT_PAREN);
                }

                if (operatorStack.Any() && currToken.m_tokType == Token.TOKENTYPE.SEMI_COLON || currToken.m_tokType == Token.TOKENTYPE.THEN)
                {
                    while (operatorStack.Count != 0)
                        postfixStack.Enqueue(operatorStack.Pop());
                }
            }
            return postfixStack;
        }//ConvertToPostfix

        /// <summary>
        /// EvaluatePostFix
        /// Evaluates a postfix "stack"
        /// </summary>
        /// <param name="stack">queue that has the expression in postfix notation</param>
        private void EvaluatePostFix(Queue<string> stack)
        {
            int num;
            while (stack.Count != 0)
            {
                string value = stack.Dequeue();

                //dealing with negative numbers.
                if (value[0] == '-' && value.Length > 1)
                {
                    if (Char.IsLetter(value[1]))
                    {
                        ATTRIBUTE entry = new ATTRIBUTE();
                        bool bPresent = false;
                        table.FindInScope(value.Substring(1), out entry, out bPresent);
                        CodeEmitt.NegToStack(String.Format("[BP+{0}]", entry.Offset));
                    }
                    else if (Int32.TryParse(value, out num))
                    {
                        CodeEmitt.AddtoIntStack(num.ToString());
                    }
                }
                else if (Int32.TryParse(value, out num))
                {
                    CodeEmitt.AddtoIntStack(num.ToString());
                }
                else if (value.Equals("DIV"))
                {
                    CodeEmitt.DivOperation();
                }
                else if (value.Equals("MOD"))
                {
                    CodeEmitt.ModOperation();
                }
                else if (value.Equals("NOT"))
                {
                    CodeEmitt.NOTOperation();
                }
                else if (value.Equals("AND"))
                {
                    CodeEmitt.ANDOperation();
                }
                else if (value.Equals("OR"))
                {
                    CodeEmitt.OROperation();
                }
                else if (Char.IsLetter(value, 0))
                {
                    ATTRIBUTE entry = new ATTRIBUTE();
                    bool bPresent = false;
                    table.FindInScope(value, out entry, out bPresent);
                    if (!bPresent)
                    {
                        throw new Exception(String.Format("Parse Error"));
                    }

                    if (entry.ATTR_T == ATTR_TYPE.ARRAY)
                    {
                        CodeEmitt.SelectArrayElement(entry.ArraySt, entry.Offset);
                        CodeEmitt.AddtoIntStack("[BP+SI]");
                    }
                    else
                    {
                        CodeEmitt.AddtoIntStack(String.Format("[BP+{0}]", entry.Offset));
                    }
                }
                else if (value.Equals("+"))
                {
                    CodeEmitt.AddOperation();
                }
                else if (value.Equals("-"))
                {
                    CodeEmitt.SubOperation();
                }
                else if (value.Equals("*"))
                {
                    CodeEmitt.MulOperation();
                }

                else if (value.Equals(">"))
                {
                    CodeEmitt.CompareInsstruction();
                    CodeEmitt.LogicOperation(Token.TOKENTYPE.GRTR_THAN);
                }
                else if (value.Equals("<="))
                {
                    CodeEmitt.CompareInsstruction();
                    CodeEmitt.LogicOperation(Token.TOKENTYPE.LESS_THAN_EQ);
                }
                else if (value.Equals(">="))
                {
                    CodeEmitt.CompareInsstruction();
                    CodeEmitt.LogicOperation(Token.TOKENTYPE.GRTR_THAN_EQ);
                }
                else if (value.Equals("<"))
                {
                    CodeEmitt.CompareInsstruction();
                    CodeEmitt.LogicOperation(Token.TOKENTYPE.LESS_THAN);
                }
                else if (value.Equals("<>"))
                {
                    CodeEmitt.CompareInsstruction();
                    CodeEmitt.LogicOperation(Token.TOKENTYPE.NOT_EQ);
                }
                else if (value.Equals("="))
                {
                    CodeEmitt.CompareInsstruction();
                    CodeEmitt.LogicOperation(Token.TOKENTYPE.EQUAL);
                }
            }
        }//EvaluatePostFix

        /// <summary>
        /// Expression
        /// Converts to postfix and evaluates the expression
        /// Reflects this changes in the assembler or Emitter
        /// </summary>
        private void Expression(bool saveexp = false)
        {
            Queue<string> queue = ConvertToPostFix();
            if (saveexp)
            {
                savedExp = new Queue<string>(queue);
            }
            EvaluatePostFix(queue);
        }//Expression
    }
}