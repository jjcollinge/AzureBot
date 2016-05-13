using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzureBot.Controllers;
using AzureBot.UnitTests.Mocks;
using Microsoft.Bot.Connector;
using AzureBot.Services.Impl;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AzureBot.UnitTests.Tests
{
    [TestClass]
    public class MessagesControllerTest
    {
        private MockAzureService _azureService;
        private MockIntentService _intentService;

        public MessagesControllerTest()
        {
            _intentService = new MockIntentService();
            _azureService = new MockAzureService();
            InitialiseTestData(_azureService);
        }

        [TestMethod]
        public async Task TestMessageWithNoUserId()
        {
            var userRepo = new UserRepository();
            var controller = new MessagesController(userRepo, _azureService, _intentService);

            var message = new Message()
            {
                Type = "Message",
                Language = "en"
            };

            var chatService = new EnglishChatService("en-GB");

            var responseMessage = await controller.Post(message);

            var expectedResponse = EscapeString(chatService.NoIdProvided());
            var actualResponse = EscapeString(responseMessage.Text);

            Assert.IsTrue(actualResponse == expectedResponse);
        }

        [TestMethod]
        public async Task TestNotloggedInMessage()
        {
            var userRepo = new UserRepository();
            var azureService = new MockAzureService();
            var controller = new MessagesController(userRepo, _azureService, _intentService);

            var testUserName = "TestUser";
            var testUserId = "0";

            var message = new Message()
            {
                Type = "Message",
                Language = "en",
                From = new ChannelAccount
                {
                    Id = testUserId,
                    Name = testUserName
                }
            };

            var chatService = new EnglishChatService("en-GB");

            var responseMessage = await controller.Post(message);

            var user = new Model.User("0");
            user.Name = testUserName;

            var expectedResponse = EscapeString(chatService.PromptUserLogin(user));
            var actualResponse = EscapeString(responseMessage.Text);

            Assert.IsTrue(actualResponse == expectedResponse);
        }

        [TestMethod]
        public async Task TestNonSupportedMessageIntent()
        {
            var username = "TestUser";
            var userId = "0";
            var user = new Model.User("0");
            user.Name = username;
            user.Token = "MyTestToken";

            var userRepo = new UserRepository();
            userRepo.Add(user);
            
            var controller = new MessagesController(userRepo, _azureService, _intentService);

            var message = new Message()
            {
                Type = "Message",
                Language = "en",
                Text = "This is an unsupported message intent",
                From = new ChannelAccount
                {
                    Id = userId,
                    Name = username
                }
            };

            var chatService = new EnglishChatService("en-GB");

            var responseMessage = await controller.Post(message);

            var expectedResponse = EscapeString(chatService.UnsupportedIntent());
            var actualResponse = EscapeString(responseMessage.Text);

            Assert.IsTrue(actualResponse == expectedResponse);
        }

        [TestMethod]
        public async Task TestMessageIntentGetSubscription()
        {
            var username = "TestUser";
            var userId = "0";
            var user = new Model.User("0");
            user.Name = username;
            user.Token = "MyTestToken";

            var userRepo = new UserRepository();
            userRepo.Add(user);

            var controller = new MessagesController(userRepo, _azureService, _intentService);

            var message = new Message()
            {
                Type = "Message",
                Language = "en",
                Text = "subscriptions",
                From = new ChannelAccount
                {
                    Id = userId,
                    Name = username
                }
            };

            var chatService = new EnglishChatService("en-GB");

            var responseMessage = await controller.Post(message);

            //TODO: This should test against the static data and not rely on these message calls.
            var expectedResponse = EscapeString(chatService.RenderSubscriptionList(await _azureService.GetSubscriptions("")));
            var actualResponse = EscapeString(responseMessage.Text);

            Assert.IsTrue(actualResponse == expectedResponse);
        }

        [TestMethod]
        public async Task TestMessageIntentGetResources()
        {
            var username = "TestUser";
            var userId = "0";
            var user = new Model.User("0");
            user.Name = username;
            user.Token = "MyTestToken";

            var userRepo = new UserRepository();
            userRepo.Add(user);

            var controller = new MessagesController(userRepo, _azureService, _intentService);

            var message = new Message()
            {
                Type = "Message",
                Language = "en",
                Text = "resources",
                From = new ChannelAccount
                {
                    Id = userId,
                    Name = username
                }
            };

            var chatService = new EnglishChatService("en-GB");

            var responseMessage = await controller.Post(message);

            //TODO: This should test against the static data and not rely on these message calls.
            var expectedResponse = EscapeString(chatService.RenderResourceList(await _azureService.GetResources("")));
            var actualResponse = EscapeString(responseMessage.Text);

            Assert.IsTrue(actualResponse == expectedResponse);
        }

        [TestMethod]
        public async Task TestMessageIntentGetResourceGroups()
        {
            var username = "TestUser";
            var userId = "0";
            var user = new Model.User("0");
            user.Name = username;
            user.Token = "MyTestToken";

            var userRepo = new UserRepository();
            userRepo.Add(user);

            var controller = new MessagesController(userRepo, _azureService, _intentService);

            var message = new Message()
            {
                Type = "Message",
                Language = "en",
                Text = "resource_groups",
                From = new ChannelAccount
                {
                    Id = userId,
                    Name = username
                }
            };

            var chatService = new EnglishChatService("en-GB");

            var responseMessage = await controller.Post(message);

            //TODO: This should test against the static data and not rely on these message calls.
            var expectedResponse = EscapeString(chatService.RenderResourceList(await _azureService.GetResourceGroups("")));
            var actualResponse = EscapeString(responseMessage.Text);

            Assert.IsTrue(actualResponse == expectedResponse);
        }

        private static void InitialiseTestData(MockAzureService azureService)
        {
            var resGroup = InitialiseResourceGroups();
            var res = InitialiseResources();
            var subs = InitialiseSubscriptions();

            azureService.LoadTestData(resGroup, res, subs);
        }

        private static Dictionary<string, string> InitialiseSubscriptions()
        {
            var subscriptions = new Dictionary<string, string>();

            for (int i = 0; i < 3; i++)
            {
                subscriptions.Add($"MySubName{i}", $"MySubId{i}");
            }

            return subscriptions;
        }

        private static List<Resource> InitialiseResources()
        {
            var resources = new List<Resource>();

            for (int i = 0; i < 10; i++)
            {
                resources.Add(new Resource()
                {
                    Id = i.ToString(),
                    Name = $"Resource{i}",
                    Location = "West Europe",
                    GroupName = $"MyResourceGroup{i}",
                    Type = "Resource Group",
                    SubscriptionId = "1"
                });
            }

            return resources;
        }

        private static List<Resource> InitialiseResourceGroups()
        {
            var resourceGroups = new List<Resource>();

            for (int i = 0; i < 10; i++)
            {
                resourceGroups.Add(new Resource()
                {
                    Id = i.ToString(),
                    Name = $"Resource{i}",
                    Location = "West Europe",
                    GroupName = $"MyResourceGroup{i}",
                    Type = "Resource Group",
                    SubscriptionId = "1"
                });
            }

            return resourceGroups;
        }

        private static string EscapeString(string inputText)
        {
            return Regex.Replace(inputText, @"\r\n?|\n", "");
        }


    }
}
