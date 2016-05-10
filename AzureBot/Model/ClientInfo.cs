using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureBot.Model
{
    public class ClientInfo
    {
        public string RedirectUri { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string TenantId { get; }

        public ClientInfo(string redirectUri,
                          string clientId,
                          string clientSecret,
                          string tenantId)
        {
            RedirectUri = redirectUri;
            ClientId = clientId;
            ClientSecret = clientSecret;
            TenantId = tenantId;
        }
    }
}