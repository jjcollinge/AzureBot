using AzureBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureBot.Repos
{
    public interface IUserRepository
    {
        void Add(User user);
        User GetById(string id);
        IEnumerable<User> GetAll();
        bool Remove(string id);
        void Clear();
    }
}
