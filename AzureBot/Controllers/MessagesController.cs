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
using System.Globalization;
using AzureBot.Services.Interfaces;
using AzureBot.Repos;

namespace AzureBot.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        //TODO: Add user resource caching

        private IAzureService _azure;
        private IUserRepository _users;
        private IIntentService _intentService;
        private IValidationService _validationService;

        public MessagesController(IUserRepository users,
                                  IAzureService azureService,
                                  IIntentService intentService,
                                  IValidationService validationService)
        {
            _users = users;
            _azure = azureService;
            _intentService = intentService;
            _validationService = validationService;
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            // Initialise a chat service to match the input language
            var chat = IChatServiceFactory.Create(message.Language);

            if (message.Type == "Message")
            {
                var id = message?.From?.Id;

                // Check message has sender Id
                if (string.IsNullOrEmpty(id))
                    return message.CreateReplyMessage(chat.NoIdProvided());

                // Get or create user
                User user = GetOrCreateUser(id);

                // If user info isn't initialised, do it now
                if (string.IsNullOrEmpty(user.Name))
                    user.Name = message.From.Name;

                // Return the response
                return message.CreateReplyMessage(await HandleUserMessage(chat, message, user));
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private async Task<string> HandleUserMessage(IChatService chat, Message message, User user)
        {
            StringBuilder response = new StringBuilder();

            // Check if user has logged into Azure
            if (String.IsNullOrEmpty(user.Token))
            {
                response.AppendLine(chat.PromptUserLogin(user));
            }
            else
            {
                // User is logged in...

                // Check message is valid
                if (!await _validationService.IsValidMessage(message))
                {
                    response.AppendLine(chat.InvalidMessage());
                }
                else
                {
                    // Implement all other chat logic here...
                    string inputText = Uri.EscapeDataString(message.Text);

                    string intent = await _intentService.GetIntentAsync(inputText);

                    switch (intent)
                    {
                        case "GetSubscriptions":
                            response.Append(chat.RenderSubscriptionList(await _azure.GetSubscriptions(user.Token)));
                            break;
                        case "GetResources":
                            response.Append(chat.RenderResourceList(await _azure.GetResources(user.Token)));
                            break;
                        case "GetResourceGroups":
                            response.Append(chat.RenderResourceList(await _azure.GetResourceGroups(user.Token)));
                            break;
                        default:
                            response.AppendLine(chat.UnsupportedIntent());
                            break;
                    }
                }
            }

            return response.ToString();
        }  

        private User GetOrCreateUser(string id)
        {
            User user = _users.GetById(id);

            // If the user doesn't exist
            if (user == null)
            {
                // Create new users
                user = new Model.User(id);
                _users.Add(user);
            }

            return user;
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