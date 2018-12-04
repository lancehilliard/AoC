using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode._2018 {
    static partial class Program {
        static void Day3() {
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
                var fabricClaim = new FabricClaim(id, firstColumn, firstRow, width, height);
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
    }
}