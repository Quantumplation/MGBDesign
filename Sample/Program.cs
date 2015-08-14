using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var interpreter = new Interpreter.Interpreter();
            var context = new TravelContext {Distance = 100};
            var last = 0m;
            while (true)
            {
                var constants = interpreter.Get<TravelConstants>("D:\\proj\\MGBDesign\\DesignValues\\ConcreteTravelConstants.cs", context);
                if (last != constants.LinkTime)
                {
                    Console.Clear();
                    Console.WriteLine($"LinkTime: {constants.LinkTime}");
                    last = constants.LinkTime;
                }
            }
        }
    }
}
