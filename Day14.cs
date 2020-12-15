using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace aoc2020
{
    class Day14 : Day
    {
        private List<(string, long, long)> program = new List<(string, long, long)>();

        public Day14(string input) : base(input)
        {
            var currentMask = String.Empty;
            foreach (var line in input.Trim().Split('\n').Select(line => line.Trim()))
            {
                if (line.Contains("mask"))
                {
                    currentMask = line.Split('=').Reverse().First().Trim();
                    continue;
                }

                if (line.Contains("mem"))
                {
                    var parts = line.Split('=')
                                    .Select(part => part.Replace("mem[", "").Replace("]", ""))
                                    .Select(part => part.Trim())
                                    .Select(part => Int64.Parse(part))
                                    .ToArray();

                    var addr = parts[0];
                    var val = parts[1];

                    program.Add((currentMask, addr, val));
                }
            }
        }

        private string longToBitString(long i)
        {
            var valStr = Convert.ToString(i, 2);
            return (valStr.Length > 36) ? valStr[^36..^0] : valStr.PadLeft(36, '0');
        }

        private long bitStringToLong(string i) => Convert.ToInt64(i, 2);

        private long applyMask(long val, string mask)
        {
            var valStr = longToBitString(val);
            var result = new String(mask.Zip(valStr, (m, v) => m == 'X' ? v : m).ToArray());
            return bitStringToLong(result);
        }

        private long[] floatingMask(long val, string mask)
        {
            var ret = new List<long>();

            var valStr = longToBitString(val);
            var floating = mask.Zip(valStr, (m, v) => m == '0' ? v : m).ToArray();

            var xs = floating.Where(bit => bit == 'X').Count();
            var iterations = (long) BigInteger.Pow(2, xs);

            for (var floatVal = 0L; floatVal < iterations; floatVal++)
            {
                var floatingCopy = floating.Clone() as char[];

                var floatVals = new Stack<char>(Convert.ToString(floatVal, 2).PadLeft(xs, '0').ToArray());

                for (int i = 0; i < floatingCopy.Length; i++)
                {
                    if (floatingCopy[i] == 'X')
                        floatingCopy[i] = floatVals.Pop();
                }

                ret.Add(bitStringToLong(new String(floatingCopy)));
            }

            return ret.ToArray();
        }

        public override string Part1()
        {
            var memory = new Dictionary<long, long>();

            foreach (var (mask, addr, val) in program)
                memory[addr] = applyMask(val, mask);

            return memory.Values.Sum().ToString();
        }

        public override string Part2()
        {
            var memory = new Dictionary<long, long>();

            foreach (var (mask, addr, val) in program)
                foreach (var a in floatingMask(addr, mask))
                    memory[a] = val;

            return memory.Values.Sum().ToString();
        }
    }
}
