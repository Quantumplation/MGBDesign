using System;

namespace DesignValueParser.Interfaces.Accessors
{
    public class GateValueSerialization
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

    public class WarpGateAccessor
        : DesignValuesAccessor<IWarpGate, GateValueSerialization>
    {
        public WarpGateAccessor(GateValueSerialization deserialized)
            : base(deserialized)
        {
            _linkTime = Evaluate(deserialized.LinkTime);
            _range = Evaluate(deserialized.Range);
            _payloadTime = Evaluate(deserialized.PayloadTime);
        }

        private readonly Func<IWarpGate, decimal> _range;
        public decimal Range(IWarpGate gate)
        {
            return _range(gate);
        }

        private readonly Func<IWarpGate, decimal> _linkTime;
        public decimal LinkTime(IWarpGate gate)
        {
            return _linkTime(gate);
        }

        private readonly Func<IWarpGate, decimal> _payloadTime;
        public decimal PayloadTime(IWarpGate gate)
        {
            return _payloadTime(gate);
        }
    }
}
