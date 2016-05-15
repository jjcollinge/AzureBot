using AzureBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace AzureBot.UnitTests.Mocks
{
    class MockValidationService : IValidationService
    {
        public Task<bool> IsValidMessage(Message message)
        {
            return Task.FromResult(true);
        }
    }
}
