using System;
using System.Collections.Generic;

namespace RPNCalc.rpn
{
    class RPN
    {
        private String expression;
        private Stack<Token> tokenStack;
        

        public RPN(String expression)
        {
            this.expression = expression;
            this.tokenStack = new Stack<Token>();
        }

        public String Test()
        {
            return tokenStack.ToString();
        }

        public void Parse()
        {
            expression = expression.Replace(" ", "");

            for (int i = 0; i <= expression.Length-1; i++)
            {
                int opCall = 1;
                int numCall = 1;
                double wholeCurrentNum;
                string rawToken = "";
                String currentSymbol = expression[i].ToString();
                rawToken = GetNumber(currentSymbol,ref i, ref numCall);
                if (rawToken != "")
                {
                    try
                    {
                        wholeCurrentNum = double.Parse(rawToken);
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }

                    tokenStack.Push(new Token(wholeCurrentNum,TokenType.Number));
                }
                else 
                { 
                    rawToken = GetOperator(currentSymbol, ref i, ref opCall);
                    if (rawToken == "+") tokenStack.Push(new Token(TokenType.Plus));
                    else if (rawToken == "-") tokenStack.Push(new Token(TokenType.Minus));
                    else if (rawToken == "/") tokenStack.Push(new Token(TokenType.Divide));
                    else if (rawToken == "*") tokenStack.Push(new Token(TokenType.Multiply));
                    else if (rawToken == "^") tokenStack.Push(new Token(TokenType.Exponent));
                    else if (rawToken == "sqrt") tokenStack.Push(new Token(TokenType.SquareRoot));
                    else if (rawToken == "sin") tokenStack.Push(new Token(TokenType.Sine));
                    else if (rawToken == "cos") tokenStack.Push(new Token(TokenType.Cosine));
                    else if (rawToken == "tan") tokenStack.Push(new Token(TokenType.Tangent));
                    else if (rawToken == "lg") tokenStack.Push(new Token(TokenType.NaturalLogarithm));
                    else if (rawToken == "abs") tokenStack.Push(new Token(TokenType.AbsoluteValue));
                    else throw new Exception();
                }

            }
        }

        private string GetOperator(string currentSymbol, ref int pos, ref int call)
        {
            string rawToken = "";
            int currentNum;

            if (currentSymbol == "/")
            {
                return currentSymbol;
            }
            else if (currentSymbol == "*")
            {
                return currentSymbol;
            }
            else if (currentSymbol == "-")
            {
                return currentSymbol;
            }
            else if (currentSymbol == "+")
            {
                return currentSymbol;
            }
            else if (currentSymbol == "^")
            {
                return currentSymbol;
            }
            else if (int.TryParse(currentSymbol, out currentNum))
            {
                pos--;
                return rawToken;
            }
            else if (currentSymbol == ")" || currentSymbol == "(")
            {
                pos++;
                call++;
                var nextSymbol = expression[pos].ToString();
                GetOperator(nextSymbol, ref pos,ref call);
            }
            else
            {
                rawToken = rawToken + currentSymbol;
                pos++;
                call++;
                var nextSymbol = expression[pos].ToString();
                GetOperator(nextSymbol, ref pos, ref call);
            }

            return rawToken;
        }

        private string GetNumber(string num, ref int pos, ref int call)
        {
            int currentNum;
            string rawToken = "";
            bool isNum = int.TryParse(num, out currentNum);
            if (isNum)
            {
                rawToken = rawToken + currentNum;
                pos++;
                call++;
                var nextSymbol = expression[pos].ToString();
                GetNumber(nextSymbol, ref pos, ref call);
            }
            else if (num == ".")
            {
                rawToken = rawToken + currentNum;
                pos++;
                call++;
                var nextSymbol = expression[pos].ToString();
                GetNumber(nextSymbol, ref pos, ref call);
            }
            else if (!isNum && call == 2) return rawToken;
            else
            {
                pos--;
            }
            return rawToken;
        }
    }
}
