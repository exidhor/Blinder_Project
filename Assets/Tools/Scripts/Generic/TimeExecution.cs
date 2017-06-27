using System;

namespace ATools
{
    /// <summary>
    /// gather exeuction time watcher
    /// </summary>
    public static class TimeExecution
    {
        /// <summary>
        /// calculate the execution time of a function
        /// </summary>
        /// <param name="methodToMeasure">a void method without parameters</param>
        /// <returns></returns>
        /// <source>https://stackoverflow.com/questions/14019510/calculate-the-execution-time-of-a-method</source>
        public static float MeasureFunction(Action methodToMeasure)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew(); // start timeWatch

            methodToMeasure(); // execute the function

            watch.Stop(); // stop the timer

            return watch.ElapsedMilliseconds;
        }
    }
}
