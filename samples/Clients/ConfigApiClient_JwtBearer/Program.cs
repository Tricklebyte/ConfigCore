using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ConfigCore.Extensions;
using Microsoft.Extensions.Logging;

namespace ConfigApiClient_Anon
{
    public class Program
    {
        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Use ConfigCore.ApiSource to build the configuration in Program.ConfigureAppConfiguration
        /// The Configuration API is protected by BearerToken Authorization
        /// The public IdentityServer demo server is used with a standard preconfigured client and scope
        /// </summary>

        // REQUIREMENTS
        // 1.) The configuration API must be running and available - see sample project ConfigApi_JwtBearer for simple testing api
        // 2.) The following environment variables must be present in the client's host environment when using this example:
        //          Name:  CONFIGURL-JwtBearer
        //          Value: https://localhost:5009/iapi/ConfigSettings/  (url of sample project api)
        //          
        //          Name:   "ConfigApi-JwtBearerAuthority"
        //          Value:  "https://demo.identityserver.io"
        //
        //          Name:   "ConfigApi-JwtBearerClientId"
        //          Value:  "m2m.short"
        //
        //          Name:   "ConfigApi-JwtBearerClientSecret"
        //          Value:  "secret"
        //
        //          Name:   "ConfigApi-JwtBearerClientScope"
        //          Value:  "api"
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureAppConfiguration(config =>
            {
                // Build app configuration using ApiSource.
                // Note: In a real project, you would also include other configuration sources and order them for desired precedence.

	// We are using the Identityserver public demo server with one of the standard, preconfigured clients
                config.AddApiSource("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope");
                
	// DEFAULT ROUTE PARAMETER Since there are no Route or Query parameters specified, 
                //     the application project/assembly name will be added to the Configuration URL string as a single route parameter.
                //     Other overloads allow for multiple route or query parameters.

            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
