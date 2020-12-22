using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    public class Day20 : Day
    {
        private class Tile
        {
            public int ID;
            public char[][] Data;

            public override string ToString() => String.Join('\n', Data.Select(row => new String(row)).Prepend($"Tile: {ID}"));

            public char[][] TrimmedData => Data.Skip(1).SkipLast(1).Select(row => row.Skip(1).SkipLast(1).ToArray()).ToArray();

            public string TrimmedToString() => String.Join('\n', TrimmedData.Select(row => new String(row)).Prepend($"Tile: {ID}"));

            public int Width => Data.First().Length;
            public int Height => Data.Length;

            public int TrimmedWidth => TrimmedData.First().Length;
            public int TrimmedHeight => TrimmedData.Length;

            public string North => new String(Data.First());
            public string East => Data.Aggregate("", (agg, row) => agg + row.Last());
            public string South => new String(Data.Last());
            public string West => Data.Aggregate("", (agg, row) => agg + row.First());

            public List<string> Borders => new List<string>
            {
                North,
                new String(North.ToCharArray().Reverse().ToArray()),
                East,
                new String(East.ToCharArray().Reverse().ToArray()),
                South,
                new String(South.ToCharArray().Reverse().ToArray()),
                West,
                new String(West.ToCharArray().Reverse().ToArray()),
            };



            private int angle = 0;
            private bool flipped = false;

            public void Permutate()
            {
                var n = Data.Length;

                var newData = new char[n][];
                for (int i = 0; i < n; i++)
                    newData[i] = new char[n];

                for (var i = 0; i < n; i++)
                    for (var j = 0; j < n; j++)
                        newData[i][j] = Data[n - j - 1][i];

                Data = newData;
                angle += 90;

                if (angle == 360)
                {
                    angle = 0;

                    for (var i = 0; i < n; i++)
                        Data[i] = Data[i].Reverse().ToArray();

                    flipped = !flipped;
                }
            }
        }

        private Dictionary<int, Tile> tiles;

        private Dictionary<string, List<int>> borderMap = new Dictionary<string, List<int>>();

        private bool uniqueBorder(string border) => borderMap.ContainsKey(border) && borderMap[border].Count == 1;

        private List<int> cornerIds => borderMap.Where(e => e.Value.Count == 1) // unique tile borders must be edge of image
                                                .Select(e => e.Value.First())   // get id
                                                .GroupBy(id => id)              // group by id
                                                .Where(grp => grp.Count() == 4) // corner tiles have 2 unique edges + 2 flipped
                                                .Select(grp => grp.Key)         // get the filered ids
                                                .Distinct()                     // remove duplicates
                                                .ToList();

        public Day20(string input) : base(input)
        {
            tiles = input.Trim()
                         .Split("\n\n")
                         .Select(t => t.Trim())
                         .Select(t => t.Split('\n').Select(l => l.Trim()))
                         .Select(t => new Tile {
                             ID = Int32.Parse(t.First().Replace("Tile", "").Replace(":", "").Trim()),
                             Data = t.Skip(1).Select(l => l.ToCharArray()).ToArray(),
                         })
                         .ToDictionary(t => t.ID, t => t);

            foreach (var (id, tile) in tiles)
                foreach (var border in tile.Borders)
                    if (borderMap.ContainsKey(border))
                        borderMap[border].Add(id);
                    else
                        borderMap.Add(border, new List<int> { id });
        }

        public override string Part1() => cornerIds.Aggregate(1L, (product, id) => product * (long)id).ToString();

        private Tile fullPicture()
        {
            var topLeftId = cornerIds.First();

            var rows = new List<List<Tile>>();
            var currentRow = new List<Tile>();
            var added = new HashSet<int>();

            var topLeft = tiles[topLeftId];

            // fit top left
            while (!(uniqueBorder(topLeft.North) && uniqueBorder(topLeft.West)))
                topLeft.Permutate();

            currentRow.Add(topLeft);
            added.Add(topLeft.ID);

            // fit first row
            while (!uniqueBorder(currentRow.Last().East))
            {
                var next = tiles[borderMap[currentRow.Last().East].Where(id => !added.Contains(id)).First()];

                while (currentRow.Last().East != next.West)
                    next.Permutate();

                currentRow.Add(next);
                added.Add(next.ID);
            }

            rows.Add(currentRow);

            // fit subsequent rows
            while (tiles.Count != added.Count)
            {
                currentRow = new List<Tile>();

                var prevLeftMost = rows.Last().First();
                var newLeftMost = tiles[borderMap[prevLeftMost.South].Where(id => !added.Contains(id)).First()];

                while (prevLeftMost.South != newLeftMost.North)
                    newLeftMost.Permutate();

                currentRow.Add(newLeftMost);
                added.Add(newLeftMost.ID);

                while (!uniqueBorder(currentRow.Last().East))
                {
                    var next = tiles[borderMap[currentRow.Last().East].Where(id => !added.Contains(id)).First()];

                    while (currentRow.Last().East != next.West)
                        next.Permutate();

                    currentRow.Add(next);
                    added.Add(next.ID);
                }

                rows.Add(currentRow);
            }

            var picWidth = rows.First().Select(t => t.TrimmedWidth).Sum();
            var picHeight = rows.Select(row => row.First().TrimmedHeight).Sum();

            if (picWidth != picHeight)
                throw new Exception();

            var n = picHeight;
            var picData = new char[n][];

            for (var i = 0; i < n; i++)
                picData[i] = new char[n];

            for (var y = 0; y < n; y++)
            {
                var rowY = y / (n / rows.Count);
                var trimmedY = y % rows.First().First().TrimmedHeight;

                for (var x = 0; x < n; x++)
                {
                    var rowX = x / (n / rows.First().Count);
                    var trimmedX = x % rows.First().First().TrimmedWidth;

                    picData[y][x] = rows[rowY][rowX].TrimmedData[trimmedY][trimmedX];
                }
            }

            return new Tile { ID = -1, Data = picData };
        }

        private char[][] seaMonster => new char[][]
        {
            "                  # ".ToCharArray(),
            "#    ##    ##    ###".ToCharArray(),
            " #  #  #  #  #  #   ".ToCharArray(),
        };

        private int seaMonsterWidth => seaMonster.First().Length;
        private int seaMonsterHeight => seaMonster.Length;
        private int seaMonsterCount => seaMonster.Select(r => r.Count(ch => ch == '#')).Sum();

        public override string Part2()
        {
            var picture = fullPicture();

            var seaMonstersFound = 0;
            while (seaMonstersFound < 1)
            {
                for (var y = 0; y + seaMonsterHeight < picture.Height; y++)
                {
                    for (var x = 0; x + seaMonsterWidth < picture.Width; x++)
                    {
                        var found = true;
                        for (var ys = 0; ys < seaMonsterHeight; ys++)
                        {
                            for (var xs = 0; xs < seaMonsterWidth; xs++)
                            {
                                if (seaMonster[ys][xs] == ' ')
                                    continue;

                                if (seaMonster[ys][xs] != picture.Data[y+ys][x+xs])
                                    found = false;
                            }
                        }
                        if (found)
                            seaMonstersFound++;
                    }
                }
                picture.Permutate();
            }

            return (picture.Data.Select(row => row.Count(t => t == '#')).Sum() - (seaMonsterCount * seaMonstersFound)).ToString();
        }
    }
}
