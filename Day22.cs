using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    public class Day22 : Day
    {
        private Queue<int> player1;
        private Queue<int> player2;

        public Day22(string input) : base(input)
        {
            var decks = input.Trim()
                             .Split("\n\n")
                             .Select(parts => parts.Trim())
                             .Select(parts => parts.Split('\n'))
                             .Select(parts => parts.Skip(1))
                             .Select(parts => parts.Select(Int32.Parse).ToArray())
                             .ToArray();

            (player1, player2) = (new Queue<int>(decks[0]), new Queue<int>(decks[1]));
        }

        public override string Part1()
        {
            var p1 = new Queue<int>(player1);
            var p2 = new Queue<int>(player2);

            while (p1.Count > 0 && p2.Count > 0)
            {
                var (p1Card, p2Card) = (p1.Dequeue(), p2.Dequeue());

                if (p1Card > p2Card)
                {
                    p1.Enqueue(p1Card);
                    p1.Enqueue(p2Card);
                }
                else
                {
                    p2.Enqueue(p2Card);
                    p2.Enqueue(p1Card);
                }
            }

            var winner = p1.Count > p2.Count ? p1 : p2;

            return winner.Reverse().Select((val, idx) => val * (idx + 1)).Sum().ToString();
        }

        private (int, Queue<int>) recursiveCombat(Queue<int> p1, Queue<int> p2)
        {
            var playStates = new HashSet<string>();

            while (p1.Count > 0 && p2.Count > 0)
            {
                var state = String.Join(",", p1) + "|" + String.Join(",", p2);

                if (playStates.Contains(state))
                    return (1, p1);
                else
                    playStates.Add(state);

                var (p1Card, p2Card) = (p1.Dequeue(), p2.Dequeue());

                int winner;
                if (p1.Count >= p1Card && p2.Count >= p2Card)
                    (winner, _) = recursiveCombat(new Queue<int>(p1.Take(p1Card)), new Queue<int>(p2.Take(p2Card)));
                else
                    winner = p1Card > p2Card ? 1 : 2;

                if (winner == 1)
                {
                    p1.Enqueue(p1Card);
                    p1.Enqueue(p2Card);
                }
                else
                {
                    p2.Enqueue(p2Card);
                    p2.Enqueue(p1Card);
                }
            }

            return p1.Count > p2.Count ? (1, p1) : (2, p2);
        }

        public override string Part2()
        {
            var (_, winner) = recursiveCombat(new Queue<int>(player1), new Queue<int>(player2));

            return winner.Reverse().Select((val, idx) => val * (idx + 1)).Sum().ToString();
        }
    }
}
