using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AzureBot.Controllers
{
    public class AuthController : ApiController
    {
        private UserRegistry _users;
        private static string _redirectUri = "http://localhost:3978/api/auth/receivetoken";
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _tenantId;

        public AuthController()
        {
            // Environment variables
            _clientId = Environment.GetEnvironmentVariable("ARM_API_CLIENTID");
            _clientSecret = Environment.GetEnvironmentVariable("ARM_API_CLIENTSECRET");

            // App settings
            _tenantId = ConfigurationManager.AppSettings["TenantId"];

            _users = UserRegistry.GetSingleton();
        }

        [Route("api/auth/home")]
        [HttpGet]
        public HttpResponseMessage Home(string UserId)
        {
            var response = Request.CreateResponse(System.Net.HttpStatusCode.Found);
            response.Headers.Location = CreateOAuthCodeRequestUri(UserId);
            return response;
        }

        private Uri CreateOAuthCodeRequestUri(string UserId)
        {
            UriBuilder uriBuilder = new UriBuilder($"https://login.microsoftonline.com/{_tenantId}/oauth2/authorize");

            var query = new StringBuilder();
            query.AppendFormat("redirect_uri={0}", Uri.EscapeUriString(_redirectUri));
            query.AppendFormat("&client_id={0}", Uri.EscapeUriString(_clientId));
            query.AppendFormat("&client_secret={0}", Uri.EscapeUriString(_clientSecret));
            query.Append("&response_type=code");

            if (!string.IsNullOrEmpty(UserId))
                query.Append($"&state={UserId}");

            uriBuilder.Query = query.ToString();
            return uriBuilder.Uri;
        }

        private Uri CreateOAuthTokenRequestUri(string code, string refreshToken = "")
        {
            UriBuilder uriBuilder = new UriBuilder($"https://login.microsoftonline.com/{_tenantId}/oauth2/token");
            var query = new StringBuilder();

            query.AppendFormat("redirect_uri={0}", Uri.EscapeUriString(_redirectUri));
            query.AppendFormat("&client_id={0}", Uri.EscapeUriString(_clientId));
            query.AppendFormat("&client_secret={0}", Uri.EscapeUriString(_clientSecret));

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

        [Route("api/auth/receivetoken")]
        [HttpGet()]
        public async Task<string> ReceiveToken(string code = null, string state = null)
        {
            if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(state))
            {
                var tokenUri = CreateOAuthTokenRequestUri(code);
                string result = null;

                using (var http = new HttpClient())
                {
                    var c = tokenUri.Query.Remove(0, 1);
                    var content = new StringContent(c);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    var resp = await http.PostAsync(new Uri($"https://login.microsoftonline.com/{_tenantId}/oauth2/token"), content);
                    result = await resp.Content.ReadAsStringAsync();
                }

                dynamic obj = JsonConvert.DeserializeObject(result);

                var user = AzureBot.Model.User.GetOrCreate(state);
                user.Token = obj.access_token.ToString();
                _users.UpdateUser(user);
                return "Success";
            }
            return "Something went wrong, please try again";
        }
    }
}