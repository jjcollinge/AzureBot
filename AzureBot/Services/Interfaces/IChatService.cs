using AzureBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureBot.Services.Interfaces
{
    public interface IChatService
    {
        string PromptUserLogin(User user);

        string NoIdProvided();

        string GreetUser(string username);

        string InvalidMessage();

        string UnsupportedIntent();

        string RenderResourceList(List<Resource> resources);

        string RenderSubscriptionList(IDictionary<string, string> subscriptions);
    }
}
