using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Shared.CrossCutting.Logging
{
    public class FileLogger : IFileLogger
    {
        private static Dictionary<string, List<LogInfo>> FileDictionary = new Dictionary<string, List<LogInfo>>();

        private static readonly NLog.Logger logman = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// log to file direct
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void LogToFile(LogType level, string message, Exception exception = null)
        {
            string clientAddress = "";
            try { clientAddress = System.Web.HttpContext.Current.Request.UserHostAddress; } catch { }
            var enableLog = ConfigurationManager.AppSettings["enableLog"].ToString();
            if (enableLog == "true")
            {
                switch (level)
                {
                   
                    case LogType.Info:
                        logman.Log(LogLevel.Info, message, exception);
                        return;
                        
                    case LogType.Warning:
                        logman.Log(LogLevel.Warn, message, exception);
                        return;
                        
                    case LogType.Trace:
                        logman.Log(LogLevel.Trace, message, exception);
                        return;
                        
                    
                }
                if (exception?.InnerException != null)
                {
                    string m = Environment.NewLine + " ----- Exception  start " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.FFF") + " ," + clientAddress + " -----" + Environment.NewLine;
                    m = m + Environment.NewLine + " Message:" + message + Environment.NewLine;
                    try { m = m + Environment.NewLine + " Exception Type:" + exception.InnerException.GetType().Name + Environment.NewLine; } catch { }
                    m = m + Environment.NewLine + " Exception  Source:" + exception.Source + Environment.NewLine;
                    m = m + Environment.NewLine + " Exception Message: " + exception.Message + Environment.NewLine;

                    m = m + Environment.NewLine + " ----- Exception  End " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.FFF") + " ----- " + Environment.NewLine;
                    logman.Log(LogLevel.Error, m, exception);

                }
            }
        }

        /// <summary>
        /// commit logs from memory to file
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="MethodName"></param>
        private void CommitLogs(string Id, string MethodName)
        {
            string LogManConfig = ConfigurationManager.AppSettings["enableLog"].ToString();
            if (LogManConfig == "true")
            {
                string clientAddress = "";
                try { clientAddress = System.Web.HttpContext.Current.Request.UserHostAddress; } catch { }
                try
                {
                    if (FileDictionary.Keys.Any(key => key.Contains(Id)))
                    {
                        if (FileDictionary[Id].Any())
                        {
                            string Message = Environment.NewLine + " --------------------- Start (  " + MethodName + ", Key:" + Id + " ,Date:" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.FFF") + " ," + clientAddress + " ) ----------------------" + Environment.NewLine;
                            int counter = 1;
                            DateTime d = FileDictionary[Id].FirstOrDefault(x => x.Index == 1).LogeDate;
                            FileDictionary[Id].ForEach(x =>
                            {

                                string t = getTimeSpan(d, x.LogeDate);
                                d = x.LogeDate;
                                Message = Message + " " + counter.ToString() + "   Time(" + t + ")    - " + x.Message + " " + Environment.NewLine + "--" + Environment.NewLine;
                                counter++;
                            });

                            string totalTime = getTimeSpan(FileDictionary[Id].Min(x => x.LogeDate), FileDictionary[Id].Max(x => x.LogeDate));
                            Message = Message + Environment.NewLine + " --------------------- end (  " + MethodName + ", Key:" + Id + ",(Api total time(" + totalTime + ") ,Date:" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.FFF") + " ," + clientAddress + " ) ----------------------" + Environment.NewLine;

                            try { logman.Log(LogLevel.Debug, Message); } catch { }
                        }
                        FileDictionary.Remove(Id);
                    }

                }
                catch (Exception ex)
                {

                   // _IFileLogger.LogToElmah(ex);
                }
            }

        }

        /// <summary>
        /// add log to memory
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        public void Log(string id, string message,bool isLastLog=false,string methodName="")
        {
            try
            {
                LogInfo o = new LogInfo();
                int index = 1;
                o.LogeDate = DateTime.Now;
                o.Message = DateTime.Now.ToString("HH:mm:ss.FFF") + "   " + message;
                if (FileDictionary.Keys.Any(key => key.Contains(id)))
                {
                    index = FileDictionary[id].Max(x => x.Index);
                    index++;
                    o.Index = index;
                    FileDictionary[id].Add(o);
                }
                else
                {
                    o.Index = index;
                    FileDictionary.Add(id, new List<LogInfo>() { o });
                }
                if (isLastLog)
                {
                    CommitLogs(id, methodName);
                }
            }
            catch (Exception ex) { }

        }


        public void Log(string id, string message,Exception exc, bool isLastLog = false, string methodName = "")
        {
            try
            {
                LogInfo o = new LogInfo();
                int index = 1;
                o.LogeDate = DateTime.Now;
                o.Message = DateTime.Now.ToString("HH:mm:ss.FFF") + "   " + message;
                if (FileDictionary.Keys.Any(key => key.Contains(id)))
                {
                    index = FileDictionary[id].Max(x => x.Index);
                    index++;
                    o.Index = index;
                    FileDictionary[id].Add(o);
                }
                else
                {
                    o.Index = index;
                    FileDictionary.Add(id, new List<LogInfo>() { o });
                }
                if (isLastLog)
                {
                    CommitLogs(id, methodName);
                }
            }
            catch (Exception ex) { }

        }

        /// <summary>
        /// log exception to Elmah
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public string LogToElmah(Exception ex, string language = "ar")
        {
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            // ravenClient.Capture(new SharpRaven.Data.SentryEvent(ex));
            return ExceptionMessage.GetErrorMessage(ex, language);
        }

        private string getTimeSpan(DateTime Date1, DateTime Date2)
        {
            TimeSpan t;
            try
            {
                long delay = Convert.ToInt64((Date2 - Date1).TotalMilliseconds);
                delay = delay * 10000;
                t = new TimeSpan(delay);
                return t.Hours.ToString() + ":" + t.Minutes.ToString() + ":" + t.Seconds.ToString() + "." + t.Milliseconds.ToString();
            }
            catch (Exception ex) { }
            return "";
        }
    }
}
