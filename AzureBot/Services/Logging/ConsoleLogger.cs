using AzureBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureBot.Services.Impl
{
    public class ConsoleLogger : IStringLogger
    {
        public void LogString(string stringLogMessage)
        {
            Console.WriteLine(stringLogMessage);
        }
    }
}