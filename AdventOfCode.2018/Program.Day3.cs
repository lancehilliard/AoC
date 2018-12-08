using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode._2018 {
    static partial class Program {
        static void Day3() {
            var inputs = File.ReadLines("inputs/Day3Input.txt");
            var grid = new List<int>[1000, 1000];
            var claims = new List<(int id, int x, int y, int width, int height)>();
            var overlappingInches = 0;
            foreach (var input in inputs) {
                var atSignParts = input.Split("@").Select(s=>s.Trim()).ToArray();
                var colonParts = atSignParts[1].Split(":");
                var commaParts = colonParts[0].Split(",").Select(s=>s.Trim()).ToArray();
                var xParts = colonParts[1].Split("x").Select(s=>s.Trim()).ToArray();
                var id = Convert.ToInt32(atSignParts[0].Substring(1));
                var x = Convert.ToInt32(commaParts[0]);
                var y = Convert.ToInt32(commaParts[1]);
                var width = Convert.ToInt32(xParts[0]);
                var height = Convert.ToInt32(xParts[1]);
                var claim = (id, x, y, width, height);
                claims.Add(claim);
                for (var i = x; i < x+width; i++) {
                    for (var j = y; j < y+height; j++) {
                        if (grid[i, j] == null) {
                            grid[i, j] = new List<int>();
                        } else if (grid[i, j].Count == 1) {
                            overlappingInches=overlappingInches+1;
                        }
                        grid[i, j].Add(id);
                    }
                }
            }
            Report($"Part 1: {overlappingInches}");

            foreach (var (id, x, y, width, height) in claims) {
                var locationsWithoutOverlap = 0;
                for (var i = x; i < x+width; i++) {
                    for (var j = y; j < y+height; j++) {
                        if (grid[i, j] != null && grid[i, j].Count == 1) {
                            locationsWithoutOverlap++;
                        }
                    }
                }
                if (width*height == locationsWithoutOverlap) {
                    Report($"Part 2: {id}");
                    break;
                }
            }
       }
    }
}