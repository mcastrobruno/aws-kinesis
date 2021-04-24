using awskinesis.shared.Logger;
using System;

namespace awskinesis.shared.logger
{
    public static class Logger
    {
        public static void LogError(string message, Exception ex = null)
        {
            var defaultColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;

            Log($"{message}", LogLevel.Error);
            if (ex != null)
            {
                Log(ex.ToString(), LogLevel.Error);
            }

            Console.ForegroundColor = defaultColor;
        }
        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            Console.WriteLine($"{DateTime.Now:o} {level}: {message}");
        }
    }

   
}
