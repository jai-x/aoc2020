using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    public class Day7 : Day
    {
        private Dictionary<string, List<(int, string)>> rules = new Dictionary<string, List<(int, string)>>();

        public Day7(string input) : base(input)
        {
            foreach (var line in input.Trim().Split('\n'))
            {
                // cursed parsing >_<
                var tokens = line
                    .Replace(".", "")
                    .Replace(" bags", "")
                    .Replace(" bag", "")
                    .Replace("no other", "")
                    .Replace(" contain", ",")
                    .Split(",")
                    .Select(token => token.Trim())
                    .Where(token => !String.IsNullOrEmpty(token))
                    .ToArray();

                if (tokens.Length == 1)
                    continue;

                var parent = tokens.First();
                var children = tokens.Skip(1).Select(child => (Int32.Parse(child[0].ToString()), child[1..].Trim())).ToList();

                rules[parent] = children;
            }
        }

        public override string Part1()
        {
            var bags = new List<string> { "shiny gold" };

            for (int i = 0; i < bags.Count; i++)
            {
                var bag = bags[i];

                foreach (var rule in rules)
                    if (rule.Value.Select(child => child.Item2).Contains(bag))
                        bags.Add(rule.Key);
            }

            return bags.Skip(1).Distinct().Count().ToString();
        }

        public override string Part2()
        {
            var bags = new List<string> { "shiny gold" };

            for (int i = 0; i < bags.Count; i++)
            {
                var bag = bags[i];

                if (rules.TryGetValue(bag, out var children))
                    foreach (var child in children)
                        bags.AddRange(Enumerable.Repeat(child.Item2, child.Item1).ToList());
            }

            return bags.Skip(1).Count().ToString();
        }
    }
}
