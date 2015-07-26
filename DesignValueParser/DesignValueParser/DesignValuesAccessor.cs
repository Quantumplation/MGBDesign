using DesignValueParser.Expressions;
using System;

namespace DesignValueParser
{
    public class DesignValuesAccessor<TContext, TSerialization, TSelf>
    {
        private readonly TSerialization _values;

        protected DesignValuesAccessor(TSerialization values)
        {
            _values = values;
        }

        protected Func<TContext, decimal> Evaluate(string expression)
        {
            //Parse the expression
            ExpressionParser p = new ExpressionParser();
            p.ParseExpression(expression);

            var f = p.ExpressionTree.Convert<TContext, TSerialization>().Compile();

            return (context) => (decimal)f.DynamicInvoke(context, _values);
        }
    }
}
