using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IContext
    {
        void SetConstant(object constants);
    }
    public class BaseContext<T, TSelf> : IContext
        where T : BaseConstants<TSelf, T>
        where TSelf : BaseContext<T, TSelf>
    {
        public T Constants { get; set; }

        public void SetConstant(object constants)
        {
            Constants = (T) constants;
        }
    }

    public class TravelContext 
        : BaseContext<TravelConstants, TravelContext>
    {
        public decimal Distance { get; set; }
    }
}
