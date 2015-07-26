
namespace DesignValueParser.Expression
{
    abstract class Operand
    {
        public string Token { get; private set; }

        protected Operand(string token)
        {
            Token = token;
        }
    }

    internal class Value
        : Operand
    {
        public Value(string token)
            : base(token)
        {
        }
    }

    internal class LeftParenthesis : Operand
    {
        public LeftParenthesis()
            : base("")
        {
        }
    }

    internal class RightParenthesis : Operand
    {
        public RightParenthesis()
            : base("")
        {
        }
    }

    internal class BinaryOperator : Operand
    {
        public OperatorNotation Notation { get; private set; }

        public Operand LeftOperand { get; set; }
        public Operand RightOperand { get; set; }

        public BinaryOperator(string token, OperatorNotation notation)
            : base(token)
        {
            Notation = notation;
        }
    }

    internal class UnaryOperator
        : Operand
    {
        public Operand Operand { get; set; }

        public UnaryOperator(string token)
            : base(token)
        {
        }
    }

    internal class Negative : UnaryOperator
    {
        public Negative()
            : base("")
        {
        }
    }
}
