using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    public class Day9 : Day
    {
        private static int preamble = 25;
        private List<Int64> cipher;

        public Day9(string input) : base(input)
        {
            cipher = input.Trim().Split('\n').Select(Int64.Parse).ToList();
        }

        private bool containsTwoSum(List<Int64> values, Int64 target)
        {
            for (int i = 0; i < values.Count; i++)
            {
                for (int j = 0; j < values.Count; j++)
                {
                    if (i == j)
                        continue;

                    if ((values[i] + values[j]) == target)
                        return true;
                }
            }

            return false;
        }

        public override string Part1()
        {
            for (int i = preamble; i < cipher.Count; i++)
            {
                var values = cipher.Skip(i - preamble).Take(preamble).ToList();
                var target = cipher[i];

                if (!containsTwoSum(values, target))
                    return target.ToString();
            }

            return "Error";
        }

        public override string Part2()
        {
            var i = 0;
            var target = Int64.Parse(Part1());
            var candidates = new Queue<Int64>();

            while (i < cipher.Count)
            {
                if (candidates.Sum() == target)
                    return (candidates.Max() + candidates.Min()).ToString();

                if (candidates.Sum() < target)
                    candidates.Enqueue(cipher[i++]);

                if (candidates.Sum() > target)
                    candidates.Dequeue();
            }

            return "Error";
        }
    }
}
