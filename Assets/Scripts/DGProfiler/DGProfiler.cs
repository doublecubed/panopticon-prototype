using System.Collections.Generic;
using DeificGames.Profiler.Internal;
using UnityEngine;

namespace DeificGames.Profiler.Internal
{
    public static class DGProfilerManager
    {
        public static Dictionary<string, Stopwatch> stopwatches = new Dictionary<string, Stopwatch>();
        public static Stopwatch currentStopwatch;

        [System.Serializable]
        public class Stopwatch
        {
            private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            public bool removeIfNotCalled;
            public double removeIfNotCalledForTime;

            public string name;

            public double runCount;
            public double totalTime;
            public double averageTime;
            public double highestTime;
            public double lowestTime = 999999;

            public double averageOverOneSecond;
            private double oneSecondTotalTime;
            private double oneSecondTotalRuns;

            public void StartTimer()
            {
                sw.Restart();
            }

            public void StopAndCacheTime()
            {
                double time = sw.Elapsed.TotalMilliseconds;
                runCount += 1;
                totalTime += time;
                averageTime = totalTime / runCount;
                highestTime = time > highestTime ? time : highestTime;
                lowestTime = time < lowestTime ? time : lowestTime;
                oneSecondTotalTime += time;
                oneSecondTotalRuns += 1;
                if (oneSecondTotalTime >= 1) {
                    averageOverOneSecond = oneSecondTotalTime / oneSecondTotalRuns;
                    oneSecondTotalRuns = 0;
                    oneSecondTotalTime = 0;
                }
                
            }

            public void Reset()
            {
                runCount = 0;
                totalTime = 0;
                averageTime = 0;
                highestTime = 0;
                lowestTime = 999999;
            }
        }
    }
}

namespace DeificGames.Profiler
{
    public static class DGProfiler
    {        
        public static void BeginScope(object callerInstance, string name)
        {
            string id = callerInstance.GetHashCode() + name;

            if (!DGProfilerManager.stopwatches.ContainsKey(id)) {               
                DGProfilerManager.currentStopwatch = new DGProfilerManager.Stopwatch();
                DGProfilerManager.currentStopwatch.name = name;
                DGProfilerManager.stopwatches.Add(id, DGProfilerManager.currentStopwatch);
                
            }

            DGProfilerManager.currentStopwatch = DGProfilerManager.stopwatches[id];

            DGProfilerManager.currentStopwatch.StartTimer();            
        }

        public static void EndScope()
        {
            DGProfilerManager.currentStopwatch.StopAndCacheTime();
        }        
    }
}