using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018 {
    static partial class Program {
        private class ShiftEvent {
            public ShiftEvent(DateTime @when, string what) {
                When = when;
                What = what;
            }
            public DateTime When { get; private set; }
            public string What { get; private set; }
        }
    }
}