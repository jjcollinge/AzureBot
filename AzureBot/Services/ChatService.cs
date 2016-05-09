using AzureBot.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace AzureBot.Services
{
    public class ChatService
    {
        private CultureInfo _culture;

        public ChatService(CultureInfo culture)
        {
            _culture = culture;
        }

        public string PromptUserLogin(User user)
        {
            var loginUri = new Uri($"http://localhost:3978/api/auth/home?UserId={user.Id}");
            return String.Format(_culture, "{0}, Please login to your Azure account at {1}.", GreetUser(user.Name),loginUri.ToString());
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
    }
}