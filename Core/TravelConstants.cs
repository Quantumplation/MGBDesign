using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public abstract class BaseConstants<T, TSelf>
        where T : BaseContext<TSelf, T>
        where TSelf : BaseConstants<T, TSelf>
    {
        protected T _context;

        protected BaseConstants(T context)
        {
            _context = context;
        }
    }

    public abstract class TravelConstants 
        : BaseConstants<TravelContext, TravelConstants>
    {
        protected TravelConstants(TravelContext context) : base(context) { }

        public abstract decimal BaseRange { get; }
        public abstract decimal BaseLinkTime { get; }
        public abstract decimal BasePayloadTime { get; }
        public abstract decimal BaseLoadingTime { get; }

        public abstract decimal Range { get; }
        public abstract decimal LinkTime { get; }
        public abstract decimal PayloadTime { get; }
    }
}
