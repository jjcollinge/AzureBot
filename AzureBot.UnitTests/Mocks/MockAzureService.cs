using AzureBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureBot.UnitTests.Mocks
{
    class MockAzureService : IAzureService
    {
        private List<Resource> _resourceGroups;
        private List<Resource> _resources;
        private IDictionary<string, string> _subscriptions;

        public void LoadTestData(List<Resource> resourceGroups,
                                 List<Resource> resources,
                                 IDictionary<string, string> subscriptions)
        {
            _resourceGroups = resourceGroups;
            _resources = resources;
            _subscriptions = subscriptions;
        }

        public Task<List<Resource>> GetResourceGroups(string token)
        {
            return Task.FromResult(_resourceGroups);
        }

        public Task<List<Resource>> GetResources(string token)
        {
            return Task.FromResult(_resources);
        }

        public Task<IDictionary<string, string>> GetSubscriptions(string token)
        {
            return Task.FromResult(_subscriptions);
        }
    }
}
