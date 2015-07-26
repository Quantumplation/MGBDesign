using DesignValueParser.Expression;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesignValueParser.Test.Expression
{
    [TestClass]
    public class ExpressionParserTest
    {
        [TestMethod]
        public void MethodName()
        {
            ExpressionParser p = new ExpressionParser();
            p.ParseExpression("-a + b * c ^ d");
        }
    }
}
