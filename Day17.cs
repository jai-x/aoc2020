using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    class Day17 : Day
    {
        private readonly HashSet<(int x, int y)> initialState = new HashSet<(int x, int y)>();
        private readonly HashSet<(int x, int y, int z)> offsets3d = new HashSet<(int x, int y, int z)>();
        private readonly HashSet<(int x, int y, int z, int w)> offsets4d = new HashSet<(int x, int y, int z, int w)>();

        public Day17(string input) : base(input)
        {
            var lines = input.Trim().Split('\n').Select(line => line.Trim()).ToArray();

            for(var y = 0; y < lines.Length; y++)
                for (var x = 0; x < lines[y].Length; x++)
                    if (lines[y][x] == '#')
                        initialState.Add((x: x, y: y));

            foreach (var x in Enumerable.Range(-1, 3))
                foreach (var y in Enumerable.Range(-1, 3))
                    foreach (var z in Enumerable.Range(-1, 3))
                        if (!((x == 0) && (y == 0) && (z == 0)))
                            offsets3d.Add((x: x, y: y, z: z));

            foreach (var x in Enumerable.Range(-1, 3))
                foreach (var y in Enumerable.Range(-1, 3))
                    foreach (var z in Enumerable.Range(-1, 3))
                        foreach (var w in Enumerable.Range(-1, 3))
                            if (!((x == 0) && (y == 0) && (z == 0) && (w == 0)))
                                offsets4d.Add((x: x, y: y, z: z, w: w));

        }

        private HashSet<(int x, int y, int z)> iterate3d(HashSet<(int x, int y, int z)> oldActive)
        {
            var newActive = new HashSet<(int x, int y, int z)>();
            var oldInactive = new HashSet<(int x, int y, int z)>();

            // consider already active
            foreach (var c in oldActive)
            {
                var numActiveNeighbours = 0;

                foreach (var o in offsets3d)
                {
                    var postion = (x: c.x + o.x, y: c.y + o.y, z: c.z + o.z);

                    if (oldActive.Contains(postion))
                        numActiveNeighbours++;
                    else
                        oldInactive.Add(postion);
                }

                if (numActiveNeighbours == 2 || numActiveNeighbours == 3)
                    newActive.Add(c);
            }

            // consider already inactive
            foreach (var c in oldInactive)
            {
                var numActiveNeighbours = 0;

                foreach (var o in offsets3d)
                {
                    var postion = (x: c.x + o.x, y: c.y + o.y, z: c.z + o.z);

                    if (oldActive.Contains(postion))
                        numActiveNeighbours++;
                }

                if (numActiveNeighbours == 3)
                    newActive.Add(c);
            }

            return newActive;
        }

        private HashSet<(int x, int y, int z, int w)> iterate4d(HashSet<(int x, int y, int z, int w)> oldActive)
        {
            var newActive = new HashSet<(int x, int y, int z, int w)>();
            var oldInactive = new HashSet<(int x, int y, int z, int w)>();

            // consider already active
            foreach (var c in oldActive)
            {
                var numActiveNeighbours = 0;

                foreach (var o in offsets4d)
                {
                    var postion = (x: c.x + o.x, y: c.y + o.y, z: c.z + o.z, w: c.w + o.w);

                    if (oldActive.Contains(postion))
                        numActiveNeighbours++;
                    else
                        oldInactive.Add(postion);
                }

                if (numActiveNeighbours == 2 || numActiveNeighbours == 3)
                    newActive.Add(c);
            }

            // consider already inactive
            foreach (var c in oldInactive)
            {
                var numActiveNeighbours = 0;

                foreach (var o in offsets4d)
                {
                    var postion = (x: c.x + o.x, y: c.y + o.y, z: c.z + o.z, w: c.w + o.w);

                    if (oldActive.Contains(postion))
                        numActiveNeighbours++;
                }

                if (numActiveNeighbours == 3)
                    newActive.Add(c);
            }

            return newActive;
        }

        public override string Part1()
        {
            // clone into three dimensions
            var activeCubes = new HashSet<(int x, int y, int z)>(initialState.Select(c => (x: c.x, y: c.y, z: 0)));

            for (var i = 0; i < 6; i++)
                activeCubes = iterate3d(activeCubes);

            return activeCubes.Count.ToString();
        }

        public override string Part2()
        {
            // clone into four dimensions
            var activeCubes = new HashSet<(int x, int y, int z, int w)>(initialState.Select(c => (x: c.x, y: c.y, z: 0, w: 0)));

            for (var i = 0; i < 6; i++)
                activeCubes = iterate4d(activeCubes);

            return activeCubes.Count.ToString();
        }
    }
}
