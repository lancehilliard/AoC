using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2018 {
    static partial class Program {
        static void Day6() {
            var inputs = File.ReadLines("inputs/Day6Input.txt");
            var coordsSortedByX = inputs.Select(input => input.Split(", ")).Select(strings => strings.Select(x => Convert.ToInt32(x)).ToArray()).OrderBy(x => x[0]).ToList();
            var maxX = coordsSortedByX.Max(x=>x[0]);
            var maxY = coordsSortedByX.Max(x=>x[1]);
            Console.WriteLine(maxX.ToString());//356
            Console.WriteLine(maxY.ToString());//357
            var gridWidth = 700;
            var gridHeight = 700;
            var grid = new int?[gridWidth,gridHeight];
            for (int gridX = 0; gridX < gridWidth; gridX++) {
                for (int gridY = 0; gridY < gridHeight; gridY++) {
                    var coordinateIndexWithShortestManhattanDistance = default(int?);
                    var shortestManhattanDistance = int.MaxValue;
                    for (int coordinateIndex = 0; coordinateIndex < coordsSortedByX.Count; coordinateIndex++) {
                        var coord = coordsSortedByX[coordinateIndex];
                        var coordinateX = coord[0];
                        var coordinateY = coord[1];
                        var manhattanDistance = ManhattanDistance(gridX, coordinateX, gridY, coordinateY);
                        if (manhattanDistance < shortestManhattanDistance) {
                            coordinateIndexWithShortestManhattanDistance = coordinateIndex;
                            shortestManhattanDistance = manhattanDistance;
                        } else if (manhattanDistance == shortestManhattanDistance) {
                            coordinateIndexWithShortestManhattanDistance = null;
                        }
                    }
                    if (coordinateIndexWithShortestManhattanDistance.HasValue) {
                        grid[gridX, gridY] = coordinateIndexWithShortestManhattanDistance;
                    }
                }
            }

            var infiniteCoordinateIndexes = new List<int>();
            for (int gridYIndex = 0; gridYIndex < gridHeight-1; gridYIndex++) {
                var infiniteCoordinate = grid[0, gridYIndex];
                if (infiniteCoordinate.HasValue) {
                    infiniteCoordinateIndexes.Add(infiniteCoordinate.Value);
                }
            }
            for (int gridYIndex = 0; gridYIndex < gridHeight-1; gridYIndex++) {
                var infiniteCoordinate = grid[gridHeight-1, gridYIndex];
                if (infiniteCoordinate.HasValue) {
                    infiniteCoordinateIndexes.Add(infiniteCoordinate.Value);
                }
            }
            for (int gridXIndex = 1; gridXIndex < gridWidth - 2; gridXIndex++) {
                var infiniteCoordinate = grid[gridXIndex, 0];
                if (infiniteCoordinate.HasValue) {
                    infiniteCoordinateIndexes.Add(infiniteCoordinate.Value);
                }
            }
            for (int gridXIndex = 1; gridXIndex < gridWidth - 2; gridXIndex++) {
                var infiniteCoordinate = grid[gridXIndex, gridHeight-1];
                if (infiniteCoordinate.HasValue) {
                    infiniteCoordinateIndexes.Add(infiniteCoordinate.Value);
                }
            }

            var finiteCoordinateAreas = new Dictionary<int, int>();
            foreach (var coordinateIndex in grid) {
                if (coordinateIndex.HasValue && !infiniteCoordinateIndexes.Contains(coordinateIndex.Value)) {
                    if (!finiteCoordinateAreas.ContainsKey(coordinateIndex.Value)) {
                        finiteCoordinateAreas[coordinateIndex.Value] = 0;
                    }

                    finiteCoordinateAreas[coordinateIndex.Value]++;
                }
            }

            var gridLocationsWithinRegion = 0;
            for (int gridX = 0; gridX < gridWidth; gridX++) {
                for (int gridY = 0; gridY < gridHeight; gridY++) {
                    var totalDistance = 0;
                    foreach (var coord in coordsSortedByX) {
                        var coordinateX = coord[0];
                        var coordinateY = coord[1];
                        totalDistance += ManhattanDistance(gridX, coordinateX, gridY, coordinateY);
                    }

                    if (totalDistance < 10000) {
                        gridLocationsWithinRegion++;
                    }
                }
            }


            var largestAreaCoordinate = finiteCoordinateAreas.OrderByDescending(x=>x.Value).First();
            var largestArea = largestAreaCoordinate.Value;
            Report($"Part 1: {largestArea}");
            Report($"Part 2: {gridLocationsWithinRegion}");
        }

        public static int ManhattanDistance(int x1, int x2, int y1, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }
    }
}