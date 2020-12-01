using System;

namespace aoc2020
{
    class Program
    {
        static void Main(string[] args)
        {
            var day1 = new Day1("./input/day1.txt");
            Console.WriteLine($"Day1, Part1: {day1.Part1()}, Part2: {day1.Part2()}");
        }
    }
}
