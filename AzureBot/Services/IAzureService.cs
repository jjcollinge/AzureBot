using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureBot.Services
{
    public interface IAzureService
    {
        Task<IDictionary<string, string>> GetSubscriptions(string token);
        Task<List<Resource>> GetResources(string token);
        Task<List<Resource>> GetResourceGroups(string token);
    }
}