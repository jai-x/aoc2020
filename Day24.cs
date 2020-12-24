using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    public class Day24 : Day
    {
        private List<List<string>> directions = new List<List<string>>();

        public Day24(string input) : base(input)
        {
            var lines = input.Trim().Split('\n').Select(line => line.Trim()).ToArray();

            foreach (var line in lines)
            {
                var dirs = new List<string>();

                for (int i = 0; i < line.Length; i++)
                {
                    switch (line[i])
                    {
                        case 'e':
                        case 'w':
                            dirs.Add(line[i].ToString());
                            break;

                        case 'n':
                        case 's':
                            dirs.Add(line[i..(i+2)]);
                            i++;
                            break;
                    }

                }

                directions.Add(dirs);
            }
        }

        private HashSet<(int x, int y)> blackTiles = new HashSet<(int x, int y)>();

        public override string Part1()
        {
            foreach (var line in directions)
            {
                var pos = (x: 0, y: 0);

                foreach (var dir in line)
                {
                    switch (dir)
                    {
                        case "e":
                            pos.x++;
                            break;

                        case "w":
                            pos.x--;
                            break;

                        case "ne":
                            pos.y++;
                            break;

                        case "sw":
                            pos.y--;
                            break;

                        case "se":
                            pos.x++;
                            pos.y--;
                            break;

                        case "nw":
                            pos.x--;
                            pos.y++;
                            break;
                    }
                }

                if (blackTiles.Contains(pos))
                    blackTiles.Remove(pos);
                else
                    blackTiles.Add(pos);
            }

            return blackTiles.Count().ToString();
        }

        private void log(object msg) => Console.WriteLine(msg);

        public override string Part2()
        {
            var offsets = new(int x, int y)[] { (0, 1), (1, 0), (1, -1), (0, -1), (-1, 0), (-1, 1) };

            for (int i = 0; i < 100; i++)
            {
                var whiteTiles = new HashSet<(int x, int y)>();

                var newBlackTiles = new HashSet<(int x, int y)>();

                foreach (var bTile in blackTiles)
                {
                    var adjacents = offsets.Select(o => (x: bTile.x + o.x, y: bTile.y + o.y)).ToList();

                    var numAdjacentBlack = 0;

                    foreach (var t in adjacents)
                    {
                        if (blackTiles.Contains(t))
                            numAdjacentBlack++;
                        else
                            whiteTiles.Add(t);
                    }

                    if (numAdjacentBlack > 0 && numAdjacentBlack <= 2)
                        newBlackTiles.Add(bTile);
                }

                foreach (var wTile in whiteTiles)
                {
                    var numAdjacentBlack = offsets.Select(o => (x: wTile.x + o.x, y: wTile.y + o.y))
                                                  .Count(t => blackTiles.Contains(t));

                    if (numAdjacentBlack == 2)
                        newBlackTiles.Add(wTile);
                }

                blackTiles = newBlackTiles;

                log($"Day {1 + i}: {blackTiles.Count()}");
            }

            return blackTiles.Count().ToString();
        }
    }
}
