using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AzureBot
{
    public class AzureService
    {
        public AzureService()
        { }

        public Task<List<string>> GetAllSubscriptions()
        {
            throw new NotImplementedException();
        }

        public Task<List<Resource>> GetAllResourcesAsync()
        {
            throw new NotImplementedException();
        }
    }
}