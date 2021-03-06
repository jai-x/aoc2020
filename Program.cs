﻿using System;
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
            { 1,  (typeof(Day1),  "./input/day1.txt" ) },
            { 2,  (typeof(Day2),  "./input/day2.txt" ) },
            { 3,  (typeof(Day3),  "./input/day3.txt" ) },
            { 4,  (typeof(Day4),  "./input/day4.txt" ) },
            { 5,  (typeof(Day5),  "./input/day5.txt" ) },
            { 6,  (typeof(Day6),  "./input/day6.txt" ) },
            { 7,  (typeof(Day7),  "./input/day7.txt" ) },
            { 8,  (typeof(Day8),  "./input/day8.txt" ) },
            { 9,  (typeof(Day9),  "./input/day9.txt" ) },
            { 10, (typeof(Day10), "./input/day10.txt") },
            { 11, (typeof(Day11), "./input/day11.txt") },
            { 12, (typeof(Day12), "./input/day12.txt") },
            { 13, (typeof(Day13), "./input/day13.txt") },
            { 14, (typeof(Day14), "./input/day14.txt") },
            { 15, (typeof(Day15), "./input/day15.txt") },
            { 16, (typeof(Day16), "./input/day16.txt") },
            { 17, (typeof(Day17), "./input/day17.txt") },
            { 18, (typeof(Day18), "./input/day18.txt") },
            { 19, (typeof(Day19), "./input/day19.txt") },
            { 20, (typeof(Day20), "./input/day20.txt") },
            { 21, (typeof(Day21), "./input/day21.txt") },
            { 22, (typeof(Day22), "./input/day22.txt") },
            { 23, (typeof(Day23), "./input/day23.txt") },
            { 24, (typeof(Day24), "./input/day24.txt") },
            { 25, (typeof(Day25), "./input/day25.txt") },
        };

    }

    public abstract class Day
    {
        public Day(string input) { }
        public abstract string Part1();
        public abstract string Part2();
    }
}
