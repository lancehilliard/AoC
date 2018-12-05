using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2018 {
    static partial class Program {
        static void Day5() {
            var input = File.ReadAllText("inputs/Day5Input.txt");
            Report($"Part 1: {React(input).Length}");

            var collapsedPolymerLengths = new Dictionary<string, string>();
            for (var character = 'A'; character <= 'Z'; character++) {
                var letter = $"{character}";
                var unitAdjustedPolymer = input.Replace(letter, string.Empty, StringComparison.InvariantCultureIgnoreCase);
                var adjustedCollapsedPolymer = React(unitAdjustedPolymer);
                collapsedPolymerLengths[letter] = adjustedCollapsedPolymer;
            }
            Report($"Part 2: {collapsedPolymerLengths.OrderBy(x => x.Value.Length).First().Value.Length.ToString()}");

            string React(string polymer) {
                var startingIndex = polymer.Length - 1;
                var polymerIndex = startingIndex;
                while (polymerIndex > 0) {
                    var chars = polymer.ToCharArray();
                    for (polymerIndex = startingIndex; polymerIndex > 0; polymerIndex--) {
                        if (Math.Abs(chars[polymerIndex]-chars[polymerIndex - 1])==32) {
                            startingIndex = polymerIndex - (polymerIndex.Equals(polymer.Length-1) ? 2 : 1);
                            polymer = polymer.Remove(polymerIndex - 1, 2);
                            break;
                        }
                    }
                }
                return polymer;
            }
        }
    }
}