using AzureBot.Model;
using AzureBot.Services.Impl;
using AzureBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace AzureBot.Services.Impl
{
    public class EnGBChatService : IChatService
    {
        private CultureInfo _culture;

        public EnGBChatService()
        {
            _culture = new CultureInfo("en-GB");
        }

        public string PromptUserLogin(User user)
        {
            var loginUri = new Uri($"http://localhost:3978/api/auth/home?UserId={user.Id}");
            var greeting = GreetUser(user.Name);
            greeting = greeting.TrimEnd('.'); // Trim the trailing period
            return String.Format(_culture, "{0}, Please login to your Azure account at {1}.", greeting, loginUri.ToString());
        }

        public string NoIdProvided()
        {
            return String.Format(_culture, "Sorry, you haven't provided an ID. Please restart the conversation.");
        }

        public string GreetUser(string username)
        {
            return String.Format(_culture, "Hello {0}.", username);
        }

        public string InvalidMessage()
        {
            return String.Format(_culture, "Sorry, your message doesn't make sense.");
        }

        public string UnsupportedIntent()
        {
            return String.Format(_culture, "Sorry, I don't understand your message.");
        }

        public string RenderResourceList(List<Resource> resources)
        {
            StringBuilder response = new StringBuilder();

            foreach (var res in resources)
            {
                response.AppendLine($"**Name:** {res.Name}");
                response.AppendLine("               ");
                response.AppendLine($"**ResourceId:** {res.Id}");
                response.AppendLine("               ");
                response.AppendLine($"**ResourceType:** {res.Type}");
                response.AppendLine("               ");
                response.AppendLine($"**Location:** {res.Location}");
                response.AppendLine("               ");
                response.AppendLine($"**SubscriptionId:** {res.SubscriptionId}");
                response.AppendLine("               ");
                response.AppendLine("-_-_-_-_-_");
                response.AppendLine("               ");
            }
            return response.ToString();
        }

        public string RenderSubscriptionList(IDictionary<string, string> subscriptions)
        {
            StringBuilder response = new StringBuilder();

            foreach (var sub in subscriptions)
            {
                response.AppendLine($"**{sub.Key}:**  + {sub.Value}");
                response.AppendLine("               ");
            }
            return response.ToString();
        }
    }
}