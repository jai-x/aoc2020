using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    class Day13 : Day
    {
        private int targetTime;
        private List<(long, long)> timetable;

        public Day13(string input) : base(input)
        {
            var lines = input.Trim().Split('\n');

            targetTime = Int32.Parse(lines[0]);

            timetable = lines[1].Trim()
                                .Split(',')
                                .Select(id => id.Trim())
                                .Select((id, idx) => (id == "x") ? (idx, -1) : (idx, Int64.Parse(id)))
                                .Where(entry => entry.Item2 != -1)
                                .Select(entry => (Convert.ToInt64(entry.Item1), entry.Item2))
                                .ToList();
        }

        public override string Part1()
        {
            var bestId = 0L;
            var bestTime = Int64.MaxValue;

            foreach (var (_, id) in timetable)
            {
                var time = 0L;

                while (time < targetTime)
                    time += id;

                if (time < bestTime)
                {
                    bestId = id;
                    bestTime = time;
                }
            }

            return ((bestTime - targetTime) * bestId).ToString();
        }

        public override string Part2()
        {
            timetable.Reverse();
            var times = new Stack<(long, long)>(timetable);

            while (times.Count > 1)
            {
                var (firstOffset, firstId) = times.Pop();
                var (secondOffset, secondId) = times.Pop();

                var time = firstOffset;

                while (true)
                {
                    if (((time + secondOffset) % secondId) == 0)
                        break;

                    time += firstId;
                }

                var newId = firstId * secondId;
                var newOffset = time;

                times.Push((newOffset, newId));
            }

            return times.Pop().Item1.ToString();
        }
    }
}
