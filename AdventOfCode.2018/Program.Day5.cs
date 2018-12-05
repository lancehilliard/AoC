using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace AdventOfCode._2018 {
    static partial class Program {
        static void Day5() {
            var input = File.ReadLines("inputs/Day5Input.txt").First();

            var startingLength = input.Length;
            var startingIndex = input.Length-1;
            var stable = false;
            while (!stable) {
                var chars = input.ToCharArray();
                int i;
                for (i = startingIndex; i > 0; i--) {
                    var thisChar = chars[i];
                    var previousCharacter = chars[i-1];
                    var reacts = string.Equals($"{thisChar}".ToLower(), $"{previousCharacter}".ToLower(), StringComparison.InvariantCultureIgnoreCase) && thisChar!=previousCharacter;
                    if (reacts) {
                        //Console.WriteLine("reacts!");
                        input = input.Remove(i - 1, 2);
                        startingIndex = i-1;
                        break;
                    }
                }

                //Console.WriteLine(i);
                if (i == 0) {
                    stable = true;
                }
            }

            Report(input.Length.ToString());


        }
    }
}