namespace BRIX.Mobile.Services
{
    public static class Logger
    {
        private static readonly string _logFile = "log.txt";

        public static void LogError(Exception ex)
        {
            string date = DateTime.Now.ToString("MM/dd/yy HH:mm");
            string message = $"{date}:: {ex.GetType()}: {ex.Message}{Environment.NewLine}";

            if(ex.StackTrace != null)
            {
                message += $"{ex.StackTrace}{Environment.NewLine}";
            }

            LocalStorage localStorage = new ();

            if (localStorage.FileExists(_logFile))
            {
                string existingLog = localStorage.ReadAllText(_logFile);
                localStorage.WriteAllText(_logFile, message + existingLog);
            }
            else
            {
                localStorage.WriteAllText(_logFile, message);
            }
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
    }
}
