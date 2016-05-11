using AzureBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureBot.UnitTests.Mocks
{
    class MockAuthenticationService : IAuthenticationService
    {
        public Uri GetAuthenticationUri(string userId)
        {
            return null;
        }

        public Task<string> GetToken(object[] args)
        {
            return null;
        }
    }
}
