using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    class Day12 : Day
    {
        private class Ship
        {
            private int x;
            private int y;
            private int direction;

            private Dictionary<int, char> dirToCardinal = new Dictionary<int, char>
            {
                {   0, 'N' },
                {  90, 'E' },
                { 180, 'S' },
                { 270, 'W' },
            };

            public Ship()
            {
                x = 0;
                y = 0;
                direction = 90;
            }

            private int normalize(int dir) => (((dir % 360) + 360) % 360);

            public void ApplyCommand(ValueTuple<char, int> command)
            {
                var (action, val) = command;

                switch (action)
                {
                    case 'N':
                        y += val;
                        break;
                    case 'S':
                        y -= val;
                        break;
                    case 'E':
                        x += val;
                        break;
                    case 'W':
                        x -= val;
                        break;
                    case 'L':
                        direction -= val;
                        break;
                    case 'R':
                        direction += val;
                        break;
                    case 'F':
                        ApplyCommand((dirToCardinal[normalize(direction)], val));
                        break;
                }
            }

            public int Manhatten() => Math.Abs(x) + Math.Abs(y);
        }

        public class ShipWithWaypoint
        {
            private int x;
            private int y;
            private int waypointX;
            private int waypointY;

            public ShipWithWaypoint()
            {
                x = 0;
                y = 0;
                waypointX = 10;
                waypointY = 1;
            }

            public void ApplyCommand(ValueTuple<char, int> command)
            {
                var (action, val) = command;

                switch (action)
                {
                    case 'N':
                        waypointY += val;
                        break;
                    case 'S':
                        waypointY -= val;
                        break;
                    case 'E':
                        waypointX += val;
                        break;
                    case 'W':
                        waypointX -= val;
                        break;
                    case 'L':
                        foreach (var _ in Enumerable.Range(0, val/90))
                            (waypointX, waypointY) = (-waypointY, waypointX);
                        break;
                    case 'R':
                        foreach (var _ in Enumerable.Range(0, val/90))
                            (waypointX, waypointY) = (waypointY, -waypointX);
                        break;
                    case 'F':
                        x += waypointX * val;
                        y += waypointY * val;
                        break;
                }
            }

            public int Manhatten() => Math.Abs(x) + Math.Abs(y);
        }

        private List<(char, int)> commands;

        public Day12(string input) : base(input)
        {
            commands = input.Trim()
                            .Split('\n')
                            .Select(line => (line[0], Int32.Parse(line[1..])))
                            .ToList();
        }

        public override string Part1()
        {
            var ship = new Ship();

            foreach (var command in commands)
                ship.ApplyCommand(command);

            return ship.Manhatten().ToString();
        }

        public override string Part2()
        {
            var ship = new ShipWithWaypoint();

            foreach (var command in commands)
                ship.ApplyCommand(command);

            return ship.Manhatten().ToString();
        }
    }
}
