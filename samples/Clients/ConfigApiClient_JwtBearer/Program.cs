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
        /// The AddApiSource extension methods for JwtBearer use an Environment variable name for the Configuration URL as the first parameter like the other extension methods.
        /// The OIDC parameters that come afterward use string values.
        /// 
       
        /// </summary>

        // REQUIREMENTS
        // 1.) The configuration API must be running and available - see sample project ConfigApi for simple testing api
        // 2.) The following environment variable must be present in the client's host environment:
        //          Name:  CONFIGAPI-URL
        //          Value: https://localhost:44397/iapi/ConfigSettings/  (url of sample project api)
        // 

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureAppConfiguration(config =>
            {
                // Build app configuration using ApiSource.
                // Note: In a real project, you would also include other configuration sources and order them for desired precedence.

                config.AddApiSource("ConfigURL-JwtBearer", "https://demo.identityserver.io", "m2m.short", "secret", "api");
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
