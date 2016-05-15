using AzureBot.Services.Impl;
using AzureBot.Services.Interfaces;
using AzureBot.UnitTests.Mocks;
using Microsoft.Bot.Connector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureBot.UnitTests.Tests
{
    [TestClass]
    public class ValidationTest
    {
        private IValidationService _validationService;

        private int MAX_MESSAGE_LENGTH = 500;
        private int MIN_MESSAGE_LENGTH = 1;

        public ValidationTest()
        {
            _validationService = new ValidationService(MIN_MESSAGE_LENGTH, MAX_MESSAGE_LENGTH);
        }

        [TestMethod]
        public async Task TestMaxiumumMessageLength()
        {
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

            for (int i = 0; i < MAX_MESSAGE_LENGTH + 1; i++)
            {
                message.Text += "a";
            }

            var actualResponse = await _validationService.IsValidMessage(message);
            var expectedResponse = false;

            Assert.IsTrue(actualResponse == expectedResponse);
        }

        [TestMethod]
        public async Task TestMinimumMessageLength()
        {
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
                },
                Text = ""
            };

            var actualResponse = await _validationService.IsValidMessage(message);
            var expectedResponse = false;

            Assert.IsTrue(actualResponse == expectedResponse);
        }

        [TestMethod]
        public async Task TestValidMessageLength()
        {
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

            // Assumes MAX_MESSAGE_LENGTH > MIN_MESSAGE_LENGTH + 1
            for(int i = 0; i <= MIN_MESSAGE_LENGTH; i++)
            {
                message.Text += "a";
            }

            var actualResponse = await _validationService.IsValidMessage(message);
            var expectedResponse = true;

            Assert.IsTrue(actualResponse == expectedResponse);
        }
    }
}
