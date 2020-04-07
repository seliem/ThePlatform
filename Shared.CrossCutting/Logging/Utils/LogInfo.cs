using System;

namespace Shared.CrossCutting.Logging
{
    internal class LogInfo
    {
        public int Index { get; set; }
        public DateTime LogeDate { get; set; }
        public TimeSpan TotalTime { get; set; }
        public string Message { get; set; }

    }
}
