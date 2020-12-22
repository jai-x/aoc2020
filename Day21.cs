using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    public class Day21 : Day
    {
        private class Food
        {
            public HashSet<string> Ingredients;
            public HashSet<string> Allergens;

            public override string ToString() => $"{String.Join(", ", Ingredients)} -> {String.Join(", ", Allergens)}";
        }

        private Food[] foods;

        public Day21(string input) : base(input)
        {
            foods = input.Trim()
                         .Split('\n')
                         .Select(line => line.Replace("(contains", ":").Replace(")", "").Replace(",", ""))
                         .Select(line => line.Split(":").Select(p => p.Trim()))
                         .Select(parts => parts.Select(part => part.Split(' ').Select(p => p.Trim()).ToHashSet()))
                         .Select(parts => new Food { Ingredients = parts.First(), Allergens = parts.Last() })
                         .ToArray();
        }

        private Dictionary<string, HashSet<string>> algToIng = new Dictionary<string, HashSet<string>>();

        public override string Part1()
        {
            foreach (var food in foods)
                foreach (var alg in food.Allergens)
                    if (!algToIng.ContainsKey(alg))
                        algToIng[alg] = new HashSet<string>(food.Ingredients);
                    else
                        algToIng[alg].IntersectWith(food.Ingredients);

            var ingWithAlg = algToIng.Select(kvp => kvp.Value)
                                     .Aggregate(new HashSet<string>(), (agg, list) => agg.Concat(list).ToHashSet());

            return foods.Select(f => f.Ingredients.Where(i => !ingWithAlg.Contains(i)).Count())
                        .Sum()
                        .ToString();
        }

        public override string Part2()
        {
            var canonAlgToIng = new SortedDictionary<string, string>();

            while (canonAlgToIng.Count != algToIng.Count)
            {
                var next = algToIng.Where(kvp => kvp.Value.Count == 1 && !canonAlgToIng.ContainsKey(kvp.Key)).First();
                var (allergen, ingredient) = (next.Key, next.Value.First());

                foreach (var (_, ings) in algToIng)
                    if (ings.Contains(ingredient))
                        ings.Remove(ingredient);

                canonAlgToIng[allergen] = ingredient;
            }

            return String.Join(",", canonAlgToIng.Values);
        }
    }
}
