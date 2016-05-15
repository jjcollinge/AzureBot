using AzureBot.Controllers;
using AzureBot.Model;
using AzureBot.Repos;
using AzureBot.Services;
using AzureBot.Services.Impl;
using AzureBot.Services.Interfaces;
using AzureBot.Services.Logging;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace AzureBot
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Json settings
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Newtonsoft.Json.Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };

            // Environment variables
            // TODO: Read logging folder location from env val

            // App settings
            var apiVersion = ConfigurationManager.AppSettings["AzureAPIVersion"];

            // Web API configuration and services
            var container = new UnityContainer();
            container.RegisterInstance<IUserRepository>(new UserRepository());
            container.RegisterType<IAuthenticationService, OAuthAuthenticationService>();
            container.RegisterType<IAzureService, RESTAzureService>(new InjectionConstructor(apiVersion));
            container.RegisterType<IIntentService, LUISIntentService>();
            container.RegisterType<IValidationService, ValidationService>(new InjectionConstructor(1, 500));
            container.RegisterType<IStringLogger, FileLogger>(new InjectionConstructor("\\src\\temp\\test.log"));
            container.RegisterType<ILoggerService, StringLoggerService>();
            config.DependencyResolver = new UnityResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
