﻿using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using DemoWebApi.Models;
using Newtonsoft.Json.Converters;
using System.Linq;

namespace DemoWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //http://www.asp.net/web-api/overview/security/enabling-cross-origin-requests-in-web-api
            //https://developer.mozilla.org/en-US/docs/Web/HTTP/Access_control_CORS
            //https://msdn.microsoft.com/en-us/magazine/dn532203.aspx
            var cors = new System.Web.Http.Cors.EnableCorsAttribute("*", "Origin, Content-Type, Accept,User-Agent,Accept-Language,Accept-encoding,Referer,Content-Length,Connection, Authorization", "GET, PUT, POST, DELETE, OPTIONS"); //better to set urls in production
            config.EnableCors(cors);

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //http://forums.asp.net/t/1821729.aspx?JsonMediaTypeFormatter+does+not+work+with+Tuple+int+List+string+
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();//   new DefaultContractResolver();
            //this will support Tuple serialization in JSON

            config.Filters.Add(new ValidateModelAttribute());

            config.Services.RemoveAll(typeof(System.Web.Http.Validation.ModelValidatorProvider), (provider) => provider is System.Web.Http.Validation.Providers.InvalidModelValidatorProvider);
            //  GlobalConfiguration.Configuration.Services.Add(typeof(System.Web.Http.Validation.ModelValidatorProvider), new System.Web.Http.Validation.Providers.RequiredMemberModelValidatorProvider());

            config.Services.Replace(typeof(System.Web.Http.Hosting.IHostBufferPolicySelector), new NoBufferPolicySelector());

            var jsonFormatter = config.Formatters.OfType<System.Net.Http.Formatting.JsonMediaTypeFormatter>().First();

            var settings = jsonFormatter.SerializerSettings;
            settings.Converters.Add(new IsoDateTimeConverter());

			config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain"));
        }
    }
}
