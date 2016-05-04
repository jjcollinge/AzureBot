using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureBot
{
    public class Resource
    {
        public string Name { get; set; }
        public string ResourceId { get; set; }
        public string ResourceName { get; set; }
        public string ResourceType { get; set; }
        public string ResourceGroupName { get; set; }
        public string Location { get; set; }
        public string SubscriptionId { get; set; }
    }
}