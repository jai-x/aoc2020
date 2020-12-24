using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    public class Day23 : Day
    {
        private int[] startingCups;

        public Day23(string input) : base(input)
        {
            startingCups = input.Trim()
                                .Select(i => Int32.Parse(i.ToString()))
                                .ToArray();
        }

        private class Cup
        {
            public int Value;
            public Cup Next;

            public Cup PickupThreeAfter()
            {
                Cup pick = Next;
                Next = pick.Next.Next.Next;
                pick.Next.Next.Next = null;
                return pick;
            }

            public void InsertThreeAfter(Cup ins)
            {
                Cup after = Next;
                ins.Next.Next.Next = after;
                Next = ins;
            }
        }

        private void log(object msg) => Console.WriteLine(msg);

        private Cup schmove(IEnumerable<int> intialCupValues, int moves)
        {
            // generate cup objects
            var allCups = intialCupValues.Select(v => new Cup { Value = v }).ToList();

            // link all cups circularly
            for (var i = 0; i < allCups.Count; i++)
                allCups[i].Next = allCups[(i + 1) % allCups.Count];

            // index of cup value to node
            var cupIndex = allCups.ToDictionary(c => c.Value, c => c);

            var current = allCups.First();

            for (int i = 0; i < moves; i++)
            {
                var pickup = current.PickupThreeAfter();

                var destination = current.Value;

                do
                {
                    destination--;

                    if (destination == 0)
                        destination = allCups.Count;

                } while(pickup.Value == destination || pickup.Next.Value == destination || pickup.Next.Next.Value == destination);

                cupIndex[destination].InsertThreeAfter(pickup);

                current = current.Next;
            }

            return cupIndex[1];
        }

        public override string Part1()
        {
            var cupOne = schmove(startingCups, 100);

            var ret = String.Empty;

            foreach (var _ in Enumerable.Range(0, 8))
            {
                cupOne = cupOne.Next;
                ret += cupOne.Value.ToString();
            }

            return ret;
        }

        public override string Part2()
        {
            var millionCups = startingCups.Concat(Enumerable.Range(startingCups.Max() + 1, 1000000 - startingCups.Count()));

            var cupOne = schmove(millionCups, 10000000);

            return (((long) cupOne.Next.Value) * ((long) cupOne.Next.Next.Value)).ToString();
        }
    }
}
