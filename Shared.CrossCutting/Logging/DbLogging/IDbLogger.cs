namespace Shared.CrossCutting.Logging
{
    public interface IDbLogger
    {
        void Log(string userId, string processName, string stepName,
            string message, object model, string StatusCode = "ok", bool IsExceptions = false, bool isLastLog = false);

        void LogAndCommit(string userId, string processName, string stepName, string message, object model,
            string StatusCode = "ok", bool IsExceptions = false);
    }
}
