using System;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var interpreter =
                new Interpreter.Interpreter("quantumplation", "MGBDesign", "DesignValues", "50cf7a7ebcf0093e25bf27a2fe3d010e6e45b5bc");

            //var context = new WarpGateContext { Distance = 10 };
            //var c = context.Get(interpreter);
            //Console.WriteLine(string.Join("\n",
            //    $"Constants:",
            //    $"- {nameof(c.BaseLinkTime)}: {c.BaseLinkTime}",
            //    $"- {nameof(c.BaseLoadingTime)}: {c.BaseLoadingTime}",
            //    $"- {nameof(c.BasePayloadTime)}: {c.BaseRange}",
            //    $"- {nameof(c.LinkTime)}: {c.LinkTime}",
            //    $"- {nameof(c.PayloadTime)}: {c.PayloadTime}",
            //    $"- {nameof(c.Range)}: {c.Range}"
            //));

            Console.ReadLine();
        }
    }
}
