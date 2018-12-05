using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode._2018 {
    static partial class Program {
        static void Day5() {
            var input = File.ReadLines("inputs/Day5Input.txt").First();
            var simpleCollapsedPolymer = React(input);
            Report($"Part 1: {simpleCollapsedPolymer.Length}");

            var collapsedPolymerLengths = new Dictionary<string, string>();
            for (var character = 'A'; character <= 'Z'; character++) {
                var letter = $"{character}";
                var unitAdjustedPolymer = input.Replace(letter, String.Empty, StringComparison.InvariantCultureIgnoreCase);
                var adjustedCollapsedPolymer = React(unitAdjustedPolymer);
                collapsedPolymerLengths[letter] = adjustedCollapsedPolymer;
            }
            Report($"Part 2: {collapsedPolymerLengths.OrderBy(x => x.Value.Length).First().Value.Length.ToString()}");

            string React(string polymer) {
                var startingIndex = polymer.Length - 1;
                while (true) {
                    var chars = polymer.ToCharArray();
                    int polymerIndex;
                    for (polymerIndex = startingIndex; polymerIndex > 0; polymerIndex--) {
                        var unit1Char = chars[polymerIndex];
                        var unit2Char = chars[polymerIndex - 1];
                        var lowerCaseUnit1Letter = $"{unit1Char}".ToLower();
                        var lowerCaseUnit2Letter = $"{unit2Char}".ToLower();
                        var reacts = string.Equals(lowerCaseUnit1Letter, lowerCaseUnit2Letter, StringComparison.InvariantCultureIgnoreCase) && unit1Char != unit2Char;
                        if (reacts) {
                            startingIndex = polymerIndex - (polymerIndex.Equals(polymer.Length-1) ? 2 : 1);
                            polymer = polymer.Remove(polymerIndex - 1, 2);
                            break;
                        }
                    }
                    if (polymerIndex == 0) {
                        break;
                    }
                }
                return polymer;
            }
        }
    }
}