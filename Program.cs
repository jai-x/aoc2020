using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    class Program
    {
        static void Main(string[] args)
        {
            var success = Int32.TryParse(args.ElementAtOrDefault(0), out var dayNumber);
            if (!success)
            {
                Console.WriteLine("Please enter the day number as the first argument");
                return;
            }

            if (!days.ContainsKey(dayNumber))
            {
                Console.WriteLine($"Please enter a valid day number ({days.Keys.Min()}-{days.Keys.Max()})");
                return;
            }

            var (dayType, inputFile) = days[dayNumber];

            var input = File.ReadAllText(args.ElementAtOrDefault(1) ?? inputFile);
            var day = Activator.CreateInstance(dayType, input) as Day;

            Console.WriteLine($"Day {dayNumber}, Part 1: {day.Part1()}, Part 2: {day.Part2()}");
        }

        private static Dictionary<int, (Type, string)> days = new Dictionary<int, (Type, string)>
        {
            { 1, (typeof(Day1), "./input/day1.txt") },
            { 2, (typeof(Day2), "./input/day2.txt") },
            { 3, (typeof(Day3), "./input/day3.txt") },
            { 4, (typeof(Day4), "./input/day4.txt") },
            { 5, (typeof(Day5), "./input/day5.txt") },
            { 6, (typeof(Day6), "./input/day6.txt") },
            { 7, (typeof(Day7), "./input/day7.txt") },
        };

    }

    public abstract class Day
    {
        public Day(string input) { }
        public abstract string Part1();
        public abstract string Part2();
    }
}
