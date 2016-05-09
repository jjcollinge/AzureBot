﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Newtonsoft.Json;
using System.Text;
using System.Security;
using AzureBot.Model;
using AzureBot.Services;

namespace AzureBot.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private const int MIN_MESSAGE_LENGTH = 1;
        private ChatService _chat;
        private AzureService _azure;

        public MessagesController()
        {
            /*
             * Ctor must remain parameterless for BotFramework
             */

            _chat = new ChatService(new System.Globalization.CultureInfo("en-GB"));
            _azure = new AzureService("2015-01-01");
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                var id = message?.From?.Id;
                
                // Check message has sender Id
                if (string.IsNullOrEmpty(id))
                    return message.CreateReplyMessage(_chat.NoIdProvided());

                User user = AzureBot.Model.User.GetOrCreate(id);

                // If user info isn't initialised, do it now
                if(string.IsNullOrEmpty(user.Name))
                    user.Name = message.From.Name;

                // Hand the message of for processing
                var response = await HandleUserMessage(message, user);

                // Return the response
                return message.CreateReplyMessage(response);
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private async Task<string> HandleUserMessage(Message message, User user)
        {
            StringBuilder response = new StringBuilder();

            // Check if user has logged into Azure
            if (String.IsNullOrEmpty(user.Token))
            {
                response.AppendLine(_chat.PromptUserLogin(user));
            }
            else
            {
                // User is logged in...
                if (!ValidateMessage(message))
                {
                    response.AppendLine(_chat.InvalidMessage());
                }
                else
                {
                    // Implement all other chat logic here...
                    string inputText = Uri.EscapeDataString(message.Text);

                    IntentRoot intentRoot = await GetIntent(inputText);

                    switch (intentRoot.intent)
                    {
                        case "GetSubscriptions":
                            foreach (var sub in await _azure.GetSubscriptions(user.Token))
                            {
                                response.AppendLine($"**{sub.Key}:**  + {sub.Value}");
                                response.AppendLine("               ");
                            }
                            break;
                        case "GetResources":
                            foreach (var res in await _azure.GetResources(user.Token))
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
                            break;
                        default:
                            response.AppendLine(_chat.UnsupportedIntent());
                            break;
                    }
                }
            }

            return response.ToString();
        }

        private async Task<IntentRoot> GetIntent(string inputText)
        {
            IntentRoot intent = new IntentRoot();

            using (var http = new HttpClient())
            {
                string key = Environment.GetEnvironmentVariable("AZUREBOT_LUIS_API_KEY");
                string id = Environment.GetEnvironmentVariable("AZUREBOT_LUIS_API_ID");
                string uri = $"https://api.projectoxford.ai/luis/v1/application?id={id}&subscription-key={key}&q={inputText}";
                var res = await http.GetAsync(uri);
                res.EnsureSuccessStatusCode();

                var strRes = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(strRes);

                // Get top intent
                intent.intent = data.intents[0].intent;
                intent.score = data.intents[0].score;
                intent.actions = data.intents[0].actions;
            }

            return intent;
        }

        private static bool ValidateMessage(Message message)
        {
            return true;
        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == "BotAddedToConversation")
            {
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
            }
            else if (message.Type == "UserAddedToConversation")
            {
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
            }
            else if (message.Type == "EndOfConversation")
            {
            }

            return null;
        }
    }
}