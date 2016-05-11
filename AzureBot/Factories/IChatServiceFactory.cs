using AzureBot.Services.Impl;
using AzureBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureBot.Services
{
    public class IChatServiceFactory
    {
        public static IChatService Create(string language)
        {
            switch (language)
            {
                case "en":
                    return new EnGBChatService();
                default:
                    return new EnGBChatService();
            }
        }
    }
}