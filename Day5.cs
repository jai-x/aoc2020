using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    public class Day5 : Day
    {
        private HashSet<int> seatIds;

        public Day5(string input) : base(input)
        {
            seatIds = input
                .Trim()
                .Split('\n')
                .Select(line => line.Replace('F', '0').Replace('B', '1').Replace('L', '0').Replace('R', '1'))
                .Select(binLine => Convert.ToInt32(binLine, 2))
                .ToHashSet();
        }

        public override string Part1() => seatIds.Max().ToString();
        public override string Part2() => Enumerable.Range(seatIds.Min(), seatIds.Count + 1).ToHashSet().Except(seatIds).First().ToString();
    }
}
