using AzureBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureBot
{
    internal class UserRegistry
    {
        //TODO: Change this to the repository pattern and persist

        static private UserRegistry _instance;
        private IDictionary<string, User> _users;

        private UserRegistry()
        {
            _users = new Dictionary<string, User>();
        }

        public static UserRegistry GetSingleton()
        {
            if (_instance == null)
                _instance = new UserRegistry();

            return _instance;
        }

        public User GetUser(string id)
        {
            if(_users.ContainsKey(id))
            {
                return _users[id];
            }
            return null;
        }

        public void AddUser(User user)
        {
            if (!_users.ContainsKey(user.Id))
            {
                _users.Add(user.Id, user);
            }
        }

        public void UpdateUser(User user)
        {
            if (!_users.ContainsKey(user.Id))
            {
                DeleteUser(user.Id);
                AddUser(user);
            }
        }

        public void DeleteUser(string id)
        {
            _users.Remove(id);
        }

        public void Clear()
        {
            _users.Clear();
        }

    }
}