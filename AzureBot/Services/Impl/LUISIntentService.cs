using AzureBot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using AzureBot.Model;

namespace AzureBot.Services.Impl
{
    public class LUISIntentService : IIntentService
    {
        public async Task<string> GetIntentAsync(string inputText)
        {
            // Get the most confident intent
            var intent = await GetIntent(inputText);
            return intent.intent;
        }

        private async Task<IntentRoot> GetIntent(string inputText)
        {
            IntentRoot intent = new IntentRoot();

            using (var http = new HttpClient())
            {
                string key = Environment.GetEnvironmentVariable("AZUREBOT_LUIS_API_KEY");
                string id = Environment.GetEnvironmentVariable("AZUREBOT_LUIS_API_ID");
                string uri = $"https://api.projectoxford.ai/luis/v1/application?id={id}&subscription-key={key}&q={inputText}";
                var res = await http.GetAsync(uri);
                res.EnsureSuccessStatusCode();

                var strRes = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(strRes);

                // Get top intent
                intent.intent = data.intents[0].intent;
                intent.score = data.intents[0].score;
                intent.actions = data.intents[0].actions;
            }

            return intent;
        }
    }
}