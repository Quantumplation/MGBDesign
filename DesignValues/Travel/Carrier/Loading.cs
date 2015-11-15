namespace DesignValues.Travel.Carrier
{
    public class Loading : Core.Travel.Carrier.Loading.Constants
    {
        public Loading(Core.Travel.Carrier.Loading.Context context)
            : base(context)
        {
        }

        private ulong BaseLoadTime => 30;

        public override decimal DepartureDelay => Context.Travellers * BaseLoadTime;
    }
}
