using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    public class Day19 : Day
    {
        private string[] messages;

        private Dictionary<int, Rule> rules = new Dictionary<int, Rule>();

        public Day19(string input) : base(input)
        {
            var inputLines = input.Trim()
                                  .Split("\n\n")
                                  .Select(b => b.Split('\n').Select(l => l.Trim()).ToArray())
                                  .ToArray();

            messages = inputLines.Last();

            var ruleLines = inputLines.First();

            foreach (var ruleStr in ruleLines)
            {
                var parts = ruleStr.Split(':');
                var (number, body) = (Int32.Parse(parts.First()), parts.Last().Trim());

                if ((body.Length == 3) && body[0] == '"')
                    rules.Add(number, new LiteralRule
                    {
                        Self = number,
                        Literal = body.Replace('"', ' ')
                                      .Trim()
                                      .First(),
                    });
                else
                    rules.Add(number, new CompositeRule
                    {
                        Self = number,
                        Subrules = body.Split('|')
                                       .Select(s => s.Trim())
                                       .Select(s => s.Split(' ').Select(Int32.Parse).ToList())
                                       .ToList(),
                    });
            }
        }

        private abstract class Rule
        {
            public int Self;
        }

        private class LiteralRule : Rule
        {
            public char Literal;

            public override string ToString() => $"{Self}: \"{Literal}\"";
        }

        private class CompositeRule : Rule
        {
            public List<List<int>> Subrules;

            public override string ToString() => $"{Self}: {String.Join(" | ", Subrules.Select(g => String.Join(" ", g)))}";
        }

        private bool match(IEnumerable<char> input, IEnumerable<int> ruleSequence)
        {
            if (!input.Any() || !ruleSequence.Any()) // either empty
                return !input.Any() && !ruleSequence.Any(); // both empty

            var r = rules[ruleSequence.First()];

            if (r is LiteralRule)
            {
                var rule = r as LiteralRule;

                if (input.First() == rule.Literal)
                    return match(input.Skip(1), ruleSequence.Skip(1)); // match first letter
                else
                    return false; // end of sequence
            }

            if (r is CompositeRule)
            {
                var rule = r as CompositeRule;

                return rule.Subrules.Select(sub => match(input, sub.Concat(ruleSequence.Skip(1)))).Any(m => m); // chain
            }

            return false;
        }

        public override string Part1() => messages.Where(m => match(m, new[] { 0 })).Count().ToString();

        public override string Part2()
        {
            rules[8] = new CompositeRule
            {
                Self = 8,
                Subrules = new List<List<int>>
                {
                    new List<int> { 42 },
                    new List<int> { 42, 8 },
                },
            };

            rules[11] = new CompositeRule
            {
                Self = 11,
                Subrules = new List<List<int>>
                {
                    new List<int> { 42, 31 },
                    new List<int> { 42, 11, 31 },
                },
            };

            return messages.Where(m => match(m, new[] { 0 })).Count().ToString();
        }
    }
}
