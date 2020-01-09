using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ConfigCore.Extensions;
using Microsoft.Extensions.Logging;

namespace ConfigClient_ApiDefault
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
        /// Basic example using ConfigCore.ApiSource to build the configuration in Program.ConfigureAppConfiguration
        /// This is the simplest extension method with only one required parameter for the name of the Environment Variable containing the URL of the configuration API.
        /// The URL must include the base address and endpoint route without parameter values.
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
                // Note: In a real project, you would also include other sources and order them for desired precedence.
                // 
                // See overload methods for specifying non-default values for application ID and Authentication settings
                // 
                config.AddApiSource("CONFIGAPI-URL");
                
     

                // For overriding all other default settings like authentication options, see sample projects ConfigClient_ApiCustom and ConfigClient_ApiStartup

            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
