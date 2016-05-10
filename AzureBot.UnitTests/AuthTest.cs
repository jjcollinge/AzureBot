using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzureBot.Controllers;
using System.Net;
using AzureBot.Services;

namespace AzureBot.UnitTests
{
    [TestClass]
    public class AuthTest
    {
        [TestMethod]
        public async void TestAuthFlowSuccess()
        {
            // Test for succesful authentication
            var code = Environment.GetEnvironmentVariable("AZURE_AUTH_CODE");
            var state = Environment.GetEnvironmentVariable("AZURE_AUTH_STATE");

            //var clientInfo = new ClientInfo();
            //var authService = new MockAuthService(clientInfo);
            //var authController = new AuthController(authService);
            //var res = controller.Home("1234");
            //Assert.IsTrue(res.StatusCode == HttpStatusCode.OK);
        }

        [TestMethod]
        public async void TestAuthFlowFail()
        {
            //TODO...
        }

        [TestMethod]
        public async void TestAuthFlowNullValues()
        {
            //TODO...
        }
    }
}
