using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DesignValueParser.Expressions
{
    internal enum TokenClass
    {
        Value,
        Operator,
        LeftParen,
        RightParen
    }

    internal enum Associativity
    {
        Right,
        Left
    }

    internal class ExpressionParser
    {
        //http://stackoverflow.com/a/2853938/108234

        public Operand ExpressionTree { get; private set; }

        private readonly Stack<Operand> _stack = new Stack<Operand>();
        private readonly Queue<Operand> _outputQueue = new Queue<Operand>();

        public void ParseExpression(string expression)
        {
            //Dijkstra's Shunting Yard Algorithm
            Regex re = new Regex(@"([\+\-\*\(\)\^\/\ ])");
            List<String> tokenList = re.Split(expression).Select(t => t.Trim()).Where(t => t != "").ToList();

            for (int tokenNumber = 0; tokenNumber < tokenList.Count(); ++tokenNumber)
            {
                String token = tokenList[tokenNumber];
                TokenClass tokenClass = GetTokenClass(token);

                switch (tokenClass)
                {
                    case TokenClass.Value:
                        _outputQueue.Enqueue(new Value(token));
                        break;

                    case TokenClass.Operator:
                        if (token == "-" && (_stack.Count == 0 || tokenList[tokenNumber - 1] == "("))
                        {
                            _stack.Push(new Negative());
                            break;
                        }
                        if (_stack.Count > 0)
                        {
                            String stackTopToken = _stack.Peek().Token;
                            if (GetTokenClass(stackTopToken) == TokenClass.Operator)
                            {
                                Associativity tokenAssociativity = GetOperatorAssociativity(token);
                                int tokenPrecedence = GetOperatorPrecedence(token);
                                int stackTopPrecedence = GetOperatorPrecedence(stackTopToken);

                                if (tokenAssociativity == Associativity.Left && tokenPrecedence <= stackTopPrecedence ||
                                    tokenAssociativity == Associativity.Right && tokenPrecedence < stackTopPrecedence)
                                {
                                    _outputQueue.Enqueue(_stack.Pop());
                                }
                            }
                        }
                        _stack.Push(new BinaryOperator(token));
                        break;

                    case TokenClass.LeftParen:
                        _stack.Push(new LeftParenthesis());
                        break;

                    case TokenClass.RightParen:
                        while (!(_stack.Peek() is LeftParenthesis))
                        {
                            _outputQueue.Enqueue(_stack.Pop());
                        }
                        _stack.Pop();

                        break;
                }

                if (tokenClass == TokenClass.Value || tokenClass == TokenClass.RightParen)
                {
                    if (tokenNumber < tokenList.Count() - 1)
                    {
                        String nextToken = tokenList[tokenNumber + 1];
                        TokenClass nextTokenClass = GetTokenClass(nextToken);
                        if (nextTokenClass != TokenClass.Operator && nextTokenClass != TokenClass.RightParen)
                        {
                            tokenList.Insert(tokenNumber + 1, "*");
                        }
                    }
                }
            }

            while (_stack.Count > 0)
            {
                Operand operand = _stack.Pop();
                if (operand is LeftParenthesis || operand is RightParenthesis)
                    throw new ArgumentException("Mismatched parentheses");

                _outputQueue.Enqueue(operand);
            }

            Stack<Operand> expressionStack = new Stack<Operand>();
            while (_outputQueue.Count > 0)
            {
                Operand operand = _outputQueue.Dequeue();

                if (operand is Value)
                {
                    expressionStack.Push(operand);
                }
                else
                {
                    if (operand is BinaryOperator)
                    {
                        BinaryOperator op = (BinaryOperator) operand;
                        Operand rightOperand = expressionStack.Pop();
                        Operand leftOperand = expressionStack.Pop();
                        op.LeftOperand = leftOperand;
                        op.RightOperand = rightOperand;
                    }
                    else if (operand is UnaryOperator)
                    {
                        ((UnaryOperator) operand).Operand = expressionStack.Pop();
                    }

                    expressionStack.Push(operand);
                }
            }

            if (expressionStack.Count != 1)
            {
                throw new ArgumentException("Invalid formula");
            }

            ExpressionTree = expressionStack.Pop();
        }

        private static readonly string[] _operators = {
            "+", "-", "*", "/", "^"
        };

        private static TokenClass GetTokenClass(String token)
        {
            switch (token)
            {
                case "(":
                    return TokenClass.LeftParen;

                case ")":
                    return TokenClass.RightParen;

                default:
                    if (_operators.Contains(token))
                        return TokenClass.Operator;
                    else
                        return TokenClass.Value;
            }
        }

        private static Associativity GetOperatorAssociativity(String token)
        {
            if (token == "^")
                return Associativity.Right;
            else
                return Associativity.Left;
        }

        private static int GetOperatorPrecedence(String token)
        {
            switch (token)
            {
                case "-":
                case "+":
                    return 1;

                case "/":
                case "*":
                    return 2;

                case "^":
                    return 3;

                default:
                    throw new ArgumentException("Invalid token");
            }
        }
    }
}
