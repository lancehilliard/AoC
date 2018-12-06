using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode._2018 {
    static partial class Program {
        static void Day6() {
            var inputs = File.ReadLines("inputs/Day6Input.txt");
            var coordinates = inputs.Select(input => input.Split(", ")).Select(strings => strings.Select(x => Convert.ToInt32(x)).ToArray()).ToList();
            var maxX = coordinates.Max(x=>x[0]);
            var maxY = coordinates.Max(x=>x[1]);
            var gridSize = maxY > maxX ? maxY : maxX;
            
            var (grid, finiteCoordinateIndexes) = BuildGrid();
            var finiteAreas = GetCoordinateAreas(finiteCoordinateIndexes);

            var largestAreaCoordinate = finiteAreas.OrderByDescending(x=>x.Value).First();
            var largestArea = largestAreaCoordinate.Value;
            Report($"Part 1: {largestArea}");
            var regionArea = GetRegionArea();
            Report($"Part 2: {regionArea}");

            int ManhattanDistance(int x1, int x2, int y1, int y2) {
                return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
            }

            (int?[,], HashSet<int>) BuildGrid() {
                var result = new int?[gridSize,gridSize];
                var finiteIndexes = new HashSet<int>(Enumerable.Range(0, coordinates.Count));
                for (var gridX = 0; gridX < gridSize; gridX++) {
                    for (var gridY = 0; gridY < gridSize; gridY++) {
                        var nearestCoordinateIndex = default(int?);
                        var nearestCoordinateDistance = int.MaxValue;
                        for (var coordinateIndex = 0; coordinateIndex < coordinates.Count; coordinateIndex++) {
                            var manhattanDistance = ManhattanDistance(gridX, coordinates[coordinateIndex][0], gridY, coordinates[coordinateIndex][1]);
                            if (manhattanDistance < nearestCoordinateDistance) {
                                nearestCoordinateIndex = coordinateIndex;
                                nearestCoordinateDistance = manhattanDistance;
                            } else if (manhattanDistance == nearestCoordinateDistance) {
                                nearestCoordinateIndex = null;
                            }
                        }
                        result[gridX, gridY] = nearestCoordinateIndex;
                        if (nearestCoordinateIndex.HasValue) {
                            var coordinateAreaExtendsToGridBoundary = new List<int> {gridX, gridY}.Intersect(new List<int> {0, gridSize - 1}).Any();
                            if (coordinateAreaExtendsToGridBoundary) {
                                finiteIndexes.Remove(nearestCoordinateIndex.Value);
                            }
                        }
                    }
                }
                return (result, finiteIndexes);
            }

            Dictionary<int, int> GetCoordinateAreas(HashSet<int> coordinateIndexes) {
                var result = new Dictionary<int, int>();
                foreach (var c in grid) {
                    if (c.HasValue && coordinateIndexes.Contains(c.Value)) {
                        result[c.Value] = (result.ContainsKey(c.Value) ? result[c.Value] : 0) + 1;
                    }
                }
                return result;
            }

            int GetRegionArea() {
                var result = 0;
                for (var gridX = 0; gridX < gridSize; gridX++) {
                    for (var gridY = 0; gridY < gridSize; gridY++) {
                        var totalDistance = 0;
                        foreach (var coordinate in coordinates) {
                            totalDistance += ManhattanDistance(gridX, coordinate[0], gridY, coordinate[1]);
                        }
                        if (totalDistance < 10000) {
                            result++;
                        }
                    }
                }
                return result;
            }
        }
    }
}