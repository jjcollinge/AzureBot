using AzureBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureBot.Services.Impl
{
    public class StringLoggerService : ILoggerService
    {
        public int LogLevel { get; set; } = 1;

        private IStringLogger _logger;

        public StringLoggerService(IStringLogger logger)
        {
            _logger = logger;
        }

        //TODO: Add culture aware formatting
        private string CurrentTime { get { return DateTime.Now.ToString(); } }

        public void LogError(string logMessage)
        {
            if (LogLevel >= 3)
                _logger.LogString($"[{CurrentTime}] ERROR: {logMessage}");
        }

        public void LogWarning(string logMessage)
        {
            if (LogLevel >= 2)
                _logger.LogString($"[{CurrentTime}] WARNING: {logMessage}");
        }

        public void LogInfo(string logMessage)
        {
            if (LogLevel >= 1)
                _logger.LogString($"[{CurrentTime}] INFO: {logMessage}");
        }

        public void LogVerbose(string logMessage)
        {
            if (LogLevel >= 0)
                _logger.LogString($"[{CurrentTime}] VERBOSE: {logMessage}");
        }
    }
}