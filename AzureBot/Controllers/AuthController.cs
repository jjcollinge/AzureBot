using AzureBot.Model;
using AzureBot.Services;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net;
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
        private AuthService _authService;

        public AuthController(AuthService authService)
        {
            // Environment variables
            var clientId = Environment.GetEnvironmentVariable("ARM_API_CLIENTID");
            var clientSecret = Environment.GetEnvironmentVariable("ARM_API_CLIENTSECRET");

            // App settings
            var tenantId = ConfigurationManager.AppSettings["TenantId"];
            var baseUri = ConfigurationManager.AppSettings["RedirectBaseUri"];
            var redirectUri = baseUri + "/api/auth/receivetoken";

            var clientInfo = new ClientInfo(redirectUri,
                                            clientId,
                                            clientSecret,
                                            tenantId);

            // TODO: Move this into IoC container and fix dependency injection
            _authService = authService;
            _authService.ClientInfo = clientInfo;

            _users = UserRegistry.GetSingleton();
        }

        [Route("api/auth/home")]
        [HttpGet]
        public HttpResponseMessage Home(string UserId)
        {
            var response = Request.CreateResponse(HttpStatusCode.Found);
            response.Headers.Location = _authService.BuildOAuthCodeRequestUri(UserId);
            return response;
        }

        [Route("api/auth/receivetoken")]
        [HttpGet()]
        public async Task<string> ReceiveToken(string code = null, string state = null)
        {
            var token = await _authService.GetToken(code, state);

            var user = AzureBot.Model.User.GetOrCreate(state);
            user.Token = token;
            _users.UpdateUser(user);

            return String.IsNullOrEmpty(token) ? "Failed" : "Success";
        }
    }
}