using DecimalMath;
using System;
using System.Linq.Expressions;

namespace DesignValueParser.Expressions
{
    internal static class Conversion
    {
        public static LambdaExpression Convert<TContext, TDeserialized>(this Operand root, Type selfType)
        {
            //Context object in universe
            var contextParam = Expression.Parameter(typeof(TContext), "context");

            //Deserialized object (raw values from JSON)
            var deserializedParam = Expression.Parameter(typeof(TDeserialized), "deserialized");

            //Self object (methods for evaluating expressions)
            var selfParam = Expression.Parameter(selfType, "self");

            return Expression.Lambda(
                Convert(root, contextParam, deserializedParam, selfParam),
                new ParameterExpression[] {
                    contextParam,
                    deserializedParam,
                    selfParam
                }
            );
        }

        private static Expression Convert(Operand root, ParameterExpression contextParam, ParameterExpression deserializedParam, ParameterExpression selfParam)
        {
            var binOp = root as BinaryOperator;
            if (binOp != null)
            {
                switch (binOp.Token)
                {
                    case "*":
                        return Expression.Multiply(Convert(binOp.LeftOperand, contextParam, deserializedParam, selfParam), Convert(binOp.RightOperand, contextParam, deserializedParam, selfParam));
                    case "+":
                        return Expression.Add(Convert(binOp.LeftOperand, contextParam, deserializedParam, selfParam), Convert(binOp.RightOperand, contextParam, deserializedParam, selfParam));
                    case "/":
                        return Expression.Divide(Convert(binOp.LeftOperand, contextParam, deserializedParam, selfParam), Convert(binOp.RightOperand, contextParam, deserializedParam, selfParam));
                    case "-":
                        return Expression.Subtract(Convert(binOp.LeftOperand, contextParam, deserializedParam, selfParam), Convert(binOp.RightOperand, contextParam, deserializedParam, selfParam));
                    case "^":
                        return Expression.Call(typeof(DecimalEx), "Pow",
                            new Type[0],
                            Convert(binOp.LeftOperand, contextParam, deserializedParam, selfParam),
                            Convert(binOp.RightOperand, contextParam, deserializedParam, selfParam)
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
                        return Expression.Negate(Convert(unOp.Operand, contextParam, deserializedParam, selfParam));
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
                    Expression obj = deserializedParam;

                    //Follow down chain of accessors
                    int start = 0;
                    if (split[0] == "Context")
                    {
                        obj = contextParam;
                        start++;
                    }

                    for (int i = start; i < split.Length; i++)
                        obj = Expression.PropertyOrField(obj, split[i]);

                    return obj;
                }

                return Expression.PropertyOrField(deserializedParam, valOp.Token);
            }

            throw new NotImplementedException();
        }
    }
}
