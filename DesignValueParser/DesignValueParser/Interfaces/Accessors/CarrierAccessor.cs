
using System;

namespace DesignValueParser.Interfaces.Accessors
{
    public class CarrierSerialization
    {
        // Constants
        public decimal BaseRange { get; set; }
        public decimal BaseSpinUpTime { get; set; }
        public decimal BaseSpeed { get; set; }

        // Formulas (declared as strings, since this is their type in JSON)
        public string Range { get; set; }
        public string SpinUpTime { get; set; }
        public string Speed { get; set; }
    }

    public class CarrierAccessor
        : DesignValuesAccessor<ICarrier, CarrierSerialization>
    {
        public CarrierAccessor(CarrierSerialization deserialized)
            : base(deserialized)
        {
            _range = Evaluate(deserialized.Range);
            _spinUpTime = Evaluate(deserialized.SpinUpTime);
            _speed = Evaluate(deserialized.Speed);
        }

        private readonly Func<ICarrier, decimal> _range;
        public decimal Range(ICarrier gate)
        {
            return _range(gate);
        }

        private readonly Func<ICarrier, decimal> _spinUpTime;
        public decimal SpinUpTime(ICarrier gate)
        {
            return _spinUpTime(gate);
        }

        private readonly Func<ICarrier, decimal> _speed;
        public decimal Speed(ICarrier gate)
        {
            return _speed(gate);
        }
    }
}
