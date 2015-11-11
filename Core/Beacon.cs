using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    ///  A class which serves as a beacon to find the assembly to include when interpreting.
    ///  i.e. we tell roslyn to include typeof(Beacon).Assembly so that our scripts can reference
    ///  these common constants and interfaces etc.
    /// </summary>
    public class Beacon
    {
    }
}
