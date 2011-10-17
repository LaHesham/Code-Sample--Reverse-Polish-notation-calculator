using System;
using System.Collections.Generic;
using System.Globalization;

namespace RPNCalc.rpn
{
    /// <summary>
    /// Reverse Polish Notation calculation class.
    /// </summary>
    public class RPN
    {
        /// <summary>
        /// The string that is read from the input window.
        /// </summary>
        private String expression;
        /// <summary>
        /// Token queue, this queue is in infix notation. It is the expression string tokenized.
        /// </summary>
        private Queue<Token> tokenQueue;
        /// <summary>
        /// The operator stack is used during the building of the RPN Queue (Postfix notation).
        /// It's temporary storage in between operations.
        /// </summary>
        private Stack<Token> operatorStack;
        /// <summary>
        /// The RPN Queue is the arithmetic expression written in postfix notation and tokenized.
        /// </summary>
        private Queue<Token> rpnQueue;
        /// <summary>
        /// Result Stack is used during the final calculation of the arithmetic expression as a 
        /// storage place for numeric values.
        /// </summary>
        private Stack<double> resultStack;
        
        public RPN(String expression)
        {
            this.expression = expression;
            this.tokenQueue = new Queue<Token>();
            this.operatorStack = new Stack<Token>();
            this.rpnQueue = new Queue<Token>();
            this.resultStack = new Stack<double>();
        }

        /// <summary>
        /// Parses the input string into a <see cref="Token"/> <see cref="Queue"/>
        /// </summary>
        public void Parse()
        {
            expression = expression.Replace(" ", "");

            for (int i = 0; i <= expression.Length-1; i++)
            {
                //Recursive call counter for the operations retrieval function.
                int opCall = 0;
                //Recursive call counter for the number retrieval function.
                int numCall = 0; 
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
                    catch (System.Exception)
                    {
                        System.InvalidCastException ex = new InvalidCastException("Error: The number <" + rawToken + "> is not a valid number.");
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
                    else throw new System.InvalidOperationException("Error: Your operation <" + rawToken + "> is not supported.");
                }

            }
        }

        /// <summary>
        /// Gets the next operator if there is one.
        /// </summary>
        /// <param name="currentSymbol">The latest symbol being parsed.</param>
        /// <param name="pos">The position in the expression string.</param>
        /// <param name="call">Number of the recursive function call (0-no recursion).</param>
        /// <returns>Operator <see cref="string"/>.</returns>
        private string GetOperator(string currentSymbol, ref int pos, ref int call)
        {
            string rawToken = "";
            int currentNum;

            if (currentSymbol == "/" || currentSymbol == "*" || currentSymbol == "-" || currentSymbol == "+")
            {
                return currentSymbol;
            }
            else if ((currentSymbol == ")" || currentSymbol == "(") && call == 0)
            {
                return currentSymbol;
            }
            else if ((currentSymbol == ")" || currentSymbol == "(") && call != 0)
            {
                pos--;
                return rawToken;
            }
            else if (int.TryParse(currentSymbol, out currentNum))
            {
                //Step back, these re not the droids you are looking for.
                pos--;
                return rawToken;
            }
            else
            {
                //Perhaps, go deeper if this is not the end.
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

        /// <summary>
        /// Gets the next number if there is one.
        /// </summary>
        /// <param name="currentSymbol">The latest symbol being parsed.</param>
        /// <param name="pos">The position in the expression string.</param>
        /// <param name="call">Number of the recursive function call (0-no recursion).</param>
        /// <returns>Number as a <see cref="string"/> or an empty <see cref="string"/> if  it's not a number.</returns>
        private string GetNumber(string currentSymbol, ref int pos, ref int call)
        {
            int currentNum;
            string rawToken = "";

            bool isNum = int.TryParse(currentSymbol, out currentNum);

            if (isNum)
            {
                //Perhaps, go deeper if this is not the end.
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
            else if (currentSymbol == ".")
            {
                //Perhaps, go deeper if this is not the end.
                rawToken = rawToken + currentSymbol;
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
            else if (!isNum && call == 0)
            {
                //Not a number.
                return rawToken;
            }
            else
            {
                //Step back, these re not the droids you are looking for.
                pos--;
            }
            return rawToken;
        }

        /// <summary>
        /// Builds the RPN Queue.
        /// Implementation of the Shunting-yard algorithm.
        /// </summary>
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

        /// <summary>
        /// Calculates the expression .
        /// </summary>
        /// <returns>Final result as <see cref="double"/></returns>
        public double CalculateExpression()
        {
            this.Parse();
            this.BuildRPNQUeue();

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
