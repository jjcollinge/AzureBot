using AzureBot.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AzureBot.Services
{
    public class AuthService
    {
        public ClientInfo ClientInfo;

        public AuthService(ClientInfo clientInfo)
        {
            ClientInfo = clientInfo;
        }

        public async Task<string> GetToken(string code = null,
                                           string state = null)
        {
            string token = string.Empty;

            if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(state))
            {
                var tokenUri = BuildOAuthTokenRequestUri(code);
                string result = null;

                using (var http = new HttpClient())
                {
                    var c = tokenUri.Query.Remove(0, 1);
                    var content = new StringContent(c);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    var resp = await http.PostAsync(new Uri($"https://login.microsoftonline.com/{ClientInfo.TenantId}/oauth2/token"), content);
                    result = await resp.Content.ReadAsStringAsync();
                }

                dynamic res = JsonConvert.DeserializeObject<string>(result);
                token = res.access_token.ToString();
            }

            return token;
        }

        public Uri BuildOAuthCodeRequestUri(string userId)
        {
            UriBuilder uriBuilder =
                new UriBuilder($"https://login.microsoftonline.com/{ClientInfo.TenantId}/oauth2/authorize");

            var query = new StringBuilder();
            query.AppendFormat("redirect_uri={0}", Uri.EscapeUriString(ClientInfo.RedirectUri));
            query.AppendFormat("&client_id={0}", Uri.EscapeUriString(ClientInfo.ClientId));
            query.AppendFormat("&client_secret={0}", Uri.EscapeUriString(ClientInfo.ClientSecret));
            query.Append("&response_type=code");

            if (!string.IsNullOrEmpty(userId))
                query.Append($"&state={userId}");

            uriBuilder.Query = query.ToString();
            return uriBuilder.Uri;
        }

        private Uri BuildOAuthTokenRequestUri(string code, string refreshToken = "")
        {
            UriBuilder uriBuilder =
                new UriBuilder($"https://login.microsoftonline.com/{ClientInfo.TenantId}/oauth2/token");
            var query = new StringBuilder();

            query.AppendFormat("redirect_uri={0}", Uri.EscapeUriString(ClientInfo.RedirectUri));
            query.AppendFormat("&client_id={0}", Uri.EscapeUriString(ClientInfo.ClientId));
            query.AppendFormat("&client_secret={0}", Uri.EscapeUriString(ClientInfo.ClientSecret));

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
