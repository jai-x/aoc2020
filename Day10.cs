using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    public class Day10 : Day
    {
        private List<int> adapterRatings;

        public Day10(string input) : base(input)
        {
            var tmp = input.Trim()
                           .Split('\n')
                           .Select(Int32.Parse)
                           .OrderBy(x => x);

            adapterRatings = tmp.Prepend(0)
                                .Append(tmp.Max() + 3)
                                .ToList();
        }

        public override string Part1()
        {
            var joltDiffs = adapterRatings.Zip(adapterRatings.Skip(1), (prev, curr) => curr - prev);

            var joltDiffCounts = new Dictionary<int, int>();

            foreach (var diff in joltDiffs)
            {
                var count = joltDiffCounts.ContainsKey(diff) ? joltDiffCounts[diff] : 0;
                joltDiffCounts[diff] = count + 1;
            }

            return (joltDiffCounts[1] * joltDiffCounts[3]).ToString();
        }

        public override string Part2()
        {
            var pathCount = new Dictionary<int, long>();

            pathCount.Add(0, 1);

            foreach (var jolt in adapterRatings.Skip(1))
            {
                long paths = 0;

                foreach (var lookback in Enumerable.Range(jolt - 3, 3))
                {
                    if (!adapterRatings.Contains(lookback))
                        continue;

                    if (pathCount.TryGetValue(lookback, out var p))
                        paths += p;
                }

                pathCount.Add(jolt, paths);
            }

            return pathCount[adapterRatings.Max()].ToString();
        }
    }
}
