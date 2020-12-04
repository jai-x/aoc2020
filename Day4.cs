using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace aoc2020
{
    public class Day4 : Day
    {
        private class Passport
        {
            private Dictionary<string, string> fields;

            private Dictionary<string, Func<string, bool>> fieldValidations = new Dictionary<string, Func<string, bool>>
            {
                { "byr", (f) => Int32.TryParse(f, out var year) && (year >= 1920) && (year <= 2002) },
                { "iyr", (f) => Int32.TryParse(f, out var year) && (year >= 2010) && (year <= 2020) },
                { "eyr", (f) => Int32.TryParse(f, out var year) && (year >= 2020) && (year <= 2030) },
                { "hgt", (f) => Int32.TryParse(f[..^2], out var hgt) && (((f[^2..^0] == "cm") && (hgt >= 150) && (hgt <= 193)) || ((f[^2..^0] == "in") && (hgt >= 59) && (hgt <= 76)))  }, // one line expression >_<
                { "hcl", (f) => f[0] == '#' && Int32.TryParse(f[1..], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _)},
                { "ecl", (f) => new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Any(f.Equals) },
                { "pid", (f) => f.Length == 9 && Int32.TryParse(f, out _) },
            };

            public Passport(string input)
            {
                fields = input
                    .Trim()
                    .Split(new[] { ' ', '\n' })
                    .Select(token => token.Split(':'))
                    .ToDictionary(pairArray => pairArray[0], pairArray => pairArray[1]);
            }

            public bool PresentFields() => fieldValidations.Keys.All(k => fields.ContainsKey(k));

            public bool ValidFields() => PresentFields()
                && fieldValidations.Keys
                .Select(k => (fields[k], fieldValidations[k]))
                .All(pair => pair.Item2.Invoke(pair.Item1));
        }

        private List<Passport> passports;

        public Day4(string input) : base(input)
        {
            passports = input
                .Split("\n\n")
                .Select(pInput => new Passport(pInput))
                .ToList();
        }

        public override string Part1() => passports.Count(p => p.PresentFields()).ToString();
        public override string Part2() => passports.Count(p => p.ValidFields()).ToString();
    }
}
