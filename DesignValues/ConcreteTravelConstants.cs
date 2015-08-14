using System;
using Core;

namespace DesignValues
{
    public class ConcreteTravelConstants : TravelConstants
    {
        public ConcreteTravelConstants(TravelContext context) : base(context) { }

        public override decimal BaseRange => 1;
        public override decimal BaseLinkTime => 1;
        public override decimal BasePayloadTime => 1;
        public override decimal BaseLoadingTime => 1;

        private decimal DistanceWeightedLinkTime => BaseLinkTime * (_context.Distance / Range); 

        public override decimal Range => BaseRange;
        public override decimal LinkTime { get { Console.WriteLine("HAHA"); return BaseLinkTime + DistanceWeightedLinkTime*DistanceWeightedLinkTime; } }
        public override decimal PayloadTime => BaseLoadingTime;
    }
}
