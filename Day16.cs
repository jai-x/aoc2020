using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    class Day16 : Day
    {
        private Dictionary<string, Func<int, bool>> fieldValidations = new Dictionary<string, Func<int, bool>>();
        private long[] myTicket;
        private int[][] otherTickets;

        public Day16(string input) : base(input)
        {
            var parts = input.Trim()
                             .Split("\n\n")
                             .Select(part => part.Trim())
                             .ToArray();

            var (fieldStr, myTicketStr, otherTicketsStr) = (parts[0], parts[1], parts[2]);

            foreach (var field in fieldStr.Split('\n').Select(f => f.Trim()))
            {
                var fieldParts = field.Replace(" or ", ":")
                                      .Replace("-", ":")
                                      .Split(':')
                                      .Select(f => f.Trim())
                                      .ToArray();

                var fieldName = fieldParts.First();
                var fieldConstraints = fieldParts.Skip(1).Select(Int32.Parse).ToArray();
                var (l1, h1, l2, h2) = (fieldConstraints[0], fieldConstraints[1], fieldConstraints[2], fieldConstraints[3]);

                fieldValidations.Add(fieldName, f => ((l1 <= f) && (f <= h1)) || ((l2 <= f) && (f <= h2)));
            }

            myTicket = myTicketStr.Split('\n')
                                  .Last()
                                  .Split(',')
                                  .Select(Int64.Parse)
                                  .ToArray();

            otherTickets = otherTicketsStr.Split('\n')
                                          .Skip(1)
                                          .Select(t => t.Split(','))
                                          .Select(t => t.Select(Int32.Parse))
                                          .Select(t => t.ToArray())
                                          .ToArray();
        }

        public override string Part1()
        {
            var errorValues = new List<int>();

            foreach (var ticket in otherTickets)
                foreach (var val in ticket)
                    if (!fieldValidations.Values.Any(v => v.Invoke(val)))
                        errorValues.Add(val);

            return errorValues.Sum().ToString();
        }

        public override string Part2()
        {
            var otherTicketsValid = new List<int[]>();

            foreach (var ticket in otherTickets)
            {
                var ticketValid = true;

                foreach (var val in ticket)
                    if (!fieldValidations.Values.Any(v => v.Invoke(val)))
                        ticketValid = false;

                if (ticketValid)
                    otherTicketsValid.Add(ticket);
            }

            var validationsPerCol = fieldValidations.Keys
                                                    .Select(_ => new List<string>())
                                                    .ToList();

            foreach (var ticketValues in otherTicketsValid)
            {
                for (var col = 0; col < ticketValues.Length; col++)
                {
                    var val = ticketValues[col];
                    foreach (var (fieldName, validation) in fieldValidations)
                        if (validation.Invoke(val))
                            validationsPerCol[col].Add(fieldName);
                }
            }

            var fieldToCol = new Dictionary<string, int>();
            var skip = new List<int>();

            while (skip.Count != validationsPerCol.Count)
            {
                for (var i = 0; i < validationsPerCol.Count; i++)
                {
                    if (skip.Contains(i))
                        continue;

                    var matching = validationsPerCol[i];

                    var common = mostCommon(matching);

                    if (common == null)
                        continue;

                    skip.Add(i);

                    for (var j = 0; j < validationsPerCol.Count; j++)
                        validationsPerCol[j] = validationsPerCol[j].Where(v => v != common).ToList();

                    fieldToCol.Add(common, i);

                    break;
                }
            }

            return fieldToCol.Select(kvp => kvp.Key.StartsWith("departure") ? myTicket[kvp.Value] : 1L)
                             .Aggregate(1L, (acc, val) => acc * val)
                             .ToString();
        }

        private string mostCommon(List<string> list)
        {
            if (list.Count == 1)
                return list.First();

            var counts = list.GroupBy(f => f)
                             .Select(grp => (field: grp.Key, count: grp.Count()))
                             .OrderByDescending(tup => tup.count)
                             .ToArray();

            if (counts.Length == 1)
                return counts.First().Item1;

            if (counts[0].Item2 == counts[1].Item2)
                return null;

            return counts.First().Item1;
        }
    }
}
