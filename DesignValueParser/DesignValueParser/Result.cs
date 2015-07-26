using DesignValueParser.Expressions;
using System;
using System.Reflection;

namespace DesignValueParser
{
    public class Result
    {
        public Func<TContext, decimal> Evaluate<TContext>(string name) where TContext : class
        {
            var exp = (string)GetType()
                .GetProperty(name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                .GetValue(this);

            //Parse the expression
            ExpressionParser p = new ExpressionParser();
            p.ParseExpression(exp);

            var f = p.ExpressionTree.Convert<TContext>(GetType()).Compile();

            return (context) => (decimal)f.DynamicInvoke(context, this);
        }
    }
}
