using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace AdventOfCode._2018 {
    static partial class Program {
        static void Day4() {
            var inputs = File.ReadLines("inputs/Day4Input.txt");
            var shiftEvents = new List<ShiftEvent>();
            foreach (var input in inputs) {
                var dateTime = DateTime.Parse(input.Substring(1, 16));
                var description = input.Substring(19);
                shiftEvents.Add(new ShiftEvent(dateTime, description));
            }

            shiftEvents = shiftEvents.OrderBy(x => x.When).ToList();
            var hours = new Dictionary<int, Dictionary<int,List<DateTime>>>();
            var gId = default(int);
            foreach (var shiftEvent in shiftEvents) {
                var when = shiftEvent.When;
                var what = shiftEvent.What;
                if (what.StartsWith("G")) {//uard #
                    var indexOf = what.IndexOf(' ', 8) - 7;
                    var substring = what.Substring(7, indexOf);
                    gId = Convert.ToInt32(substring);
                    if (!hours.ContainsKey(gId)) {
                        hours[gId] = new Dictionary<int, List<DateTime>>();
                        for (int i = 0; i < 60; i++) {
                            hours[gId][i] = new List<DateTime>();
                        }
                    }

                    var firstMinuteOnShift = when.Hour==0?when.Minute:0;
                    var dateTime = (when.Hour == 0 ? when : when.AddDays(1)).Date;
                    for (int i = firstMinuteOnShift; i < 60; i++) {
                        var asleepDateTimesForMinute = hours[gId][i];
                        asleepDateTimesForMinute.Remove(dateTime);
                    }
                } else {
                    for (int i = when.Minute; i < 60; i++) {
                        var asleepDateTimesForMinute = hours[gId][i];
                        if (what.StartsWith("w")) {
                            asleepDateTimesForMinute.Remove(when.Date);
                        }
                        else {
                            asleepDateTimesForMinute.Add(when.Date);
                        }
                    }
                }
            }

            var guardIdWithMostAsleepMinutes = -1;
            var mostAsleepMinutes = -1;
            foreach (var hour in hours) {
                Dictionary<int, List<DateTime>> guardRecord = hour.Value;
                var minutes = guardRecord.Values;
                var totalDaysAsleepAcrossAllMinutes = minutes.Aggregate(0, (a, b) => a + b.Count);
                if (totalDaysAsleepAcrossAllMinutes >= mostAsleepMinutes) {
                    mostAsleepMinutes = totalDaysAsleepAcrossAllMinutes;
                    guardIdWithMostAsleepMinutes = hour.Key;
                }
            }

            var record = hours[guardIdWithMostAsleepMinutes];
            var minuteWithMostDaysAsleep = -1;
            var mostDaysAsleep = -1;
            foreach (var minute in record) {
                var daysAsleepForThisMinute = minute.Value.Count;
                if (daysAsleepForThisMinute > mostDaysAsleep) {
                    mostDaysAsleep = daysAsleepForThisMinute;
                    minuteWithMostDaysAsleep = minute.Key;
                }
            }
            var answer = guardIdWithMostAsleepMinutes * minuteWithMostDaysAsleep;
            Report($"Answer: {answer}");

            var guardIdWithMostFrequentSleepMinute = -1;
            var highestFrequency = -1;
            var chosenMinute = -1;
            foreach (var hour in hours) {
                Dictionary<int, List<DateTime>> guardRecord = hour.Value;
                KeyValuePair<int, List<DateTime>> minuteWithHighestFrequency = guardRecord.OrderByDescending(x => x.Value.Count).First();
                
                if (minuteWithHighestFrequency.Value.Count >= highestFrequency) {
                    highestFrequency = minuteWithHighestFrequency.Value.Count;
                    guardIdWithMostFrequentSleepMinute = hour.Key;
                    chosenMinute = minuteWithHighestFrequency.Key;
                }
            }
            Report($"Answer 2: {guardIdWithMostFrequentSleepMinute*chosenMinute}");

        }
    }
}