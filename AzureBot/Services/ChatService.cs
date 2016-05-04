using AzureBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureBot.Services
{
    public class ChatService
    {
        // TODO: Inject cultural details

        public string PromptUserLogin(User user)
        {
            var loginUri = new Uri($"http://localhost:3978/api/auth/home?UserId={user.Id}");
            return $@"Please login to your Azure account at {loginUri.ToString()}";
        }

        public string GreetUser(string username)
        {
            return $"Hello {username}";
        }

        public string InvalidMessage()
        {
            return $"Sorry, I don't understand your message.";
        }
    }
}