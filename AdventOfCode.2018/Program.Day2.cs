using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode._2018 {
    static partial class Program {
        static void Day2() {
            var boxIds = File.ReadLines("inputs/Day2Input.txt").OrderBy(x => x).ToList();
            var twoCount = default(int);
            var threeCount = default(int);
            var correctBoxIds = new List<string>();
            foreach (var boxId in boxIds) {
                var chars = boxId.ToCharArray();
                var charCounts = chars.Select(x => chars.Count(y => x == y)).ToList();
                twoCount = twoCount + (charCounts.Contains(2) ? 1 : 0);
                threeCount = threeCount + (charCounts.Contains(3) ? 1 : 0);
                if (!correctBoxIds.Any()) {
                    var otherBoxIdsOfSameLength = boxIds.Where(x=>!x.Equals(boxId) && x.Length==boxId.Length);
                    foreach (var otherBoxId in otherBoxIdsOfSameLength) {
                        var differingLetterCount = otherBoxId.Where((_, index) => boxId.ElementAt(index) != otherBoxId.ElementAt(index)).Count();
                        if (differingLetterCount == 1) {
                            correctBoxIds.Add(boxId);
                            correctBoxIds.Add(otherBoxId);
                        }
                    }
                }
            }
            var checkSum = twoCount * threeCount;
            Report($"Checksum: {checkSum}");

            var firstCorrectBoxId = correctBoxIds.First();
            var secondCorrectBoxId = correctBoxIds.Last();
            var correctBoxIdCommonLetters = new List<char>();
            for (var i = 0; i < firstCorrectBoxId.Length; i++) {
                if (firstCorrectBoxId.ElementAt(i) == secondCorrectBoxId.ElementAt(i)) {
                    correctBoxIdCommonLetters.Add(firstCorrectBoxId.ElementAt(i));
                }
            }
            Report($"Common box ID letters: {string.Join(string.Empty, correctBoxIdCommonLetters)}");
        }
    }
}