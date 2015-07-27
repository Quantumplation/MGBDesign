
namespace DesignValueParser.Interfaces.Accessors
{
    public class CarrierSerialization
    {
    }

    public class CarrierAccessor
        : DesignValuesAccessor<ICarrier, CarrierSerialization>
    {
        public CarrierAccessor(CarrierSerialization deserialized)
            : base(deserialized)
        {
        }
    }
}
