using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.CrossCutting.Logging
{
   public interface IFileLogger
    {
         void LogToFile(LogType level, string message, Exception exception = null);
        void Log(string id, string message, bool isLastLog = false, string methodName = "");
        void Log(string id, string message,Exception exc, bool isLastLog = false, string methodName = "");
        string LogToElmah(Exception ex, string language = "ar");
    }
}
