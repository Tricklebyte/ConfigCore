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
        #region DB Config Source
        // Connvar
        //DbSource with with Environment variable parameters. Does not require pre-built configuration object. 


        // Convar optional
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, string connEnvVar, bool optional=false)
        {
            return builder.Add(new DbConfigSource(builder, connEnvVar, null, 0, optional));
        }


        // Convar, appId, optional
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, string connEnvVar, string appId, bool optional=false)
        {
            return builder.Add(new DbConfigSource(builder, connEnvVar, appId, 0, optional));
        }


        // Convar, sqlTimeout,optional
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, string connEnvVar, int sqlCommandTimeout, bool optional=false)
        {
            return builder.Add(new DbConfigSource(builder, connEnvVar, null, sqlCommandTimeout, optional));
        }


        // Convar, appId,sqlTimeout,optional
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, string connEnvVar, string appId, int sqlCommandTimeout, bool optional=false)
        {
            return builder.Add(new DbConfigSource(builder, connEnvVar, appId, sqlCommandTimeout, optional));
        }



        //DbSource Configuration object parameter. Uses configuration settings to control the behaviour of DB Source and Provider.
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, IConfiguration config, bool optional = false)
        {
            return builder.Add(new DbConfigSource(builder, config, optional));
        }

        #endregion

        #region API ConfigSource - OPT Params - ANON and WINDOWS AUTH



        //URL,OPTIONAL
        //API Source Override with Single parameter for ConfigURL - used with Windows Auth and Default AppId
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, bool optional=false)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, null, null, "", optional));
        }


        //URL,APPID,OPTIONAL
        //API Source Override with parameters for ConfigURL and App Id- used with Windows Auth and custom App Id 
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string appId, bool optional=false)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, null, null, appId, optional));
        }
        #endregion

        #region Api Config Source - OPT Params - CERTIFICATE and API AUTH

        //URL,AUTHT,AUTHS,OPTIONAL
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string authType, string authSecretVar, bool optional = false)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, authType, authSecretVar, "", optional));
        }

        //URL,AUTHT,AUTHS,APPID,OPTIONAL
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string authType, string authSecretVar, string appId, bool optional=false)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, authType, authSecretVar, appId, optional));
        }



        #endregion

        #region Api Config Source - OPT Params - BEARER TOKEN AUTH
       

        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, BearerConfig bConfig, bool optional =false)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, bConfig, "",optional));
        }


        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, BearerConfig bConfig, string appId, bool optional = false)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, bConfig, appId, optional));
        }


        #endregion

        #region Api Config Source Query String Params  
       
        //URL,PARAMS,OPTIONAL
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, Dictionary<string, string> qParams, bool optional = false)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, null, null, qParams, optional));
        }

       
        //URL,AUTHT,ATHS,PARAMS,OPTIONAL
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string authType, string authSecretVar, Dictionary<string, string> qParams, bool optional=false)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, authType, authSecretVar, qParams, optional));
        }
        #endregion




        //Api Source with Configuration object parameter. Uses configuration settings 
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, IConfiguration config, bool optional = false)
        {
            return builder.Add(new ApiClientSource(builder, config, optional));
        }


    }
}
