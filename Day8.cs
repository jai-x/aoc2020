using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    public class Day8 : Day
    {
        private List<(string, int)> instructions;

        public Day8(string input) : base(input)
        {
            instructions = input
                .Trim()
                .Split('\n')
                .Select(inst => inst.Split(' '))
                .Select(inst => (inst[0].Trim(), Int32.Parse(inst[1])))
                .ToList();
        }

        /// <summary> Runs the Game console instructions </summary>
        /// <returns> (int, bool) value of accumulator, if program is valid </returns>
        private (int, bool) runCode(List<(string, int)> instructions)
        {
            var position = 0;
            var visited = new HashSet<int>();
            var accumulator = 0;

            while (true)
            {
                // Looping
                if (visited.Contains(position))
                    return (accumulator, false);

                // Invalid jump
                if (position > instructions.Count || position < 0)
                    return (accumulator, false);

                visited.Add(position);

                var (opcode, argument) = instructions[position];

                switch (opcode)
                {
                    case "nop":
                        position++;
                        break;

                    case "acc":
                        accumulator += argument;
                        position++;
                        break;

                    case "jmp":
                        position += argument;
                        break;
                }

                // Clean exit
                if (position == instructions.Count)
                    return (accumulator, true);
            }
        }

        public override string Part1()
        {
            var (acc, _) = runCode(instructions);
            return acc.ToString();
        }

        private string swap(string op)
        {
            if (op == "jmp")
                return "nop";

            if (op == "nop")
                return "jmp";

            throw new ArgumentException();
        }

        public override string Part2()
        {
            for (var i = 0; i < instructions.Count; i++) 
            {
                var (opcode, argument) = instructions[i];

                if (opcode == "acc")
                    continue;

                if (opcode == "jmp" || opcode == "nop")
                {
                    instructions[i] = (swap(opcode), argument);

                    var (acc, valid) = runCode(instructions);

                    if (valid)
                        return acc.ToString();
                    else
                        instructions[i] = (opcode, argument);
                }
            }

            return "Error";
        }
    }
}
