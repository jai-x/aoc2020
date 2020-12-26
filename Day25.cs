using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    public class Day25 : Day
    {
        private long cardPubKey;
        private long cardSecretLoop = 0L;

        private long doorPubKey;
        private long doorSecretLoop = 0L;

        public Day25(string input) : base(input)
        {
            var keys = input.Trim().Split('\n').Select(Int64.Parse);
            (cardPubKey, doorPubKey) = (keys.First(), keys.Last());
        }

        private long handshake(long val, long subject) => (val * subject) % 20201227;

        private void log(object msg) => Console.WriteLine(msg);

        public override string Part1()
        {
            var cardHandshake = 1L;
            while (true)
            {
                if (cardHandshake == cardPubKey)
                    break;

                cardHandshake = handshake(cardHandshake, 7);

                cardSecretLoop++;
            }

            var doorHandshake = 1L;
            while (true)
            {
                if (doorHandshake == doorPubKey)
                    break;

                doorHandshake = handshake(doorHandshake, 7);

                doorSecretLoop++;
            }

            var key1 = Enumerable.Range(0, (int)cardSecretLoop)
                                 .Aggregate(1L, (val, _) => handshake(val, doorPubKey));

            var key2 = Enumerable.Range(0, (int)doorSecretLoop)
                                 .Aggregate(1L, (val, _) => handshake(val, cardPubKey));

            if (key1 == key2)
                return key1.ToString();
            else
                return "Error";
        }

        public override string Part2() => "Merry Christmas!";
    }
}
