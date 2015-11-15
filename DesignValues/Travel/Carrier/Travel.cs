namespace DesignValues.Travel.Carrier
{
    public class Travel : Core.Travel.Carrier.Travel.Constants
    {
        public Travel(Core.Travel.Carrier.Travel.Context context)
            : base(context)
        {
        }

        public decimal BaseSpeed => 1;

        public override decimal Speed => BaseSpeed;
    }
}
