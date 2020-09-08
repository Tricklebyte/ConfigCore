using ConfigCore.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ConfigApiClient_Windows
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
                config.AddApiSource("ConfigURL-Win");
                

                // For overriding all other default settings like authentication options, see sample projects ConfigClient_ApiCustom and ConfigClient_ApiStartup

            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
