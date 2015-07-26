using DesignValueParser.Expression;
using System;
using System.Reflection;
using Exp = System.Linq.Expressions.Expression;

namespace DesignValueParser
{
    public class Result
    {
        public Func<TContext, decimal> Evaluate<TContext>(string name)
        {
            var exp = (string)GetType()
                .GetProperty(name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                .GetValue(this);

            ExpressionParser p = new ExpressionParser();
            p.ParseExpression(exp);

            var e = Exp.Lambda<Func<int>>(
                System.Linq.Expressions.Expression.Multiply(
                    System.Linq.Expressions.Expression.Constant(2),
                    System.Linq.Expressions.Expression.Constant(3)
                )
            );

            var r = e.Compile();

            var re = r();

            throw new NotImplementedException();
        }
    }
}
