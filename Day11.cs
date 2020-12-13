using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    public class Day11 : Day
    {
        private class Layout : ICloneable
        {
            private char[][] map;

            public int YMax { get; private set; }
            public int XMax { get; private set; }
            public bool Dirty { get; private set; }

            public Layout(char[][] map)
            {
                this.map = map;
                YMax = map.Length;
                XMax = map.First().Length;
                Dirty = false;
            }

            // scuffed deep clone >_<
            public object Clone() => new Layout(this.map.Select(a => a.ToArray()).ToArray());

            public char Get(int x, int y)
            {
                if (y < 0 || y >= YMax)
                    return 'L';

                if (x < 0 || x >= XMax)
                    return 'L';

                return map[y][x];
            }

            public void Set(int x, int y, char newTile)
            {
                var oldTile = map[y][x];
                if (newTile != oldTile)
                    Dirty = true;

                map[y][x] = newTile;
            }

            public int TotalOccupied() => map.Select(row => row.Where(tile => tile == '#').Count()).Sum();

            private char rayTrace(ValueTuple<int, int> start, ValueTuple<int, int> direction, int limit)
            {
                var position = (x: start.Item1, y: start.Item2);
                var offset = (x: direction.Item1, y: direction.Item2);
                var tile = Get(position.x, position.y);

                for (int i = 0; i < limit; i++)
                {
                    position = (x: position.x + offset.x, y: position.y + offset.y);
                    tile = Get(position.x, position.y);

                    if (tile == '#' || tile =='L')
                        return tile;

                }

                return tile;
            }

            public int AdjacentOccupied(int x, int y, int limit) =>
                new [] { (-1, -1), (0, -1), (1, -1),
                         (-1,  0),          (1,  0),
                         (-1,  1), (0,  1), (1,  1), }
                .Select(direction => rayTrace((x, y), direction, limit))
                .Where(tile => tile == '#')
                .Count();

            public void Print()
            {
                foreach (var row in map)
                    Console.WriteLine(new String(row));
            }
        }

        private Layout layout;

        public Day11(string input) : base(input)
        {
            var map = input.Trim()
                           .Split('\n')
                           .Select(line => line.Trim().ToCharArray())
                           .ToArray();

            layout = new Layout(map);
        }

        private Layout simulate(Layout oldLayout, int maxSeek, int maxAdjacent)
        {
            var newLayout = oldLayout.Clone() as Layout;

            for (int y = 0; y < oldLayout.YMax; y++)
            {
                for (int x = 0; x < oldLayout.XMax; x++)
                {
                    var oldTile = oldLayout.Get(x, y);

                    if (oldTile == '.')
                        continue;

                    var newTile = oldTile;
                    var numAdjacentOccupied = oldLayout.AdjacentOccupied(x, y, maxSeek);

                    if (oldTile == '#')
                        if (numAdjacentOccupied >= maxAdjacent)
                            newTile = 'L';

                    if (oldTile == 'L')
                        if (numAdjacentOccupied == 0)
                            newTile = '#';

                    newLayout.Set(x, y, newTile);
                }
            }

            return newLayout;
        }

        public override string Part1()
        {
            var workingLayout = layout.Clone() as Layout;

            while (true)
            {
                workingLayout = simulate(workingLayout, 1, 4);

                if (!workingLayout.Dirty)
                    break;
            }

            return workingLayout.TotalOccupied().ToString();
        }

        public override string Part2()
        {
            var workingLayout = layout.Clone() as Layout;

            while (true)
            {
                workingLayout = simulate(workingLayout, Int32.MaxValue, 5);

                if (!workingLayout.Dirty)
                    break;
            }

            return workingLayout.TotalOccupied().ToString();
        }
    }
}
