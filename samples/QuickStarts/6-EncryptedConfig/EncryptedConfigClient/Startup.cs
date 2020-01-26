using ConfigCore;
using ConfigCore.Cryptography;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace EncryptedConfigClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

      
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Data Protection Services using the configuration loaded from DI. 
            // This is required to add Microsoft Data Protection services to DI, and to create a service for ICryptoHelper which performs the cryptography tasks.
            // The configuration must have section ConfigOptions:Cryptography with plain text values for ClientScope, EncValPrefix, and KeyStore.
            // These configuration values are required to initialize encyrption services, so they must be in plain text.
            services.AddDataProtectionServices(Configuration);
           
          // Get the DPAPI Service provider which a dependency of CryptoHelper  
            var serviceProvider = services.BuildServiceProvider();

            //Add ICryptoHelper, with its dependency, to DI
            ICryptoHelper crypto = serviceProvider.GetRequiredService<ICryptoHelper>();
           
            //  DECRYPT the configuration values - Setting values will be decrypted if they begin with the Encrypted Value Prefix (EncValPrefix)
            Configuration.Decrypt(crypto);

            // Use decrypted configuration to initialize other services like data access, authentication, and authorization
            // other services...
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
