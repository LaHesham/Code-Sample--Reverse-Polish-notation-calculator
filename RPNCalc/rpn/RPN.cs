using System;
using System.Collections.Generic;
using System.Globalization;

namespace RPNCalc.rpn
{
    class RPN
    {
        private String expression;
        private Queue<Token> tokenQueue;
        private Stack<Token> operatorStack;
        private Queue<Token> rpnQueue;
        private Stack<double> resultStack;
        

        public RPN(String expression)
        {
            this.expression = expression;
            this.tokenQueue = new Queue<Token>();
            this.operatorStack = new Stack<Token>();
            this.rpnQueue = new Queue<Token>();
            this.resultStack = new Stack<double>();
        }

        public String Test()
        {
            return tokenQueue.ToString();
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
                        wholeCurrentNum = double.Parse(rawToken, CultureInfo.InvariantCulture);
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }

                    tokenQueue.Enqueue(new Token(wholeCurrentNum,TokenType.Number));
                }
                else 
                { 
                    rawToken = GetOperator(currentSymbol, ref i, ref opCall);
                    if (rawToken == "+") tokenQueue.Enqueue(new Token(TokenType.Plus));
                    else if (rawToken == "-") tokenQueue.Enqueue(new Token(TokenType.Minus));
                    else if (rawToken == "–") tokenQueue.Enqueue(new Token(TokenType.Minus)); // As pasted from word ... just to be sure.        
                    else if (rawToken == "/") tokenQueue.Enqueue(new Token(TokenType.Divide));
                    else if (rawToken == "*") tokenQueue.Enqueue(new Token(TokenType.Multiply));
                    else if (rawToken == "(") tokenQueue.Enqueue(new Token(TokenType.LeftParenthesis));
                    else if (rawToken == ")") tokenQueue.Enqueue(new Token(TokenType.RightParenthesis));
                    else if (rawToken == "exp") tokenQueue.Enqueue(new Token(TokenType.Exponent));
                    else if (rawToken == "sqrt") tokenQueue.Enqueue(new Token(TokenType.SquareRoot));
                    else if (rawToken == "lg") tokenQueue.Enqueue(new Token(TokenType.NaturalLogarithm));
                    else if (rawToken == "abs") tokenQueue.Enqueue(new Token(TokenType.AbsoluteValue));
                    else if (rawToken == "") ;
                    else throw new Exception();
                }

            }
        }

        private string GetOperator(string currentSymbol, ref int pos, ref int call)
        {
            string rawToken = "";
            int currentNum;

            if (currentSymbol == "/" || currentSymbol == "*" || currentSymbol == "-" || currentSymbol == "+")
            {
                return currentSymbol;
            }
            else if ((currentSymbol == ")" || currentSymbol == "(") && call == 1)
            {
                return currentSymbol;
            }
            else if ((currentSymbol == ")" || currentSymbol == "(") && call != 1)
            {
                pos--;
                return rawToken;
            }
            else if (int.TryParse(currentSymbol, out currentNum))
            {
                pos--;
                return rawToken;
            }
            else
            {
                rawToken = rawToken + currentSymbol;
                if ((pos + 1) < expression.Length - 1)
                {
                    pos++;
                    call++;
                    var nextSymbol = expression[pos].ToString();
                    rawToken = rawToken + GetOperator(nextSymbol, ref pos, ref call);
                }
                else
                {
                    return rawToken;
                }
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
                if ((pos+1) < expression.Length-1)
                {
                    pos++;
                    call++;
                    var nextSymbol = expression[pos].ToString();
                    rawToken = rawToken + GetNumber(nextSymbol, ref pos, ref call);
                }
                else
                {
                    return rawToken;
                }
                
            }
            else if (num == ".")
            {
                rawToken = rawToken + num;
                if ((pos+1) < expression.Length-1)
                {
                    pos++;
                    call++;
                    var nextSymbol = expression[pos].ToString();
                    rawToken = rawToken + GetNumber(nextSymbol, ref pos, ref call); 
                }
                else
                {
                    return rawToken;
                }
            }
            else if (!isNum && call == 1)
            {
                return rawToken;
            }
            else
            {
                pos--;
            }
            return rawToken;
        }

        public void BuildRPNQUeue()
        {
            while (tokenQueue.Count !=0 )
            {
                Token tkn = tokenQueue.Dequeue();

                if (!tkn.IsOperator)
                {
                    rpnQueue.Enqueue(tkn);
                }
                else if (operatorStack.Count == 0) operatorStack.Push(tkn);
                else if (tkn.PTokenType == TokenType.LeftParenthesis)
                {
                    operatorStack.Push(tkn);
                }
                else if (tkn.PTokenType == TokenType.RightParenthesis)
                {
                    while (operatorStack.Peek().PTokenType != TokenType.LeftParenthesis)
                        rpnQueue.Enqueue(operatorStack.Pop());
                    operatorStack.Pop();
                }
                else if (operatorStack.Peek().OperationPriority < tkn.OperationPriority)
                {
                    operatorStack.Push(tkn);
                }
                else if (operatorStack.Peek().OperationPriority > tkn.OperationPriority)
                {
                    if (operatorStack.Peek().PTokenType == TokenType.Exponent)
                    {
                        while (operatorStack.Peek().PTokenType == TokenType.Exponent)
                            rpnQueue.Enqueue(operatorStack.Pop());
                        rpnQueue.Enqueue(tkn);
                    }
                    rpnQueue.Enqueue(operatorStack.Pop());
                    rpnQueue.Enqueue(tkn);
                }
                else if ((operatorStack.Peek().OperationPriority == tkn.OperationPriority) && tkn.PTokenType != TokenType.Exponent)
                {
                    rpnQueue.Enqueue(operatorStack.Pop());
                    operatorStack.Push(tkn);
                }
                else if ((operatorStack.Peek().OperationPriority == tkn.OperationPriority) && tkn.PTokenType == TokenType.Exponent)
                {
                    operatorStack.Push(tkn);
                }

            }
            while (operatorStack.Count != 0)
                rpnQueue.Enqueue(operatorStack.Pop());
        }

        public double CalculateExpression()
        {
            while (rpnQueue.Count != 0)
            {
                if (!rpnQueue.Peek().IsOperator) resultStack.Push(rpnQueue.Dequeue().PTokenValue);
                else if ((rpnQueue.Peek().PTokenType == TokenType.SquareRoot) || (rpnQueue.Peek().PTokenType == TokenType.NaturalLogarithm) || 
                    (rpnQueue.Peek().PTokenType == TokenType.AbsoluteValue))
                {
                    resultStack.Push(rpnQueue.Dequeue().Calculate(resultStack.Pop()));
                }
                else
                {
                    double operand2 = resultStack.Pop();
                    double operand1 = resultStack.Pop();

                    resultStack.Push(rpnQueue.Dequeue().Calculate(operand1, operand2));
                }
            }

            return resultStack.Pop();
        }
    }
}
