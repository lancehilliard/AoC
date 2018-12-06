using System;
using System.Diagnostics;

namespace AdventOfCode._2018 {
    static partial class Program {
        static readonly Stopwatch Timer = new Stopwatch();
        static TimeSpan _lastElapsedReported;
        static void Main() {
            Timer.Start();
            //Day1();
            //Day2();
            //Day3();
            //Day4();
            //Day5();
            Day6();
            Console.WriteLine($"\nFinished.");
            Console.Write($"Total Runtime: {Timer.Elapsed}");
            Console.ReadKey();
        }
    }
}