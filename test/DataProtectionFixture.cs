using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConfigCore.Tests
{
   public class DataProtectionFixture
    {
        public static IConfiguration Configuration;
        public static IDataProtectionProvider Provider;
        public DataProtectionFixture()
        {
            //setup config
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: true);
            Configuration = builder.Build();

            //set up data protection provider
            Provider = DataProtectionProvider.Create(
             new DirectoryInfo(Configuration["ConfigOptions:Cryptography:KeyStore"]),
            configuration =>
            {
                configuration.SetApplicationName(Configuration["ConfigOptions:Cryptography:ClientScope"]);
                configuration.ProtectKeysWithDpapi();
            }
             );
        }

    }
}
