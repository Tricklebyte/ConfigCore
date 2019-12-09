using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ConfigCore.Extensions;

namespace DbConfigClient_DefaultDb
{/// <summary>
 /// Sample Client for  ConfigCore.DbSource custom configuration srource and provider. The Provider gets configuration settings from a SQL server database.
 /// The DB search is filtered by AppId, which by default will be the current Project/Assemply name.
 /// </summary>
    //  
    // REQUIREMENTS
    // 1.) The configuration Db must be available - see DBSource/SQL folder for scripts to build the sample database
    // 2.) To use this example, the following environment variable must be present in the client's host environment:
    //          Name:  ConfigDb-Connection
    //          Value: Server=(localdb)\MSSQLLocalDB;Database=ConfigDb;Trusted_Connection=True;MultipleActiveResultSets=true  (Conn string of sample db)
    // 


    public class Program
    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        //This simple example uses default database table and column names, and only requires the name of the Environment Variable where the connection string is stored.
        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(config =>
        {
            // Build app configuration using DbSource.
            // Note: In a real project, you would also include other sources and order them as desired for precedence.

            // DB SOURCE 
            config.AddDbSource("ConfigDb-Connection");

            // There is also an overload that allows you to specify a non-default application name
            //config.AddApiSource("CONFIGAPI-URL","CustomAppName");

            // For overriding all other default settings like authentication options, see sample projects ConfigClient_DbCustom and ConfigClient_DbStartup

        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
    }
}
