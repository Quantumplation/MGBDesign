using System;
using System.Linq.Expressions;

namespace DesignValueParser.Expressions
{
    internal static class Conversion
    {
        public static LambdaExpression Convert<TContext>(this Operand root, Type selfType)
        {
            var contextParam = Expression.Parameter(typeof(TContext), "context");
            var selfParam = Expression.Parameter(selfType, "self");

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
                        return Expression.Power(Convert(binOp.LeftOperand, contextParam, selfType), Convert(binOp.RightOperand, contextParam, selfType));
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
                if (valOp.Token.Contains("."))
                {
                    var split = valOp.Token.Split('.');

                    if (split[0] != "Context")
                        throw new InvalidOperationException("Cannot access non-context object");

                    //Follow down chain of accessors
                    Expression obj = contextParam;
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
