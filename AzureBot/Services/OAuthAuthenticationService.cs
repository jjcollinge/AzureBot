using AzureBot.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AzureBot.Services
{
    public class OAuthAuthenticationService : IAuthenticationService
    {
        private ClientInfo _clientInfo;

        public OAuthAuthenticationService()
        {
            // Environment variables
            var clientId = Environment.GetEnvironmentVariable("ARM_API_CLIENTID");
            var clientSecret = Environment.GetEnvironmentVariable("ARM_API_CLIENTSECRET");

            // App settings
            var tenantId = ConfigurationManager.AppSettings["TenantId"];
            var baseUri = ConfigurationManager.AppSettings["RedirectBaseUri"];
            var redirectUri = baseUri + "/api/auth/receivetoken";

            _clientInfo = new ClientInfo(redirectUri,
                                            clientId,
                                            clientSecret,
                                            tenantId);
        }

        public async Task<string> GetToken(object[] args)
        {
            string authenticationCode = (string)args[0];
            string userId = (string)args[1];
            string token = string.Empty;

            if (!string.IsNullOrEmpty(authenticationCode) && !string.IsNullOrEmpty(userId))
            {
                var tokenUri = BuildOAuthTokenRequestUri(authenticationCode);
                string result = null;

                using (var http = new HttpClient())
                {
                    var c = tokenUri.Query.Remove(0, 1);
                    var content = new StringContent(c);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    var resp = await http.PostAsync(new Uri($"https://login.microsoftonline.com/{_clientInfo.TenantId}/oauth2/token"), content);
                    result = await resp.Content.ReadAsStringAsync();
                }

                dynamic res = JsonConvert.DeserializeObject<dynamic>(result);
                token = res.access_token.ToString();
            }

            return token;
        }

        public Uri GetAuthenticationUri(string userId)
        {
            return BuildOAuthCodeRequestUri(userId);
        }

        private Uri BuildOAuthCodeRequestUri(string userId)
        {
            UriBuilder uriBuilder =
                new UriBuilder($"https://login.microsoftonline.com/{_clientInfo.TenantId}/oauth2/authorize");

            var query = new StringBuilder();
            query.AppendFormat("redirect_uri={0}", Uri.EscapeUriString(_clientInfo.RedirectUri));
            query.AppendFormat("&client_id={0}", Uri.EscapeUriString(_clientInfo.ClientId));
            query.AppendFormat("&client_secret={0}", Uri.EscapeUriString(_clientInfo.ClientSecret));
            query.Append("&response_type=code");

            if (!string.IsNullOrEmpty(userId))
                query.Append($"&state={userId}");

            uriBuilder.Query = query.ToString();
            return uriBuilder.Uri;
        }

        private Uri BuildOAuthTokenRequestUri(string code, string refreshToken = "")
        {
            UriBuilder uriBuilder =
                new UriBuilder($"https://login.microsoftonline.com/{_clientInfo.TenantId}/oauth2/token");
            var query = new StringBuilder();

            query.AppendFormat("redirect_uri={0}", Uri.EscapeUriString(_clientInfo.RedirectUri));
            query.AppendFormat("&client_id={0}", Uri.EscapeUriString(_clientInfo.ClientId));
            query.AppendFormat("&client_secret={0}", Uri.EscapeUriString(_clientInfo.ClientSecret));

            string grant = "authorization_code";

            if (!string.IsNullOrEmpty(refreshToken))
            {
                grant = "refresh_token";
                query.AppendFormat("&refresh_token={0}", Uri.EscapeUriString(refreshToken));
            }
            else
            {
                query.AppendFormat("&code={0}", Uri.EscapeUriString(code));
            }

            query.AppendFormat("&grant_type={0}", grant);
            query.AppendFormat("&resource={0}", "https://management.azure.com/");

            uriBuilder.Query = query.ToString();
            return uriBuilder.Uri;
        }
    }
}
