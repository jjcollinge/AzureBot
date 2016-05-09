using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureBot
{
    public class Resource
    {
        public Resource()
        {
            Tags = new List<string>();
        }

        public string Name { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string GroupName { get; set; }
        public string Location { get; set; }
        public string SubscriptionId { get; set; }
        public List<string> Tags { get; set; }
    }
}