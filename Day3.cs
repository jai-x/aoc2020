using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace aoc2020
{
    public class Day3 : Day
    {
        private class MountainMap
        {
            private List<string> coords = new List<string>();

            public MountainMap(string mapInput)
            {
                using (var sr = new StringReader(mapInput))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                        coords.Add(line);
                }
            }

            public int TreesPerSlope(int across, int down)
            {
                int x = 0;
                int y = 0;

                int trees = 0;

                while (y < coords.Count)
                {
                    if (hasTree(x, y))
                        trees++;

                    y += down;
                    x += across;
                }

                return trees;
            }

            private bool hasTree(int x, int y)
            {
                var row = coords[y];
                var square = row[x % row.Length];
                return square == '#';
            }
        }

        private MountainMap map;

        public Day3(string input) : base(input)
        {
            map = new MountainMap(input);
        }

        public override string Part1() => map.TreesPerSlope(3, 1).ToString();

        public override string Part2() => new[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) }
                .Select(slope => map.TreesPerSlope(slope.Item1, slope.Item2))
                .Select(trees => new BigInteger(trees))
                .Aggregate(new BigInteger(1), (product, val) => product * val)
                .ToString();
    }
}
