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
            LocalStorage localStorage = new();

            if (localStorage.FileExists(_logFile))
            {
                return localStorage.ReadAllText(_logFile);
            }
            else
            {
                return string.Empty;
            }
        }

        public static void ClearLog()
        {
            LocalStorage localStorage = new();

            if (localStorage.FileExists(_logFile))
            {
                localStorage.WriteAllText(_logFile, string.Empty);
            }
        }

        private static void WriteLogMessage(string message)
        {
            LocalStorage localStorage = new();
            string existingLog = string.Empty;

            if (localStorage.FileExists(_logFile))
            {
                existingLog = localStorage.ReadAllText(_logFile);
            }
            
            localStorage.WriteAllText(_logFile, message + Environment.NewLine + existingLog);
        }
    }
}
