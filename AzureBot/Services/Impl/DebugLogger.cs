using AzureBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace AzureBot.Services.Impl
{
    public class DebugLogger : IStringLogger
    {
        public void LogString(string stringLogMessage)
        {
            Debug.WriteLine(stringLogMessage);
        }
    }
}