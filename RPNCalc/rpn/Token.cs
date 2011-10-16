namespace RPNCalc.rpn
{
    public enum TokenType
    {
        None,
        Number,
        Constant,
        Plus,
        Minus,
        Multiply,
        Divide,
        Exponent,
        UnaryMinus,
        Sine,
        Cosine,
        Tangent,
        SquareRoot,
        NaturalLogarithm,
        AbsoluteValue
    }

    class Token
    {
        private double tokenValue;
        private TokenType tokenType;

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

        public Token(double numberValue, TokenType type)
        {
            this.tokenValue = numberValue;
            this.tokenType = type;
        }

        public Token(TokenType type)
        {
            this.tokenType = type;
        }
    }
}
