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
            var interpreter =
                new Interpreter.Interpreter("quantumplation", "MGBDesign", "DesignValues", "50cf7a7ebcf0093e25bf27a2fe3d010e6e45b5bc");

            var context = new TravelContext {Distance = 10};
            interpreter.Populate(context);
            var c = context.Constants;
            Console.WriteLine($"Constants:\n" +
                              $" - {nameof(c.BaseLinkTime)}: {c.BaseLinkTime}\n" +
                              $" - {nameof(c.BaseLoadingTime)}: {c.BaseLoadingTime}\n" +
                              $" - {nameof(c.BasePayloadTime)}: {c.BaseRange}\n" +
                              $" - {nameof(c.LinkTime)}: {c.LinkTime}\n" +
                              $" - {nameof(c.PayloadTime)}: {c.PayloadTime}\n" +
                              $" - {nameof(c.Range)}: {c.Range}\n" +
                              $"");
        }
    }
}
