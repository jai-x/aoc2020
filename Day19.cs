using System;
using System.Linq;
using System.Collections.Generic;

namespace aoc2020
{
    public class Day19 : Day
    {
        private class Rule
        {
            private static Dictionary<(int, string), string> memo = new Dictionary<(int, string), string>();

            private int self;
            private Dictionary<int, Rule> rules;
            private int[][] lookups;
            private char? literal = null;

            public Rule(string rule, Dictionary<int, Rule> rules, int self)
            {
                this.self = self;
                this.rules = rules;

                if (rule.Length == 3 && rule[0] == '"')
                    literal = rule[1];
                else
                    lookups = rule.Split('|')
                                  .Select(l => l.Trim().Split(' ').Select(Int32.Parse).ToArray())
                                  .ToArray();
            }

            public static void ResetMemo() => memo.Clear();

            public override string ToString()
            {
                var toString = $"[{nameof(Rule)}: {self}] ";

                toString += (literal != null)
                         ? $"literal ({literal})"
                         : "";

                toString += (lookups != null)
                         ? "(" + String.Join(" | ", lookups?.Select(l => String.Join(" ", l))) + ")"
                         : "";

                return toString;
            }

            private void padLog(int level, string log)
            {
                Console.WriteLine(new String(' ', level) + this.ToString() + log);
            }

            public string Match(string input, int level = 0)
            {
                if (String.IsNullOrEmpty(input))
                    return null;

                padLog(level, ", matching? " + input);

                if (memo.ContainsKey((self, input)))
                {
                    var ret = memo[(self, input)];
                    padLog(level, ", memo? " +  input + " returning! " + ret);
                    return ret;
                }


                if (literal != null)
                {
                    if (input.StartsWith((char)literal))
                    {
                        padLog(level, ", matched! " + literal);
                        var ret = input[1..];
                        padLog(level, ", returning! " + ((ret == "") ? "(empty)" : ret));
                        return ret;
                    }
                    else
                    {
                        padLog(level, ", returning! null");
                        return null;
                    }
                }

                foreach (var subrules in lookups)
                {
                    padLog(level, ", subrules? " + String.Join(", ", subrules));

                    string result = input.Clone() as string;
                    foreach (var r in subrules)
                    {
                        result = rules[r].Match(result, level + 2);
                    }

                    if (result != null)
                    {
                        padLog(level, ", subrules! " + String.Join(", ", subrules) + ", returning! " + result);
                        padLog(level, ", memo! " +  input);
                        memo.Add((self, input), result);
                        return result;
                    }
                }

                padLog(level, ", memo! " +  input);
                memo.Add((self, input), null);
                return null;
            }
        }

        private Dictionary<int, Rule> rules = new Dictionary<int, Rule>();

        private string[] messages;

        public Day19(string input) : base(input)
        {
            var inputParts = input.Trim().Split("\n\n");
            var (rulesStr, messagesStr) = (inputParts[0], inputParts[1]);

            messages = messagesStr.Split('\n').Select(m => m.Trim()).ToArray();

            foreach (var ruleStr in rulesStr.Trim().Split('\n').Select(r => r.Trim()))
            {
                var parts = ruleStr.Split(':');
                var num = Int32.Parse(parts[0]);
                var rule = new Rule(parts[1].Trim(), rules, num);
                rules.Add(num, rule);
            }
        }

        public override string Part1()
        {
            return messages.Where(m => rules[0].Match(m) == "").ToArray().Count().ToString();
        }

        public override string Part2()
        {
            rules[8] = new Rule("42 | 42 8", rules, 8);
            rules[11] = new Rule("42 31 | 42 11 31", rules, 11);

            Rule.ResetMemo();

            return messages.Where(m => rules[0].Match(m) == "").ToArray().Count().ToString();
        }
    }
}
