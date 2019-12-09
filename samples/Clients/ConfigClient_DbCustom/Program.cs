using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ConfigCore.Extensions;
using System;

namespace DbConfigClient_StartupFile

{
    /// Sample Client for  ConfigCore.DbSource custom configuration srource and provider. The Provider gets configuration settings from a SQL server database.
    /// The DB search is filtered by AppId, which by default will be the current Project/Assemply name.
    /// This example loads configuration database options first, then loads the full configuration using these pre-loaded options.
    /// This is required when using non-default table and column names, a non-default AppId, or when needing to set the SQL Command Timeout.
    /// These settings must be found in Configuration section ConfigOptions:DbSource.
    /// Load a config containing these settings first, then pass it into the AddDbSource extension method of IConfigurationBuilder.
    ///
    /// 
    /// NOTE: To load the DBSource with default settings using just the name of the ConnectionString Environment Variable, see example client DbConfigClient_DefaultDb
    public class Program
    {
        private static IConfiguration _preConfig { get; set; }
        public static void Main(string[] args)
        {
            // Creates initial configuration using just the Json provider.
            // Additional providers may also be specified if needed
            // The resulting config must contain section ConfigOptions:DbSource
            _preConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            CreateHostBuilder(args).Build().Run();
        }
    
        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(config =>
        {
            // Build app configuration using DbSource.
            // Note: In a real project, you would include other sources and order them for desired precedence.

            //... other config sources ....

            config.AddDbSource(_preConfig, false);

            //... other config sources ...
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
    
      
        
    
    }
}

