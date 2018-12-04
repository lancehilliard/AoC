using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018 {
    static partial class Program {
        private class FabricClaim {
            private readonly int _width;
            private readonly int _height;

            public FabricClaim(int id, int firstColumn, int firstRow, int width, int height) {
                Id = id;
                FirstColumn = firstColumn;
                FirstRow = firstRow;
                _width = width;
                _height = height;
            }
            public int Id { get; set; }
            public int FirstColumn { get; set; }
            public int FirstRow { get; set; }

            public int LastRow => FirstRow + _height - 1;
            public int LastColumn => FirstColumn + _width - 1;

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
    }
}