using ConfigCore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using ConfigCore.DbSource;
using ConfigCore.ApiSource;

namespace ConfigCore.Extensions
{
    public static class IConfigurationBuilderExtensions
    {

        //DbSource with with Environment variable parameters. Does not require pre-built configuration object. 
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, string connEnvVar,string appId = null, int sqlCommandTimeout = 0,  bool optional = false)
        {
            return builder.Add(new DbConfigSource(builder, connEnvVar,appId, sqlCommandTimeout,  optional));
        }

        //DbSource Configuration object parameter. Uses configuration settings to control the behaviour of DB Source and Provider.
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, IConfiguration config, bool optional = false)
        {
            return builder.Add(new DbConfigSource(builder, config,  optional));
        }
		
		//ApiSource with Environment variable parameters and all options. Does not require pre-built configuration object. 
		public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string authType,   string authSecretVar,  string appId = null, bool optional = false)
        {
            return builder.Add(new ApiClientSource(builder,urlKeyVar, authType,authSecretVar,appId, optional));
        }

        //API Source Override with Single parameter for ConfigURL - used with Windows Auth and Default AppId
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar,  bool optional = false)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, null, null, null, optional));
        }

        //API Source Override with parameters for ConfigURL and App Id- used with Windows Auth and custom App Id 
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string appId, bool optional = false)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, null, null, appId, optional));
        }



        //Api Source with Configuration object parameter. Uses configuration settings 
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, IConfiguration config, bool optional = false)
        {
            return builder.Add(new ApiClientSource(builder, config,  optional));
        }


    }
}
