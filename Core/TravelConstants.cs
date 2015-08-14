using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public abstract class TravelConstants
    {
        protected TravelContext _context;
        public TravelConstants(TravelContext context)
        {
            _context = context;
        }

        public abstract decimal BaseRange { get; }
        public abstract decimal BaseLinkTime { get; }
        public abstract decimal BasePayloadTime { get; }
        public abstract decimal BaseLoadingTime { get; }

        public abstract decimal Range { get; }
        public abstract decimal LinkTime { get; }
        public abstract decimal PayloadTime { get; }
    }
}
