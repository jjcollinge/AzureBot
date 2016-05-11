using AzureBot.Model;
using AzureBot.Repos;
using AzureBot.Services;
using AzureBot.Services.Interfaces;
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
        private IAuthenticationService _authService;
        private IUserRepository _users;

        public AuthController(IUserRepository users, IAuthenticationService authService)
        {
            _authService = authService;
            _users = users;
        }

        [Route("api/auth/home")]
        [HttpGet]
        public HttpResponseMessage Home(string UserId)
        {
            var response = Request.CreateResponse(HttpStatusCode.Found);
            response.Headers.Location = _authService.GetAuthenticationUri(UserId);
            return response;
        }

        [Route("api/auth/receivetoken")]
        [HttpGet()]
        public async Task<string> ReceiveToken(string code = null, string state = null)
        {
            // Get the token from the authentication service
            var token = await _authService.GetToken(new object[] {code, state});

            // Attempt to retrieve an existing user
            var users = UserRepository.GetInstance();
            var user = users.GetById(state);

            // Create user if one doesn't exist
            if (user == null)
            {
                user = new Model.User(state);
                users.Add(user);
            }

            // Assign token
            user.Token = token;

            return String.IsNullOrEmpty(token) ? "Failed" : "Success";
        }
    }
}