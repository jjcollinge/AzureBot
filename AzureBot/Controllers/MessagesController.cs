using System;
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

        public MessagesController(ChatService chat)
        {
            _chat = chat;
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
                    return message.CreateReplyMessage("Sorry, you haven't provided an ID. Please restart the conversation.");

                User user = AzureBot.Model.User.GetOrCreate(id);

                // If user info isn't initialised, do it now
                if(string.IsNullOrEmpty(user.Name))
                    user.Name = message.From.Name;

                // Hand the message of for processing
                var response = await ProcessMessage(message, user);

                // Return the response
                return message.CreateReplyMessage(response);
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private async Task<string> ProcessMessage(Message message, User user)
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
                }
            }

            return response.ToString();
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