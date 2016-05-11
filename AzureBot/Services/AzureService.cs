using AzureBot.Controllers;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AzureBot.Services
{
    public class AzureService : IAzureService
    {
        private string _apiVersion;

        public string CurrentSubscriptionId { get; set; }

        public AzureService(string apiVersion)
        { _apiVersion = apiVersion; }

        public async Task<IDictionary<string, string>> GetSubscriptions(string token)
        {
            Dictionary<string, string> subscriptions = new Dictionary<string, string>();

            using (var http = new HttpClient())
            {
                var uri = $"https://management.azure.com/subscriptions?&api-version={_apiVersion}";
                http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var res = await http.GetAsync(uri);
                res.EnsureSuccessStatusCode();

                var strRes = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(strRes);

                foreach (var element in data.value)
                {
                    // Convert from dynamic to strongly typed
                    string name = element.displayName;
                    string subId = element.subscriptionId;

                    subscriptions.Add(name, subId);
                }
            }

            if (CurrentSubscriptionId == null)
            {
                CurrentSubscriptionId = subscriptions.First().Value;
            }

            return subscriptions;
        }

        public async Task<List<Resource>> GetResources(string token)
        {
            var uriExtension = "resources";
            var resources = await GetResources(uriExtension, token);
            return resources;
        }

        public async Task<List<Resource>> GetResourceGroups(string token)
        {
            var uriExtension = "resourcegroups";
            var resourceGroups = await GetResources(uriExtension, token);
            return resourceGroups;
        }

        private async Task<List<Resource>> GetResources(string uriExtension, string token)
        {
            if (CurrentSubscriptionId == null)
            {
                await GetSubscriptions(token);
            }

            var uri = $"https://management.azure.com/subscriptions/{CurrentSubscriptionId}/{uriExtension}?&api-version={_apiVersion}";


            List<Resource> resources = new List<Resource>();

            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var res = await http.GetAsync(uri);
                res.EnsureSuccessStatusCode();

                var strRes = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(strRes);

                foreach (var element in data.value)
                {
                    // Convert from dynamic to strongly typed
                    Resource resource = new Resource();
                    resource.Id = element.id;
                    resource.Name = element.name;
                    resource.Type = element.type;
                    resource.Location = element.location;
                    resource.SubscriptionId = CurrentSubscriptionId;

                    resources.Add(resource);
                }
            }

            return resources;
        }
    }
}