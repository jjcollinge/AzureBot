using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AzureBot.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> GetToken(object[] args);
        Uri GetAuthenticationUri(string userId);
    }
}