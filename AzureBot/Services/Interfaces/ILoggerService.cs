using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureBot.Services.Interfaces
{
    public interface ILoggerService
    {
        void LogVerbose(string logMessage);
        void LogInfo(string logMessage);
        void LogWarning(string logMessage);
        void LogError(string logMessage);
    }
}
