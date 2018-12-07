using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode._2018 {
    static partial class Program {
        static void Day7() {
            var inputs = File.ReadLines("inputs/Day7Input.txt");
            var prerequisites = new Dictionary<char, HashSet<char>>();
            foreach (var input in inputs) {
                // Step C must be finished before step A can begin. 5 36
                var key = input[36];
                var prerequisite = input[5];
                if (!prerequisites.ContainsKey(key)) {
                    prerequisites[key] = new HashSet<char>();
                }
                if (!prerequisites.ContainsKey(prerequisite)) {
                    prerequisites[prerequisite] = new HashSet<char>();
                }
                prerequisites[key].Add(prerequisite);
            }

            var part1 = string.Empty;
            while (prerequisites.Any()) {
                var keyValuePair = prerequisites.OrderBy(x=>x.Value.Distinct().Count()).ThenBy(x=>x.Key).First();
                part1 = $"{part1}{keyValuePair.Key}";
                prerequisites.Remove(keyValuePair.Key);
                foreach (var prerequisite in prerequisites) {
                    prerequisite.Value.Remove(keyValuePair.Key);
                }
            }

            Report($"Part 1: {part1}");

        }
    }
}