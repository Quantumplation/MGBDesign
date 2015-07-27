using System;

namespace DesignValueParser.Interfaces.Accessors
{
    public class GateValueSerialization
    {
        // Constants
        public decimal BaseRange { get; set; }
        public decimal BaseLinkTime { get; set; }
        public decimal BaseSpeed { get; set; }
        public decimal BaseLoadingTime { get; set; }

        // Formulas (declared as strings, since this is their type in JSON)
        public string Range { get; set; }
        public string LinkTime { get; set; }
        public string Speed { get; set; }
        public string LoadingTime { get; set; }
    }

    public class WarpGateAccessor
        : DesignValuesAccessor<IWarpGate, GateValueSerialization>
    {
        public WarpGateAccessor(GateValueSerialization deserialized)
            : base(deserialized)
        {
            _linkTime = Evaluate(deserialized.LinkTime);
            _range = Evaluate(deserialized.Range);
            _speed = Evaluate(deserialized.Speed);
            _loadingTime = Evaluate(deserialized.LoadingTime);
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

        private readonly Func<IWarpGate, decimal> _speed;
        public decimal Speed(IWarpGate gate)
        {
            return _speed(gate);
        }

        private readonly Func<IWarpGate, decimal> _loadingTime;
        public decimal LoadingTime(IWarpGate gate)
        {
            return _loadingTime(gate);
        }
    }
}
