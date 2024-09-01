using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
namespace DebugHelper
{
    public static class DebugHelper
    {
        public static void DebugHowLongActionTake(Action action, out float elapsedTimeInMs)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            action?.Invoke();
            sw.Stop();
            elapsedTimeInMs = sw.ElapsedMilliseconds;
        }
    }
}
