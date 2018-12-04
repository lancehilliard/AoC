using System;
using System.Runtime.CompilerServices;

namespace AdventOfCode._2018 {
    static partial class Program {
        static void Report(string x, [CallerMemberName] string callerMemberName = null) {
            var elapsed = Timer.Elapsed;
            Console.WriteLine($"[{(_lastElapsedReported.Milliseconds.Equals(0) ? " " : "+")}{elapsed-_lastElapsedReported}] {callerMemberName}: {x}");
            _lastElapsedReported = elapsed;
        }
    }
}