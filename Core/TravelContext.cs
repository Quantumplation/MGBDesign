using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public abstract class BaseContext<T, TSelf>
        where T : BaseConstants<TSelf, T>
        where TSelf: BaseContext<T, TSelf>
    {
        public T Get(IRuleProvider rules)
        {
            return rules.Get<T, TSelf>((TSelf)this);
        }
    }
    public class TravelContext : BaseContext<TravelConstants, TravelContext>
    {
        public decimal Distance { get; set; }
    }
}
