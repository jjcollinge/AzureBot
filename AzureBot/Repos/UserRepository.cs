using AzureBot.Model;
using AzureBot.Repos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureBot
{
    internal class UserRepository : IUserRepository
    {
        private static UserRepository _instance;
        IDictionary<string, User> _users;

        private UserRepository()
        {
            _users = new Dictionary<string, User>();
        }

        public static UserRepository GetInstance()
        {
            if (_instance == null)
                _instance = new UserRepository();

            return _instance;
        }

        public void Add(User user)
        {
            _users.Add(user.Id, user);
        }

        public void Clear()
        {
            _users.Clear();
        }

        public IEnumerable<User> GetAll()
        {
            return _users.Cast<User>().ToList();
        }

        public User GetById(string id)
        {
            User user = null;
            
            if(_users.ContainsKey(id))
                user = _users[id];

            return user;
        }

        public bool Remove(string id)
        {
            return _users.Remove(id);
        }
    }
}