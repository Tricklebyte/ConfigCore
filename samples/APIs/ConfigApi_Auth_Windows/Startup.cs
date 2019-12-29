using ConfigCore;
using ConfigCore.Data;
using ConfigCore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ConfigApi_Auth_Windows
{
    public class Startup
    {
        public IHostingEnvironment Environment;
        public IConfiguration Configuration { get; set; }
        
        public Startup(IHostingEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Claim type is Microsoft claim type for AD Secruty Group Identifier
            string claimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid";

            // Claim Value is the SID of the allowed Windows Group
            // This example is set to the well-known SID for the group BUILTIN\Users. 
            //   For use in Production, change this to the sid of an actual security group.
            string claimValue = "S-1-5-32-545";

            //Add authorization, create policy to check SID of Windows Security Group.
            // Note - policy must also be applied in the controller
            services.AddAuthorization(options =>
            {
                options.AddPolicy("BuiltinUser", policy =>
                                            policy.RequireClaim(claimType, claimValue));
            });
            services.AddAuthentication(IISDefaults.AuthenticationScheme);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
