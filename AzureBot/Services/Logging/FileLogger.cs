using AzureBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace AzureBot.Services.Logging
{
    public class FileLogger : IStringLogger
    {
        private string _logFilePath;

        public FileLogger(string filePath)
        {
            _logFilePath = filePath;
        }

        public void LogString(string stringLogMessage)
        {
            if (File.Exists(_logFilePath))
            {
                stringLogMessage += Environment.NewLine;
                File.AppendAllText(_logFilePath, stringLogMessage);
            }
            else
            {
                using (FileStream fs = File.Create(_logFilePath))
                {
                    stringLogMessage += Environment.NewLine;
                    Byte[] log = new UTF8Encoding(true).GetBytes(stringLogMessage);
                    fs.Write(log, 0, log.Length);
                }
            }
        }
    }
}