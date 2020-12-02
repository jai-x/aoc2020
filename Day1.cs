using System;
using System.IO;
using System.Collections.Generic;

namespace aoc2020
{
    public class Day1 : Day
    {
        private List<int> entries = new List<int>();

        public Day1(string input) : base(input)
        {
            using (var sr = new StringReader(input))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                    entries.Add(Int32.Parse(line));
            }

            entries.Sort();
        }

        public override int Part1()
        {
            for (int i = 0; i < entries.Count; i++)
            {
                for (int j = 0; j < entries.Count; j++)
                {
                    if (i == j)
                        continue;

                    var first = entries[i];
                    var second = entries[j];

                    if ((first + second) == 2020)
                        return first * second;
                }
            }

            throw new InvalidOperationException();
        }

        public override int Part2()
        {
            for (int i = 0; i < entries.Count; i++)
            {
                for (int j = 0; j < entries.Count; j++)
                {
                    for (int k = 0; k < entries.Count; k++)
                    {
                        if ((i == j) || (j == k) || (k == i))
                            continue;

                        var first = entries[i];
                        var second = entries[j];
                        var third = entries[k];

                        if ((first + second + third) == 2020)
                            return first * second * third;
                    }
                }
            }

            throw new InvalidOperationException();
        }
    }
}
