using AzureBot.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureBot.Model;

namespace AzureBot.UnitTests.Mocks
{
    class MockUserRepository : IUserRepository
    {
        public void Add(User user)
        {

        }

        public void Clear()
        {

        }

        public IEnumerable<User> GetAll()
        {
            return null;
        }

        public User GetById(string id)
        {
            return null;
        }

        public bool Remove(string id)
        {
            return true;
        }
    }
}
