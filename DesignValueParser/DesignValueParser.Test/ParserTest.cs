using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace DesignValueParser.Test
{
    [TestClass]
    public class ParserTest
    {
        private const string JSON = @"{
    ""A"": 2,
    ""B"": 3,
    ""C"": ""A * B"",
    ""D"":""A * B * Context.B.A""
}";

        [TestMethod]
        public void AssertThat_ParsingFileWithConstants_ExtractsConstants()
        {
            Parser p = new Parser();
            var r = p.Parse<TestResultType>(new StringReader(JSON));

            Assert.AreEqual(2, r.A);
            Assert.AreEqual(3, r.B);
            Assert.AreEqual("A * B", r.C);
        }

        [TestMethod]
        public void AssertThat_EvaluatingFormula_ProducesCorrectValue()
        {
            Parser p = new Parser();
            var r = p.Parse<TestResultType>(new StringReader(JSON));

            Assert.AreEqual(6, r.Evaluate<object>("C")(null));
        }

        [TestMethod]
        public void AssertThat_EvaluatingFormula_WithContext_ProducesCorrectValue()
        {
            Parser p = new Parser();
            var r = p.Parse<TestResultType>(new StringReader(JSON));

            Assert.AreEqual(30, r.Evaluate<ContextType>("D")(new ContextType
            {
                A = 4,
                B = new ContextType {
                    A = 5
                }
            }));
        }
    }

    public class TestResultType
        : Result
    {
        public decimal A { get; set; }

        public decimal B { get; set; }

        public string C { get; set; }

        public string D { get; set; }
    }

    public class ContextType
        : Result
    {
        public decimal A { get; set; }

        public ContextType B { get; set; }
    }
}
