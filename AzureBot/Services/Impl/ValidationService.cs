using AzureBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;
using System.Threading.Tasks;

namespace AzureBot.Services.Impl
{
    public class ValidationService : IValidationService
    {
        private const int MIN_MESSAGE_LENGTH = 1;
        private const int MAX_MESSAGE_LENGTH = 500;

        public Task<bool> IsValidMessage(Message message)
        {
            var messageText = message.Text;
            var isValid = false;

            if(messageText.Length >= MAX_MESSAGE_LENGTH)
            {
                isValid = false;
            }

            if (messageText.Length < MIN_MESSAGE_LENGTH)
            {
                isValid = false;
            }

            return Task.FromResult(isValid);
        }
    }
}