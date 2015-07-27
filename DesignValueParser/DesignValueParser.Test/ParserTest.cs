using System;
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
    ""D"":""A * B * Context.F.E""
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

            var acs = new TestAccessor(r);

            Assert.AreEqual(6, acs.C(null));
        }

        [TestMethod]
        public void AssertThat_EvaluatingFormula_WithContext_ProducesCorrectValue()
        {
            Parser p = new Parser();
            var r = p.Parse<TestResultType>(new StringReader(JSON));

            var acs = new TestAccessor(r);

            Assert.AreEqual(30, acs.D(new ContextType
            {
                E = 4,
                F = new ContextType {
                    E = 5
                }
            }));
        }
    }

    public class TestAccessor
        : DesignValuesAccessor<ContextType, TestResultType>
    {
        public TestAccessor(TestResultType values)
            : base(values)
        {
            _c = Evaluate(values.C);
            _d = Evaluate(values.D);
        }

        private readonly Func<ContextType, decimal> _c;
        public decimal C(ContextType ctx)
        {
            return _c(ctx);
        }

        private readonly Func<ContextType, decimal> _d;
        public decimal D(ContextType ctx)
        {
            return _d(ctx);
        }
    }

    public class TestResultType
    {
        public decimal A { get; set; }

        public decimal B { get; set; }

        public string C { get; set; }

        public string D { get; set; }
    }

    public class ContextType
    {
        public decimal E { get; set; }

        public ContextType F { get; set; }
    }
}
