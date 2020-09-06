using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.DataProtection;
using ConfigCore.Cryptography;

namespace ConfigCore.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddDataProtectionServices(this IServiceCollection services, IConfiguration config)
        {
            //Create Data Protection Service 
            
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(config["ConfigOptions:Cryptography:KeyStore"]))
                .ProtectKeysWithDpapi(true)
                .SetApplicationName(config["ConfigOptions:Cryptography:ClientScope"]);
            var serviceCollection = new ServiceCollection();

            // Get reference to Data ProtectionProvider Service to register ICryptoHelpert.
            var serviceProvider = services.BuildServiceProvider();
            var proProvider = serviceProvider.GetDataProtectionProvider();

            // Create Crypto Helper service to decrypt secure environment variables 
            services.AddSingleton<ICryptoHelper>(h => new CryptoHelper(proProvider));

        }
             

        public static void SetCurrentConfig(this IServiceCollection services, IConfiguration config)
        {
            // Remove default IConfiguration Service
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IConfiguration));

            // Add DDS Custom configuration to DI
            if (serviceDescriptor != null)
                services.Remove(serviceDescriptor);

            services.AddSingleton<IConfiguration>(config);

        }
        
        
       

        

    }
}
