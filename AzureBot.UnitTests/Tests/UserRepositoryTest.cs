using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzureBot.Repos;
using AzureBot.Model;
using System.Collections.Generic;

namespace AzureBot.UnitTests.Tests
{
    [TestClass]
    public class UserRepositoryTest
    {
        [TestMethod]
        public void TestClearingAllUsers()
        {
            IUserRepository users = new UserRepository();

            for (int i = 0; i < 10; i++)
            {
                User newUser = new User(i.ToString());
                users.Add(newUser);
            }

            users.Clear();

            var userList = new List<User>(users.GetAll());
            Assert.AreEqual(0, userList.Count);
        }

        [TestMethod]
        public void TestAddingNewUser()
        {
            IUserRepository users = new UserRepository();

            User newUser = new User("0");
            users.Add(newUser);

            Assert.IsNotNull(users.GetById(newUser.Id));
        }

        [TestMethod]
        public void TestRemovingExistingUser()
        {
            IUserRepository users = new UserRepository();

            User newUser = new User("0");
            users.Add(newUser);

            users.Remove(newUser.Id);

            Assert.IsNull(users.GetById(newUser.Id));
        }   
    }
}
