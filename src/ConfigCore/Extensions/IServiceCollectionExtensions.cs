using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace ConfigCore
{
    public static class IServiceCollectionExtensions
    {
        //public static void AddDataProtectionServices(this IServiceCollection services, IConfiguration config)
        //{
        //    //Create Data Protection Service to decrypt connection string for configuration database

        //    services.AddDataProtection()
        //        .PersistKeysToFileSystem(new DirectoryInfo(config["Encryption:KeyStore"]))
        //        .ProtectKeysWithDpapi(true)
        //        .SetApplicationName(config["Encryption:EncAppName"]);

        //    // Get reference to Data ProtectionProvider Service to register ICryptoHelpert.
        //    var serviceProvider = services.BuildServiceProvider();
        //    var proProvider = serviceProvider.GetDataProtectionProvider();

        //    // Create Crypto Helper service to decrypt secure environment variables 
        //    services.AddSingleton<ICryptoHelper>(h => new CryptoHelper(proProvider));

        //}

        ////  initialize the DbContext and Repo for API that connects directly to Config Database. 
        //public static void AddDBConfigServices(this IServiceCollection services, IConfiguration initConfig, IHostingEnvironment environment)
        //{
        //    //services.AddSqlContexts<ConfigSettingDbContext>(initConfig, environment);
        //    //services.AddSqlRepos<ConfigSettingRepo>();
        //    services.AddDbContexts<ConfigSettingDbContext, ConfigSettingHistoryDbContext>(initConfig, environment);

        //    services.AddRepos<ConfigSettingRepo>();

        //}
      
        public static void SetCurrentConfig(this IServiceCollection services, IConfiguration config)
        {
            // Remove default IConfiguration Service
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IConfiguration));

            // Add DDS Custom configuration to DI
            if (serviceDescriptor != null)
                services.Remove(serviceDescriptor);

            services.AddSingleton<IConfiguration>(config);

        }
        
        
        
        /// <summary>
        /// Initialize windows authorization services (only for APIs that will use integrated authentication to authorize clients)
        /// <param name="services"></param>
        /// <param name="initConfig"></param>
        /// <param name="environment"></param>
        /// </summary>
        //public static void AddApiWinAuthServices(this IServiceCollection services, IConfiguration initConfig, IHostingEnvironment environment)
        //{
        //    string claimType = initConfig["WindowsAuth:RoleClaimType"];
        //    string claimValue = initConfig["WindowsAuth:RoleClaimValue"];
        //    services.AddAuthorization(options =>
        //    {
        //        options.AddPolicy("WindowsGroupPolicy", policy => policy.RequireClaim(claimType, claimValue));

        //    });
        //    services.AddAuthentication(IISDefaults.AuthenticationScheme);
        //}

        

    }
}
