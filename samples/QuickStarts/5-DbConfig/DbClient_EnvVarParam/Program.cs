using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ConfigCore.Extensions;
using System;

namespace DbConfigClient_StartupFile

{
    /// Sample Client for  ConfigCore.DbSource custom configuration source and provider. The Provider gets app configuration settings from a SQL server database during configuration build using ADO.NET.
    /// The DB search is filtered by AppId, which by default will be the current Project/Assemply name. A custom application Id may be supplied using overloaded methods.

    /// BASIC METHOD: AddDbSource(string) : Load configuration in a single step using only the name of the Environment Variable holding the connection string. Default values are used for Application Id, Table and Column names, and S
    //                              
    /// OVERLOAD with IConfiguration Parameter  AddDbSource(IConfiguration): 
    /// Load a 'Pre-config' configuration first to customize the options for connecting to the database when building the final IConfiguration.
    /// This is required when using non-default table and column names.
    /// Load a config containing these settings first, then pass the configuration object into the AddDbSource extension method of IConfigurationBuilder.
    /// These settings must be found in Config section ConfigOptions:DbSource, and they may be sourced from other configuration sources like appsettings files, environment variables, and command line arguments.
    /// This override method may also be called from the Startup File using the default configuration supplied by DI. 
    /// 
    public class Program
    {
        private static IConfiguration _preConfig { get; set; }
        public static void Main(string[] args)
        {
            
            // DBConfig OPTION 2 ONLY : OVERLOAD FOR ICONFIGURATION OBJECT PARAMETER   =======================
            // Creates initial IConfiguration object that will be used as a parameter for the final configuration builder.
            // Additional providers may also be specified if needed
            // The resulting config must contain section ConfigOptions:DbSource

            // _preConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        
            
            
            CreateHostBuilder(args).Build().Run();
        }
    
        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(config =>
        {
            // Build app configuration using DbSource.
            // Note: In a real project, you would include other sources and order them for desired precedence.

            //... Preceding config sources ....
            
            // DBConfig OPTION 1: Environment Variable Name Parameter
            config.AddDbSource("ConfigDb-Connection");

            // DbConfig OPTION 2: IConfiguration Parameter
            // config.AddDbSource(_preConfig);


            //... Subsequent config sources ...
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
    
      
        
    
    }
}

