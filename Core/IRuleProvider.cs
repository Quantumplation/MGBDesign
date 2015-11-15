using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IRuleProvider
    {
        TConstants Get<TConstants, TContext>(TContext contextA)
            where TConstants : BaseConstants<TContext, TConstants>
            where TContext : BaseContext<TConstants, TContext>;
    }
}
