using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    public class Day6 : Day
    {
        private List<List<string>> groups;

        public Day6(string input) : base(input)
        {
            groups = input
                .Trim()
                .Split("\n\n")
                .Select(grp => grp.Trim().Split("\n").ToList())
                .ToList();
        }

        public override string Part1() => groups
            .Select(grp => grp.SelectMany(g => g).Distinct().Count())
            .Sum()
            .ToString();

        public override string Part2() => groups
            .Select(grp => grp.Aggregate((agg, ans) => String.Concat(agg.Intersect(ans))).Count())
            .Sum()
            .ToString();
    }
}
