using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ConfigCore.Extensions;
using Microsoft.Extensions.Logging;

namespace ConfigClient_ApiCustom
{
    public class Program
    {
        private static IConfiguration _preConfig { get; set; }
        /// <summary>
        /// This example shows how to override the default settings of the Api Configuration Source.
        /// An intitial configuration, _preConfig, is built in Main prior to calling ConfigureAppConfiguration.
        /// It is also possible to use the default configuration passed into Startup by DI for initiliaztion and build the configuration in ConfigureServices using a single builder.
        /// (see example ConfigClient_ApiStartup)
        /// 
        /// </summary>

        // REQUIREMENTS
        // 1.) The configuration API must be running and available 
        // 2.) The required parameter for this overload method is an IConfigruation instance.
        //     The configuration passed to the method must contain section "ConfigOptions:ApiSource". 
        //     This requires that a configuration is built prior to calling .AddApiSource on the final configuration builder.
        // 

        public static void Main(string[] args)
        {
            // Creates initial configuration using just the Json provider.
            // Additional providers may also be specified if needed
            // The resulting config must contain section ConfigOptions:ApiSource
            _preConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            CreateHostBuilder(args).Build().Run();
        }


       

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureAppConfiguration(config =>
            {                
                // Build app configuration using ApiSource.
                // Note: In a real project, you would include other sources and order them for desired precedence.
               
                //... other config sources ....
                
                config.AddApiSource(_preConfig,false);
                
                //... other config sources ...
               

            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
