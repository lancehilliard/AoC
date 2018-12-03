﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode._2018 {
    static class Program {
        class FabricClaim {
            public int Id { get; set; }
            public int FirstColumn { get; set; }
            public int FirstRow { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }

            public int LastRow => FirstRow + Height - 1;
            public int LastColumn => FirstColumn + Width - 1;

            bool CoversRow(int rowNum) {
                return FirstRow <= rowNum && LastRow >= rowNum;
            }

            IEnumerable<int> CoveredRows => Enumerable.Range(FirstRow,LastRow-FirstRow+1);
            IEnumerable<int> CoveredColumns => Enumerable.Range(FirstColumn,LastColumn-FirstColumn+1);

            public bool Overlaps(FabricClaim other) {
                return other.CoveredColumns.Intersect(CoveredColumns).Any() || other.CoveredRows.Intersect(CoveredRows).Any();
            }

            bool CoversColumn(int columnNum) {
                return FirstColumn <= columnNum && LastColumn >= columnNum;
            }

            public bool CoversGridLocation(int gridColumn, int gridRow) {
                return CoversColumn(gridColumn) && CoversRow(gridRow);
            }
        }
        static readonly Stopwatch Timer = new Stopwatch();
        static TimeSpan _lastElapsedReported;
        static void Main() {
            Timer.Start();
            Day1();
            Day2();
            Day3();
            Console.WriteLine($"\nFinished.");
            Console.Write($"Total Runtime: {Timer.Elapsed}");
            Console.ReadKey();
        }

        private static void Day3() {
            var inputs = File.ReadLines("inputs/Day3Input.txt");
            var fabricClaims = new List<FabricClaim>();
            foreach (var input in inputs) {
                var atSignParts = input.Split("@");
                var colonParts = atSignParts[1].Split(":");
                var commaParts = colonParts[0].Split(",");
                var xParts = colonParts[1].Split("x");
                var id = Convert.ToInt32(atSignParts[0].Replace("#",string.Empty).Trim());
                var firstColumn = Convert.ToInt32(commaParts[0].Trim())+1;
                var firstRow = Convert.ToInt32(commaParts[1].Trim())+1;
                var width = Convert.ToInt32(xParts[0].Trim());
                var height = Convert.ToInt32(xParts[1].Trim());
                var fabricClaim = new FabricClaim{Id=id,FirstColumn = firstColumn,FirstRow = firstRow, Width = width,Height = height};
                fabricClaims.Add(fabricClaim);
            }
            var gridWidth = fabricClaims.Max(x=>x.LastColumn);
            var gridHeight = fabricClaims.Max(x=>x.LastRow);
            var claimIdWithNoOverlap = default(int);
            var squaresWithOverlap = new bool[gridWidth, gridHeight];
            foreach (var fabricClaim in fabricClaims) {
                var claimOverlapsAnotherClaim = false;
                var relevantFabricClaims = fabricClaims.Where(x => x.Overlaps(fabricClaim)).ToList();
                for (var gridRow = fabricClaim.FirstRow; gridRow <= fabricClaim.LastRow; gridRow++) {
                    for (var gridColumn = fabricClaim.FirstColumn; gridColumn <= fabricClaim.LastColumn; gridColumn++) {
                        if (relevantFabricClaims.Count(x => x.CoversGridLocation(gridColumn, gridRow)) > 1) {
                            squaresWithOverlap[gridColumn-1, gridRow-1] = true;
                            claimOverlapsAnotherClaim = true;
                        }
                    }
                }

                if (!claimOverlapsAnotherClaim) {
                    claimIdWithNoOverlap = fabricClaim.Id;
                }
            }

            var inchesWithOverlap = squaresWithOverlap.Cast<bool>().Count(x => x);

            Report($"Inches with overlap: {inchesWithOverlap}");
            Report($"Claim that does not overlap: {claimIdWithNoOverlap}");
        }

        private static void Day2() {
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

        static void Day1() {
            //*
            var frequencyDrifts = new[] {-10, +18, +5, +1, +1, -19, -13, -4, -4, -5, -17, +13, -3, +18, -17, +14, +11, +14, +16, +6, -2, -3, -3, -5, +18, +17, -9, +14, +13, +3, -17, -3, +11, -4, -5, +3, +9, -20, -18, +10, +1, +1, -14, +9, -5, +20, -6, +1, +2, +2, -13, -11, +18, +18, +1, +2, -11, +23, +2, -7, +2, +7, -3, +6, +9, +8, +13, -8, +12, +7, +4, +9, -11, -19, +1, +13, +15, -14, +10, +19, -6, +1, +7, -14, +3, +17, +2, +2, +15, +15, +18, +6, -9, +8, +15, +12, +15, +5, +8, +1, +6, -13, +18, +10, +10, +7, +14, +4, +9, +17, +10, +6, +7, +9, +5, +1, +1, +19, +10, +18, -5, +10, +7, -6, -10, +17, -12, -15, -17, +5, +7, -11, +18, -12, -12, -3, +10, -4, -5, +13, +10, -1, +7, -12, -11, -9, -18, +3, -1, -15, -7, -15, +4, -7, -4, -7, +19, -3, +14, +9, -11, -7, +16, -10, -3, +10, +13, +10, -7, +13, +11, +17, +7, +4, +20, +19, -1, -5, +10, +7, -9, -14, -3, -15, +5, -19, +13, +9, -12, -6, -13, -15, -2, -15, -3, +11, +22, +19, +18, -6, +10, -9, +10, +19, -15, -2, +20, +12, -20, -8, +23, +15, -11, +10, -2, +8, -4, -19, +16, +16, -6, -22, -2, +9, +13, -19, +8, +16, +18, +21, +17, +26, -19, +16, +8, -14, -1, +8, +9, +2, +6, +4, -7, -2, +1, +14, -3, +17, +6, +19, -14, +10, +13, -17, -10, +11, -14, -13, -13, +11, +19, +18, +1, -12, +14, -17, +8, +11, +1, +15, +13, -12, +14, +2, -8, +9, +9, +2, +14, +7, +14, -1, -11, +13, -11, -3, -18, +7, -19, -1, -14, +4, -11, +12, -6, -19, +6, +15, -10, -9, +24, -9, -1, +14, +8, -5, -5, +12, +16, -14, +3, -6, +12, +11, -18, +9, +17, +10, +11, -19, +13, +9, +4, +15, +8, -21, +5, -12, +4, +13, +13, +6, +1, +6, +4, +6, -14, -6, +17, -4, +10, +13, +17, +3, -5, +19, +6, -16, -8, +21, -19, +4, -15, -5, +7, -11, -19, +18, -2, +10, -17, -8, -8, -10, +7, -19, -6, -11, +19, +5, +9, -11, +16, -13, -17, +5, -18, -1, +12, -9, -18, +25, -3, -8, -15, +9, -19, -5, +18, +5, +3, -1, -12, -12, +8, -21, +3, +1, -32, -8, +2, -3, +27, +12, +38, -17, +42, +27, +10, +5, -1, -1, +10, -17, +11, +18, +2, -19, -11, +18, +7, +23, -5, +21, -20, +2, -6, -20, -7, +16, -8, +41, +15, -3, -18, +16, +3, -2, +16, +8, -1, +3, +17, +21, +6, +19, +8, -14, +8, +4, -1, +10, +7, -9, -20, -9, -60, +40, -14, -19, +38, -52, -13, -203, -31, -15, +1, -9, -12, +17, +10, -9, -5, -21, -5, -6, -11, +19, +10, -11, -16, -37, -22, -13, +5, +19, -14, -7, +13, -8, +26, +16, +19, -7, +28, +16, -2, +18, +27, +13, -150, +5, +22, -6, -22, -14, -136, -91, -7, -14, +31, -278, -73081, +12, +15, -16, +6, -8, -3, +19, -3, +10, +1, +1, +8, +9, +9, -12, +9, -2, +14, +9, -19, +15, +21, +19, +1, +3, +19, -11, +8, +20, -14, +16, +4, +6, -16, -3, -3, -13, -4, +5, +16, -5, -10, +3, -19, -5, +7, -16, -7, +10, +1, +18, +10, -12, -14, -15, -7, +14, +12, +16, +17, +11, +4, +12, +15, -17, +13, -8, +1, -12, -16, -10, -9, -10, +8, -20, +8, -5, -7, -16, +9, -8, -13, -12, -8, +2, +3, +16, -15, +11, +11, +2, -3, -17, -17, -3, -7, -20, +18, -9, +3, -5, -10, -17, +16, -2, -19, -18, -12, -14, -17, -3, +6, +10, +16, +3, +2, -16, -16, -10, -10, +14, -6, -10, +9, +15, +15, +7, -14, -2, +10, -14, -11, -2, +14, +16, +5, +20, +12, -18, +11, +9, +9, -3, -5, +12, +15, +6, -8, +3, +14, +15, -6, -12, -15, +23, -7, +18, -4, +18, +4, +9, +19, -7, +6, +3, -10, +18, -19, -20, -6, +17, -15, +3, +20, -5, -10, -12, -23, +5, -1, -14, -16, -17, -12, +17, -14, +4, -12, +18, +20, -15, -19, -1, -15, +10, -9, +1, +3, -18, +19, -14, -19, +17, -11, -3, -13, +7, +19, -15, +7, -33, -8, +2, +2, -18, -10, +16, -7, +4, -14, +8, +13, -5, +20, -14, +15, -11, -7, -22, -10, +11, +1, +16, -8, +16, -3, +15, +6, -17, +27, +14, +15, +8, +18, +21, -19, -25, +11, +10, +65, -11, -36, -15, -11, -25, +18, +35, +72, +1, +58, +3, +12, +19, -10, +21, +16, +11, +9, +1, +1, +9, -6, +20, -12, -20, +5, +8, -7, +18, +19, -13, -15, -24, +8, -16, +3, +2, -14, -1, -10, -16, -17, +21, +4, +14, -17, +18, -12, +10, +11, +2, -15, -4, -19, +13, +1, -18, -6, -23, -17, +19, +28, +6, +29, +10, +17, -6, +5, -11, +26, +25, -22, +2, -10, -29, +43, +17, +5, +4, +26, +1, +11, +19, -12, +20, +17, -13, +1, +19, +14, -19, -9, +2, +13, +4, -2, +19, +10, -14, +10, -9, +1, +20, +16, +6, -13, +4, +19, +16, -6, -7, -25, -20, +5, +2, -3, -9, -21, -19, +12, -8, -7, +17, -15, -18, -11, +6, +3, +15, -5, -5, -6, -18, -5, +19, -5, +18, -1, +2, -16, -6, -16, +17, -8, +16, +30, +18, +5, -2, +16, +19, -8, +9, -23, +12, +3, +18, +36, +50, -133, +7, +9, -68, +7, +2, -113, -85, -23, -211, -19, -6, -21, -11, -16, +19, +15, -13, -4, +14, +27, -12, -5, -18, -20, -16, -15, -15, -12, +1, +6, +9, -11, +18, +5, -14, -19, -9, +18, +2, -18, -11, +3, +13, -4, -4, +9, -10, -17, -4, +16, +13, +19, -15, +6, -3, +4, +10, +20, +17, -24, -5, +1, +8, +27, +6, +4, +20, +8, -3, +66, -139, -21, +11, -19, +20, -13, -31, +7, -2, +5, -2, +4, -16, +7, +6, -21, -10, -12, +25, -7, -17, -13, +73906}.ToList();
            var totalDrift = frequencyDrifts.Aggregate(default(int), (x, y) => x + y);
            Report($"Total drift is {totalDrift}.");

            //**
            var lastFrequency = default(int);
            var reachedFrequencyCounts = new SortedDictionary<int, short>();
            var duplicatedFrequency = default(int?);
            while (!duplicatedFrequency.HasValue) {
                foreach (var frequencyDrift in frequencyDrifts) {
                    lastFrequency = lastFrequency + frequencyDrift;
                    if (reachedFrequencyCounts.ContainsKey(lastFrequency)) {
                        duplicatedFrequency = lastFrequency;
                        break;
                    }
                    reachedFrequencyCounts[lastFrequency] = 1;
                }
            }
            Report($"Repeated frequency is {lastFrequency}.");
        }

        static void Report(string x, [CallerMemberName] string callerMemberName = null) {
            var elapsed = Timer.Elapsed;
            Console.WriteLine($"[{(_lastElapsedReported.Milliseconds.Equals(0) ? " " : "+")}{elapsed-_lastElapsedReported}] {callerMemberName}: {x}");
            _lastElapsedReported = elapsed;
        }
    }
}