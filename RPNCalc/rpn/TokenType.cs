namespace RPNCalc.rpn
{
    /// <summary>
    /// The different types of tokens which can be found in an arithmetic expression.
    /// </summary>
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
        SquareRoot,
        NaturalLogarithm,
        AbsoluteValue,
        LeftParenthesis,
        RightParenthesis
    }
}