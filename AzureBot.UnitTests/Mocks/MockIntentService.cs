using AzureBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureBot.UnitTests.Mocks
{
    class MockIntentService : IIntentService
    {
        public Task<string> GetIntentAsync(string inputText)
        {
            switch(inputText)
            {
                case "subscriptions":
                    return Task.FromResult("GetSubscriptions");
                case "resources":
                    return Task.FromResult("GetResources");
                case "resource_groups":
                    return Task.FromResult("GetResourceGroups");
                default:
                    return Task.FromResult("Unsupported intent");
            }
        }
    }
}
