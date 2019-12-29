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
		
		//ApiSource with Environment variable parameters. Does not require pre-built configuration object. 
		public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder,  string urlKeyVar, string authSecretVar = null, string authType =null, string appId = null, bool optional = false)
        {
            return builder.Add(new ApiConfigSource(builder,urlKeyVar, authSecretVar,authType,appId, optional));
        }

        //Api Source with Configuration object parameter. Uses configuration settings to control the behaviour of DB Source and Provider.
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, IConfiguration config, bool optional = false)
        {
            return builder.Add(new ApiConfigSource(builder, config,  optional));
        }


    }
}
