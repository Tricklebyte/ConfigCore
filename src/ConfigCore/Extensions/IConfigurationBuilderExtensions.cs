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
        // Connvar
        //DbSource with with Environment variable parameters. Does not require pre-built configuration object. 
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, string connEnvVar)
        {
            return builder.Add(new DbConfigSource(builder, connEnvVar, null, 0, false));
        }

        // Convar optional
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, string connEnvVar, bool optional)
        {
            return builder.Add(new DbConfigSource(builder, connEnvVar, null, 0, optional));
        }

        // Convar,appId
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, string connEnvVar, string appId)
        {
            return builder.Add(new DbConfigSource(builder, connEnvVar, appId, 0, false));
        }

        // Convar, appId, optional
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, string connEnvVar, string appId, bool optional)
        {
            return builder.Add(new DbConfigSource(builder, connEnvVar, appId,0,optional));
        }

        // Convar, sqlTimeout
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, string connEnvVar,  int sqlCommandTimeout)
        {
            return builder.Add(new DbConfigSource(builder, connEnvVar, null, sqlCommandTimeout, false));
        }

        // Convar, sqlTimeout,optional
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, string connEnvVar,  int sqlCommandTimeout , bool optional)
        {
            return builder.Add(new DbConfigSource(builder, connEnvVar, null, sqlCommandTimeout, optional));
        }
        
        // Convar, appId,sqlTimeout
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, string connEnvVar, string appId, int sqlCommandTimeout)
        {
            return builder.Add(new DbConfigSource(builder, connEnvVar, appId, sqlCommandTimeout, false));
        }

        // Convar, appId,sqlTimeout,optional
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, string connEnvVar, string appId, int sqlCommandTimeout, bool optional)
        {
            return builder.Add(new DbConfigSource(builder, connEnvVar, appId, sqlCommandTimeout, optional));
        }



        //DbSource Configuration object parameter. Uses configuration settings to control the behaviour of DB Source and Provider.
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, IConfiguration config, bool optional = false)
        {
            return builder.Add(new DbConfigSource(builder, config, optional));
        }


        //#################################################################### 




        //URL
        //API Source Override with Single parameter for ConfigURL - used with Windows Auth and Default AppId
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, null, null, null, false));
        }

        //URL,OPTIONAL
        //API Source Override with Single parameter for ConfigURL - used with Windows Auth and Default AppId
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, bool optional)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, null, null, null, optional));
        }

        //URL,APPID
        //API Source Override with parameters for ConfigURL and App Id- used with Windows Auth and custom App Id 
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string appId)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, null, null, appId, false));
        }

        //URL,APPID,OPTIONAL
        //API Source Override with parameters for ConfigURL and App Id- used with Windows Auth and custom App Id 
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string appId, bool optional)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, null, null, appId, optional));
        }

        //URL,AUTHT,AUTHS
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string authType, string authSecretVar)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, authType, authSecretVar, null, false));
        }

        //URL,AUTHT,AUTHS,OPTIONAL
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string authType, string authSecretVar, bool optional)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, authType, authSecretVar, null, optional));
        }

        //URL,AUTHT,AUTHS,APPID
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string authType, string authSecretVar, string appId)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, authType, authSecretVar, appId, false));
        }
        //URL,AUTHT,AUTHS,APPID,OPTIONAL
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string authType, string authSecretVar, string appId, bool optional)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, authType, authSecretVar, appId, optional));
        }
















        //Api Source with Configuration object parameter. Uses configuration settings 
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, IConfiguration config, bool optional = false)
        {
            return builder.Add(new ApiClientSource(builder, config, optional));
        }


    }
}
