using System;
using System.Diagnostics;

namespace Common
{
    /// <summary>
    /// 计算执行时间帮助类
    /// </summary>
    public sealed class StopwatchHelper
    {
        public static void ReportExecutedTime(Action action)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            action.Invoke();
            sw.Stop();
            string methodName = action.Method.Name;
            Console.WriteLine($"{methodName} 执行耗时：{sw.ElapsedMilliseconds} ms");
        }
    }
}
