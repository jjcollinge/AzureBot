using AzureBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureBot.Services.Impl
{
    public class ConsoleLogger : ILogger
    {
        public int LogLevel { get; set; } = 1;

        //TODO: Add culture aware formatting
        private string CurrentTime { get { return DateTime.Now.ToString(); } }

        public void LogError(string logMessage)
        {
            if (LogLevel >= 3)
                Console.WriteLine($"[{CurrentTime}] ERROR: {logMessage}");
        }

        public void LogWarning(string logMessage)
        {
            if (LogLevel >= 2)
                Console.WriteLine($"[{CurrentTime}] WARNING: {logMessage}");
        }

        public void LogInfo(string logMessage)
        {
            if (LogLevel >= 1)
                Console.WriteLine($"[{CurrentTime}] INFO: {logMessage}");
        }

        public void LogVerbose(string logMessage)
        {
            if (LogLevel >= 0)
                Console.WriteLine($"[{CurrentTime}] VERBOSE: {logMessage}");
        }
    }
}