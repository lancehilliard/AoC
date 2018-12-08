using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode._2018 {
    static partial class Program {
        static void Day3() {
            var inputs = File.ReadLines("inputs/Day3Input.txt");
            var grid = new (int id, List<int> ids)[1000, 1000];
            var fabricClaims = new List<(int id, int x, int y, int width, int height)>();
            foreach (var input in inputs) {
                var atSignParts = input.Split("@");
                var colonParts = atSignParts[1].Split(":");
                var commaParts = colonParts[0].Split(",");
                var xParts = colonParts[1].Split("x");
                var id = Convert.ToInt32(atSignParts[0].Replace("#",string.Empty).Trim());
                var x = Convert.ToInt32(commaParts[0].Trim())+1;
                var y = Convert.ToInt32(commaParts[1].Trim())+1;
                var width = Convert.ToInt32(xParts[0].Trim());
                var height = Convert.ToInt32(xParts[1].Trim());
                var fabricClaim = (id, x, y, width, height);
                fabricClaims.Add(fabricClaim);
                for (var i = x; i <= x-1+width; i++) {
                    for (var j = y; j <= y-1+height; j++) {
                        if (grid[i - 1, j - 1].ids == null) {
                            grid[i - 1, j - 1].ids = new List<int>();
                        }
                        grid[i - 1, j - 1].ids.Add(id);
                    }
                }
            }

            var overlappingInches = 0;
            for (var i = 0; i < 1000; i++) {
                for (var j = 0; j < 1000; j++) {
                    var location = grid[i,j];
                    if (location.ids != null) {
                        var idsCount = location.ids.Count;
                        if (idsCount > 1) {
                            overlappingInches=overlappingInches+1;
                        }
                    }
                }
            }
            Report($"Part 1: {overlappingInches}");

            foreach (var fabricClaim in fabricClaims) {
                var totalLocations = 0;
                var locationsWithoutOverlap = 0;
                for (var i = fabricClaim.x-1; i < fabricClaim.x+fabricClaim.width-1; i++) {
                    for (var j = fabricClaim.y - 1; j < fabricClaim.y+fabricClaim.height-1; j++) {
                        totalLocations++;
                        if (grid[i, j].ids != null && grid[i, j].ids.Count == 1) {
                            locationsWithoutOverlap++;
                        }
                    }
                }
                if (totalLocations == locationsWithoutOverlap) {
                    Report($"Part 2: {fabricClaim.id}");
                    break;
                }
            }
       }
    }
}