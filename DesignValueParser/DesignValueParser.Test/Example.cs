using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace DesignValueParser.Test
{
    /// <summary>
    /// Your "context" for these operations is a gate object, this provides properties with certain information about this individual gate
    /// </summary>
    public class Gate
    {
        /// <summary>
        /// The distance this warp gate is linking
        /// </summary>
        public decimal Distance
        {
            get
            {
                return 10;
            }
        }
    }

    /// <summary>
    /// This is the deserialization object, it should contain exactly the same fields as the json file
    /// </summary>
    public class GateValues
        : Result    // <-- deserialization types must inherit from Result
    {
        // Constants
        public decimal BaseRange { get; set; }
        public decimal BaseLinkTime { get; set; }
        public decimal BasePayloadTime { get; set; }

        // Formulas (declared as strings, since this is their type in JSON)
        public string Range { get; set; }
        public string LinkTime { get; set; }
        public string PayloadTime { get; set; }
    }

    [TestClass]
    public class Example
    {
                    private const string JSON = @"{
    /* ""Context"" Refers to the warp gate in question */

    /* Constant values */
    ""BaseRange"": 1,
    ""BaseLinkTime"": 1,
    ""BasePayloadTime"": 1,
    
    //Formulaic values
    ""Range"": ""BaseRange"",
    ""LinkTime"": ""BaseLinkTime + (BaseLinkTime * (Self.Distance / Range)) ^ 2"",
    ""PayloadTime"": ""BasePayloadTime""
}";

        [TestMethod]
        public void Example1()
        {
            //Read the JSON config straight off github
            var gateConstants = new Parser().Parse<GateValues>(new StringReader(JSON));

            //This is a gate instance somewhere in the universe
            Gate myGate = new Gate();

            //Evaluate a value from the file
            decimal range = gateConstants.Evaluate<Gate>("Range")(myGate);
                                                        // ^- Name of the parameter you want

            Console.WriteLine(range);
        }
    }

    /// <summary>
    /// This is an accessor, I suggest you wrap all your deserialization types like this. This hides away the ugly details of deserialization and caches the expensive delegate creation
    /// </summary>
    public class GateValuesAccessor
    {
        public GateValuesAccessor(Result values)
        {
            _range = values.Evaluate<Gate>("Range");
            _linkTime = values.Evaluate<Gate>("LinkTime");
            _payloadTime = values.Evaluate<Gate>("PayloadTime");
        }

        private readonly Func<Gate, decimal> _range; 
        public decimal Range(Gate gate)
        {
            return _range(gate);
        }

        private readonly Func<Gate, decimal> _linkTime;
        public decimal LinkTime(Gate gate)
        {
            return _linkTime(gate);
        }

        private readonly Func<Gate, decimal> _payloadTime;
        public decimal PayloadTime(Gate gate)
        {
            return _payloadTime(gate);
        }
    }
}
