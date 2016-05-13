using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AzureBot.Services.Interfaces
{
    public interface IIntentService
    {
        Task<string> GetIntentAsync(string inputText);
    }
}