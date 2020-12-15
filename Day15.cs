using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    public class Day15 : Day
    {
        private List<int> startingNumbers;

        public Day15(string input) : base(input)
        {
            startingNumbers = input.Trim()
                                   .Split(',')
                                   .Select(num => num.Trim())
                                   .Select(Int32.Parse)
                                   .ToList();
        }

        private int lastSpoken(List<int> starting, int limit)
        {
            var spoken = new Dictionary<int, int>();

            var turn = 0;
            var lastSpoken = 0;

            foreach (var currentSpoken in starting)
            {
                spoken[currentSpoken] = turn++;
                lastSpoken = currentSpoken;
            }

            while (turn < limit)
            {
                var currentSpoken = 0;

                if (spoken.TryGetValue(lastSpoken, out var lastSpokenTurn))
                    currentSpoken = (turn - 1) - lastSpokenTurn;

                spoken[lastSpoken] = (turn++ - 1);

                lastSpoken = currentSpoken;
            }

            return lastSpoken;
        }

        public override string Part1() => lastSpoken(startingNumbers, 2020).ToString();

        public override string Part2() => lastSpoken(startingNumbers, 30000000).ToString();
    }
}
