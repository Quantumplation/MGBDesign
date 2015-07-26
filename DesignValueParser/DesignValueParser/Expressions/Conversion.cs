using DecimalMath;
using System;
using System.Linq.Expressions;

namespace DesignValueParser.Expressions
{
    internal static class Conversion
    {
        public static LambdaExpression Convert<TContext, TThis>(this Operand root)
        {
            var contextParam = Expression.Parameter(typeof(TContext), "context");
            var selfParam = Expression.Parameter(typeof(TThis), "self");

            return Expression.Lambda(
                Convert(root, contextParam, selfParam),
                new ParameterExpression[] {
                    contextParam,
                    selfParam
                }
            );
        }

        private static Expression Convert(Operand root, ParameterExpression contextParam, ParameterExpression selfType)
        {
            var binOp = root as BinaryOperator;
            if (binOp != null)
            {
                switch (binOp.Token)
                {
                    case "*":
                        return Expression.Multiply(Convert(binOp.LeftOperand, contextParam, selfType), Convert(binOp.RightOperand, contextParam, selfType));
                    case "+":
                        return Expression.Add(Convert(binOp.LeftOperand, contextParam, selfType), Convert(binOp.RightOperand, contextParam, selfType));
                    case "/":
                        return Expression.Divide(Convert(binOp.LeftOperand, contextParam, selfType), Convert(binOp.RightOperand, contextParam, selfType));
                    case "-":
                        return Expression.Subtract(Convert(binOp.LeftOperand, contextParam, selfType), Convert(binOp.RightOperand, contextParam, selfType));
                    case "^":
                        return Expression.Call(typeof(DecimalEx), "Pow",
                            new Type[0],
                            Convert(binOp.LeftOperand, contextParam, selfType),
                            Convert(binOp.RightOperand, contextParam, selfType)
                        );
                    default:
                        throw new InvalidOperationException("Unknown Binary Operator \"" + binOp.Token + "\"");
                }
            }

            var unOp = root as UnaryOperator;
            if (unOp != null)
            {
                switch (unOp.Token)
                {
                    case "-":
                        return Expression.Negate(Convert(unOp.Operand, contextParam, selfType));
                    default:
                        throw new InvalidOperationException("Unknown Unary Operator \"" + unOp.Token + "\"");
                }
            }

            var valOp = root as Value;
            if (valOp != null)
            {
                //A value could be a number, if it is we'll run with that
                decimal d;
                if (decimal.TryParse(valOp.Token, out d))
                    return Expression.Constant(d);

                //If it's got a path then we should follow it
                if (valOp.Token.Contains("."))
                {
                    var split = valOp.Token.Split('.');

                    //If context isn't specified, we assume we're accessing self
                    Expression obj = split[0] == "Context" ? contextParam : selfType;

                    //Follow down chain of accessors
                    for (int i = 1; i < split.Length; i++)
                        obj = Expression.PropertyOrField(obj, split[i]);

                    return obj;
                }

                return Expression.PropertyOrField(selfType, valOp.Token);
            }

            throw new NotImplementedException();
        }
    }
}
