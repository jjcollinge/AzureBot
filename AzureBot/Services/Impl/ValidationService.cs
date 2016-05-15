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
        private int MIN_MESSAGE_LENGTH;
        private int MAX_MESSAGE_LENGTH;

        public ValidationService()
        {
            MIN_MESSAGE_LENGTH = 1;
            MAX_MESSAGE_LENGTH = 500;
        }

        public ValidationService(int minMessageTextSize, int maxMessageTextSize)
        {
            MIN_MESSAGE_LENGTH = minMessageTextSize;
            MAX_MESSAGE_LENGTH = maxMessageTextSize;
        }

        public Task<bool> IsValidMessage(Message message)
        {
            var messageText = message.Text;
            var isValid = true;

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