using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode._2018 {
    static partial class Program {
        static void Day7() {
            var inputs = File.ReadLines("inputs/Day7Input.txt").ToList();
            var prerequisites = new Dictionary<char, HashSet<char>>();
            var prerequisites2 = new Dictionary<char, HashSet<char>>();
            foreach (var input in inputs) {
                var key = input[36];
                var prerequisite = input[5];
                if (!prerequisites.ContainsKey(key)) {
                    prerequisites[key] = new HashSet<char>();
                    prerequisites2[key] = new HashSet<char>();
                }
                if (!prerequisites.ContainsKey(prerequisite)) {
                    prerequisites[prerequisite] = new HashSet<char>();
                    prerequisites2[prerequisite] = new HashSet<char>();
                }
                prerequisites[key].Add(prerequisite);
                prerequisites2[key].Add(prerequisite);
            }
            var part1 = new List<KeyValuePair<char, HashSet<char>>>();
            while (prerequisites.Any())
            {
                var keyValuePair = prerequisites.OrderBy(x => x.Value.Distinct().Count()).ThenBy(x => x.Key).First();
                part1.Add(keyValuePair);
                prerequisites.Remove(keyValuePair.Key);
                foreach (var prerequisite in prerequisites)
                {
                    prerequisite.Value.Remove(keyValuePair.Key);
                }
            }
            Report($"Part 1: {string.Join(string.Empty, part1.Select(x=>x.Key))}");

            IEnumerable<KeyValuePair<char, HashSet<char>>> part2 = prerequisites2.OrderBy(x => x.Value.Distinct().Count()).ThenBy(x => x.Key).Where(x=>part1.Any(y=>y.Key.Equals(x.Key))).ToList();

            var workerCount = 5;
            var additionalStepDuration = 60;
            var workers = new char?[workerCount];
            var secondsRemaining = new Dictionary<char, int>();
            foreach (var job in part2)
            {
                var keyNumber = job.Key - 64;
                secondsRemaining[job.Key] = keyNumber + additionalStepDuration;
            }
            int seconds;
            for (seconds = 0; secondsRemaining.Any(); seconds++)
            {
                var unfinishedJobsWithNoRemainingPrerequisites = part2.Where(x => !x.Value.Any()).ToList();
                for (int i = 0; i < workers.Length; i++)
                {
                    if (!workers[i].HasValue)
                    {
                        var jobsForWorker = unfinishedJobsWithNoRemainingPrerequisites.Where(x=>secondsRemaining.Any(y=>y.Key.Equals(x.Key))&&!workers.Contains(x.Key)).OrderBy(y => y.Value.Distinct().Count()).ThenBy(y => y.Key).ToList();
                        if (jobsForWorker.Any()) {
                            var jobKey = jobsForWorker.First().Key;
                            workers[i] = jobKey;
                        }
                    }
                }

                for (int i = 0; i < workers.Length; i++) {
                    if (workers[i].HasValue)
                    {
                        var jobKey = workers[i].Value;
                        secondsRemaining[jobKey]--;
                        var secondsRemainingOnJob = secondsRemaining[jobKey];
                        if (secondsRemainingOnJob == 0)
                        {
                            secondsRemaining.Remove(jobKey);
                            foreach (var keyValuePair in part2)
                            {
                                keyValuePair.Value.Remove(jobKey);
                            }
                            workers[i] = null;
                        }
                    }
                }
            }
            Report($"Part 2: {seconds}");
        }
    }
}