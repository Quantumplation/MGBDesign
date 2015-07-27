﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace DesignValueParser.Test
{
    public class Star
    {
        public decimal Mass
        {
            get
            {
                return 100000;
            }
        }
    }

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

        public Star Star
        {
            get
            {
                return new Star();
            }
        }
    }

    /// <summary>
    /// This is the deserialization object, it should contain exactly the same fields as the json file
    /// </summary>
    public class GateValues
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
    ""BaseRange"": 20,
    ""BaseLinkTime"": 10,
    ""BasePayloadTime"": 1,
    
    //Formulaic values
    ""Range"": ""BaseRange"",
    ""LinkTime"": ""BaseLinkTime + (BaseLinkTime * (Context.Distance / $Range)) ^ 0.5"",
    ""PayloadTime"": ""BasePayloadTime""
}";

        [TestMethod]
        public void Example1()
        {
            //Read the JSON config straight off github
            var gateConstants = new Parser().Parse<GateValues>(new StringReader(JSON));
            var accessor = new GateValuesAccessor(gateConstants);

            //This is a gate instance somewhere in the universe
            Gate myGate = new Gate();

            //Evaluate values from the file
            Console.WriteLine(accessor.Range(myGate));
            Console.WriteLine(accessor.LinkTime(myGate));
            Console.WriteLine(accessor.PayloadTime(myGate));
        }
    }

    /// <summary>
    /// This is an accessor, I suggest you wrap all your deserialization types like this. This hides away the ugly details of deserialization and caches the expensive delegate creation
    /// </summary>
    public class GateValuesAccessor
        : DesignValuesAccessor<Gate, GateValues>
    {
        public GateValuesAccessor(GateValues values)
            : base(values)
        {
            _linkTime = Evaluate(values.LinkTime);
            _range = Evaluate(values.Range);
            _payloadTime = Evaluate(values.PayloadTime);                        
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
