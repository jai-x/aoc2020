using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace aoc2020
{
    public class Day2 : Day
    {
        private class PolicyChecker
        {
            private int min;
            private int max;
            private char letter;
            private string password;

            public PolicyChecker(int min, int max, char letter, string password)
            {
                this.min = min;
                this.max = max;
                this.letter = letter;
                this.password = password;
            }

            public bool ValidCount()
            {
                var count = password.Count(c => c == letter);

                return (count >= min) && (count <= max);
            }

            public bool ValidPositions()
            {
                var first = password.ElementAt(min - 1) == letter;
                var second = password.ElementAt(max - 1) == letter;

                return first ^ second;
            }

            public override string ToString()
            {
                return $"PolicyChecker ({min}-{max} {letter}: {password})";
            }
        }

        private List<PolicyChecker> policies = new List<PolicyChecker>();

        public Day2(string input) : base(input)
        {
            using (var sr = new StringReader(input))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    var parts = line.Split(" ");

                    var minmaxParts = parts[0].Split("-");

                    var min = Int32.Parse(minmaxParts[0]);
                    var max = Int32.Parse(minmaxParts[1]);

                    var letter = parts[1][0];

                    var password = parts[2];

                    policies.Add(new PolicyChecker(min, max, letter, password));
                }
            }
        }

        public override string Part1() => policies.Count(p => p.ValidCount()).ToString();

        public override string Part2() => policies.Count(p => p.ValidPositions()).ToString();
    }
}
