using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Shared.CrossCutting.Logging.LogContext;

namespace Shared.CrossCutting.Logging
{
    /// <summary>
    /// Please add connection string named LogDbContext in web config
    /// </summary>
    public class DbLogger : IDbLogger
    {
        private static Dictionary<Guid, List<EventLog>> logs = new Dictionary<Guid, List<EventLog>>();
        private readonly LogDbContext _dbContext;
        private Guid logKey;

        public DbLogger(LogDbContext context)
        {
            _dbContext = context;
            logKey = Guid.NewGuid();
        }

      
        /// <summary>
        /// log to memory
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="processName"></param>
        /// <param name="stepName"></param>
        /// <param name="message"></param>
        /// <param name="model"></param>
        /// <param name="StatusCode"></param>
        /// <param name="IsExceptions"></param>
        public void Log(string userId, string processName, string stepName,
            string message, object model, string StatusCode = "ok", bool IsExceptions = false,bool isLastLog=false)
        {
            try
            {
                EventLog LogInfo = new EventLog();
                int index = 1;
                LogInfo.LogKey = logKey;//Guid.Parse(logKey);
                LogInfo.LogTime = DateTime.Now;
                LogInfo.Message = message;
                LogInfo.ProcessName = processName;
                LogInfo.StepName = stepName;
                LogInfo.Message = message;
                LogInfo.Model = IsExceptions == false ? JsonConvert.SerializeObject(model) : null;
                LogInfo.LogExceptions = IsExceptions == true ? JsonConvert.SerializeObject(model) : null;
                LogInfo.UserId = userId;
                LogInfo.StatusCode = StatusCode;

                if (logs.Keys.Any(key => key==logKey))
                {
                    index = logs[logKey].Max(x => x.Index);
                    index++;
                    LogInfo.Index = index;
                    logs[logKey].Add(LogInfo);
                }
                else
                {
                    LogInfo.Index = index;
                    logs.Add(logKey, new List<EventLog>() { LogInfo });
                }

                if (isLastLog)
                {
                    CommitLogs();
                }
            }
            catch (Exception ex)
            {


            }


        }


        /// <summary>
        /// log and save to database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="processName"></param>
        /// <param name="stepName"></param>
        /// <param name="message"></param>
        /// <param name="model"></param>
        /// <param name="StatusCode"></param>
        /// <param name="IsExceptions"></param>
        public void LogAndCommit(string userId, string processName, string stepName, string message, object model,
            string StatusCode = "ok", bool IsExceptions = false)
        {
            try
            {
                EventLog LogInfo = new EventLog();
                int index = 1;
                LogInfo.LogKey = logKey;
                LogInfo.LogTime = DateTime.Now;
                LogInfo.Message = message;
                LogInfo.ProcessName = processName;
                LogInfo.StepName = stepName;
                LogInfo.UserId = message;
                LogInfo.Model = IsExceptions == false ? JsonConvert.SerializeObject(model) : null;
                LogInfo.LogExceptions = IsExceptions == true ? JsonConvert.SerializeObject(model) : null;
                LogInfo.UserId = userId;
                LogInfo.StatusCode = StatusCode;

                if (logs.Keys.Any(key => key==logKey))
                {
                    index = logs[logKey].Max(x => x.Index);
                    index++;
                    LogInfo.Index = index;
                    logs[logKey].Add(LogInfo);
                }
                else
                {
                    LogInfo.Index = index;
                    logs.Add(logKey, new List<EventLog>() { LogInfo });
                }

                CommitLogs();
            }
            catch (Exception ex)
            {


            }
        }

        
        /// <summary>
        /// save log from memmory to database
        /// </summary>
        private void CommitLogs()
        {
          
            string clientAddress = HttpContext.Current.Request.UserHostAddress;
            if (logs.Keys.Any(key => key==logKey))
            {
                if (logs[logKey].Any())
                {

                    int counter = 1;
                    DateTime d = logs[logKey].FirstOrDefault(x => x.Index == 1).LogTime;
                    logs[logKey].ForEach(x =>
                    {

                        string t = getTimeSpan(d, x.LogTime);
                        d = x.LogTime;
                        x.TakeTime = t;
                        _dbContext.Logs.Add(x);
                        counter++;
                    });

                    string totalTime = getTimeSpan(logs[logKey].Min(x => x.LogTime), logs[logKey].Max(x => x.LogTime));


                    try
                    {
                        var result = _dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                       // Log(LogType.Error, "", ex);
                    }
                }
                logs.Remove(logKey);
            }
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
    public enum LogType
    {
        Error, Info, Warning, Trace,Exceptions
    }
}
