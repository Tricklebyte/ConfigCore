using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ConfigCore.Extensions;
using System;

namespace DbConfigClient_StartupFile

{
    /// Sample Client for  ConfigCore.DbSource custom configuration source and provider. The Provider gets app configuration settings from a SQL server database during configuration build using ADO.NET.
    /// The DB search is filtered by AppId, which by default will be the current Project/Assemply name. A custom application Id may be supplied using overloaded methods.
    /// 
    public class Program
    {
        private static IConfiguration _preConfig { get; set; }
        public static void Main(string[] args)
        {
            // BUILD PRE-CONFIG ===========================================
            // Creates initial IConfiguration object that will be used as a parameter for the final configuration builder.
            // Required when using overload method .AddDbSource(IConfiguration)
            // This example builds from appsettings.json. Additional providers may also be specified as needed
            // The resulting config must contain section ConfigOptions:DbSource
            // Uncomment the following line, when using the IConfiguration Parameter Overload in ConfigureAppConfiguration.
            
            // _preConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            
            // END PRE-CONFIG  =============================================

            CreateHostBuilder(args).Build().Run();
        }
    
        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(config =>
        {
            // Build app configuration using ConfigCore.DbSource.
            
            // Note: In a real project, you would include other sources and order them for desired precedence.

            //... Preceding config sources ....

            // BASE METHOD: Database Configuration Source Basic Method AddDbSource(string ConfigUrlVar)
            // Load configuration in a single step using only the name of the Environment Variable holding the connection string.
            // Related overloads provide additional parameters for AppId, SqlCommandTimout, and Optional.
             config.AddDbSource("ConfigDb-Connection");


            // OVERLOAD METHOD FOR ICONFIGURATION OBJECT PARAMETER  AddDbSource(IConfiguration):  
            // Load a 'Pre-config' configuration first to customize the options for connecting to the database when building the final IConfiguration.
            // Configuration section ConfigOptions:DbSource is required
            // This is required when using non-default table and column names.
            // Load a config containing these settings first, then pass the configuration object into the AddDbSource extension method of IConfigurationBuilder.
            // These settings must be found in Config section ConfigOptions:DbSource, and they may be sourced from other configuration sources like appsettings files, environment variables, and command line arguments.
            // This override method may also be called from the Startup File using the default configuration supplied by DI. 
            // To use this overload, you must uncomment both the statement below AND the IConfigurationBuilder statement in the Main method that creates the _preConfig.

            // config.AddDbSource(_preConfig);


            //... Subsequent config sources ...
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
    
      
        
    
    }
}

