using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    class Day18 : Day
    {
        private string[] expressions;

        public Day18(string input) : base(input)
        {
            expressions = input.Trim()
                               .Split('\n')
                               .Select(line => line.Trim())
                               .Select(line => line.Replace(" ", ""))
                               .ToArray();
        }

        private long math(string expr)
        {
            if (expr.Length == 1)
                return Convert.ToInt64(expr);

            var bDepth = 0;
            for (var i = expr.Length - 1; i > 0; i--)
            {
                if (expr[i] == ')')
                {
                    bDepth++;
                    continue;
                }

                if (expr[i] == '(')
                {
                    bDepth--;
                    continue;
                }

                if ((expr[i] == '+' || expr[i] == '*') && bDepth == 0)
                {
                    var op = expr[i];
                    var left = math(expr[0..i]);
                    var right = math(expr[(i+1)..]);

                    if (op == '*')
                        return left * right;

                    if (op == '+')
                        return left + right;
                }
            }

            // fully bracketed expression
            return math(expr[1..^1]);
        }

        private long advancedMath(string expr)
        {
            if (expr.Length == 1)
                return Convert.ToInt64(expr);

            var bDepth = 0;
            for (var i = expr.Length - 1; i > 0; i--)
            {
                if (expr[i] == ')')
                {
                    bDepth++;
                    continue;
                }

                if (expr[i] == '(')
                {
                    bDepth--;
                    continue;
                }

                if (expr[i] == '*' && bDepth == 0)
                {
                    var left = advancedMath(expr[0..i]);
                    var right = advancedMath(expr[(i+1)..]);
                    return left * right;
                }
            }

            bDepth = 0;
            for (var i = expr.Length - 1; i > 0; i--)
            {
                if (expr[i] == ')')
                {
                    bDepth++;
                    continue;
                }

                if (expr[i] == '(')
                {
                    bDepth--;
                    continue;
                }

                if (expr[i] == '+' && bDepth == 0)
                {
                    var left = advancedMath(expr[0..i]);
                    var right = advancedMath(expr[(i+1)..]);
                    return left + right;
                }
            }

            // fully bracketed expression
            return advancedMath(expr[1..^1]);
        }

        public override string Part1() => expressions.Select(math).Sum().ToString();

        public override string Part2() => expressions.Select(advancedMath).Sum().ToString();
    }
}
