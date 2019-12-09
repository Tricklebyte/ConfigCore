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
        
		//DbSource
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, string connEnvVar,  bool optional = false, int sqlCommandTimeout = 0)
        {
            return builder.Add(new DbConfigSource(builder,connEnvVar,optional,sqlCommandTimeout));
        }
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, string connEnvVar, string appId, bool optional = false, int sqlCommandTimeout = 0)
        {
            return builder.Add(new DbConfigSource(builder, connEnvVar, appId, optional, sqlCommandTimeout));
        }
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, IConfiguration config, bool optional = false)
        {
            return builder.Add(new DbConfigSource(builder, config,  optional));
        }
		
		//ApiSource
		public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar,  bool optional = false)
        {
            return builder.Add(new ApiConfigSource(builder,urlKeyVar,optional));
        }
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string appId, bool optional = false)
        {
            return builder.Add(new ApiConfigSource(builder, urlKeyVar, appId, optional));
        }
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, IConfiguration config, bool optional = false)
        {
            return builder.Add(new ApiConfigSource(builder, config,  optional));
        }


    }
}
