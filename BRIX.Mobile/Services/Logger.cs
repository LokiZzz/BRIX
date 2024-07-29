using Newtonsoft.Json;

namespace BRIX.Mobile.Services
{
    public static class Logger
    {
        private static readonly string _logFile = "log.txt";

        public static void LogError(Exception ex)
        {
            string date = DateTime.Now.ToString("MM/dd/yy HH:mm:ss");
            string message = $"{date} :: {ex.GetType()}: {ex.Message}{Environment.NewLine}";

            if (ex.StackTrace != null)
            {
                message += $"{ex.StackTrace}{Environment.NewLine}";
            }

            WriteLogMessage(message);
        }

        public static void LogInfo(string messageText, object? objectToLog = null)
        {
            string date = DateTime.Now.ToString("MM/dd/yy HH:mm:ss");
            string message = $"{date} :: {messageText}{Environment.NewLine}";

            if (objectToLog != null)
            {
                message += JsonConvert.SerializeObject(objectToLog) + Environment.NewLine;
            }

            WriteLogMessage(message);
        }

        public static string GetLog()
        {
            ILocalStorage localStorage = Resolver.Resolve<ILocalStorage>();

            return localStorage.ReadText(_logFile);
        }

        public static void ClearLog()
        {
            ILocalStorage localStorage = Resolver.Resolve<ILocalStorage>();
            localStorage.WriteText(_logFile, string.Empty);
        }

        private static void WriteLogMessage(string message)
        {
            ILocalStorage localStorage = Resolver.Resolve<ILocalStorage>();
            string existingLog = localStorage.ReadText(_logFile);
            localStorage.WriteText(_logFile, message + Environment.NewLine + existingLog);
        }
    }
}
