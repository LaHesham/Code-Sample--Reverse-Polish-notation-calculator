using System;

namespace RPNCalc.rpn
{
    /// <summary>
    /// Main building block in the reverse polish notation.
    /// A token can be an operation or an operand.
    /// </summary>
    public class Token
    {
        private double tokenValue;
        private TokenType tokenType;
        private int operationPriority;
        private bool isOperator;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is an operator.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is an operator; otherwise, <c>false</c>.
        /// </value>
        public bool IsOperator
        {
            get
            {
                return isOperator;
            }
            set
            {
                isOperator = value;
            }
        }

        /// <summary>
        /// Gets or sets the operation priority.
        /// The higher the number the higher the priority.
        /// e.g. Priority 4 > Priority 1
        /// </summary>
        /// <value>
        /// The operation priority.
        /// Left Parenthesis, Right Parenthesis - 0 (actually highest, this is for the sake of implementation)
        /// Add, Subtract - 1
        /// Multiply, Divide - 2
        /// Power, Square Root - 3
        /// Natural Logarithm, Absolute Value - 4
        /// Unary Minus - 5
        /// </value>
        public int OperationPriority
        {
            get
            {
                return operationPriority;
            }
            set
            {
                operationPriority = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the token.
        /// </summary>
        /// <value>
        /// The type of the  token. See <see cref="TokenType"/>
        /// </value>
        public TokenType PTokenType
        {
            get
            {
                return tokenType;
            }
            set
            {
                tokenType = value;
            }
        }

        /// <summary>
        /// Gets or sets the token value.
        /// To be used only if the token is a number
        /// </summary>
        /// <value>
        /// The token value.
        /// </value>
        public double PTokenValue
        {
            get
            {
                return tokenValue;
            }
            set
            {
                tokenValue = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// This constructor is used when creating a number token.
        /// </summary>
        /// <param name="numberValue">The number value.</param>
        /// <param name="type">The type of the token. Use a  <see cref="TokenType"/> .</param>
        public Token(double numberValue, TokenType type)
        {
            this.tokenValue = numberValue;
            this.tokenType = type;
            this.isOperator = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// This constructor is used when creating a operator token.
        /// </summary>
        /// <param name="type">The type of the operator. Use a <see cref="TokenType"/>. </param>
        public Token(TokenType type)
        {
            this.tokenType = type;
            this.isOperator = true;

            if (tokenType == TokenType.Plus) this.operationPriority = 1;
            else if (tokenType == TokenType.Minus) this.operationPriority = 1;
            else if (tokenType == TokenType.Multiply) this.operationPriority = 2;
            else if (tokenType == TokenType.Divide) this.operationPriority = 2;
            else if (tokenType == TokenType.Exponent) this.operationPriority = 3;
            else if (tokenType == TokenType.SquareRoot) this.operationPriority = 3;
            else if (tokenType == TokenType.NaturalLogarithm) this.operationPriority = 4;
            else if (tokenType == TokenType.AbsoluteValue) this.operationPriority = 4;
            else if (tokenType == TokenType.LeftParenthesis) this.operationPriority = 0;
            else if (tokenType == TokenType.RightParenthesis) this.operationPriority = 0;
            else if (tokenType == TokenType.UnaryMinus) this.operationPriority = 5;
        }

        /// <summary>
        /// Performs the calculation that the current token does.
        /// This method should be used with operation which involve two operands.
        /// (Add, Subtract, Divide, Multiply)
        /// </summary>
        /// <param name="operand1">First operand</param>
        /// <param name="operand2">Second operand.</param>
        /// <returns>The result of the operation.</returns>
        public double Calculate(double operand1, double operand2)
        {
            if (tokenType == TokenType.Plus) return operand1 + operand2;
            else if (tokenType == TokenType.Minus) return operand1 - operand2;
            else if (tokenType == TokenType.Multiply) return operand1 * operand2;
            else if (tokenType == TokenType.Divide)
            {
                if (operand2 == 0) throw new System.DivideByZeroException("Error: Attempted to divide by zero.");
                else return operand1 / operand2;
            }
            else return 0;
        }

        /// <summary>
        /// Performs the calculation that the current token does.
        /// This method should be used with operation which involve one operand.
        /// (Square Root, Natural Logarithm, Absolute Value, Unary MInus, Exponent)
        /// </summary>
        /// <param name="operand">The operand.</param>
        /// <returns>The result of the operation.</returns>
        public double Calculate(double operand)
        {
            if (tokenType == TokenType.SquareRoot) return Math.Sqrt(operand);
            else if (tokenType == TokenType.NaturalLogarithm) return Math.Log(operand,Math.E);
            else if (tokenType == TokenType.AbsoluteValue) return Math.Abs(operand);
            else if (tokenType == TokenType.UnaryMinus) return operand*-1;
            else if (tokenType == TokenType.Exponent) return Math.Exp(operand);
            else return 0;
        }
    }
}
