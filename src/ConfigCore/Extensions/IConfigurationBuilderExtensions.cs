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
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, string connEnvVar, bool optional = false)
        {
            return builder.Add(new DbConfigSource(builder, connEnvVar, null, 0, optional));
        }


        // Convar, appId, optional
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, string connEnvVar, string appId, bool optional = false)
        {
            return builder.Add(new DbConfigSource(builder, connEnvVar, appId, 0, optional));
        }


        // Convar, sqlTimeout,optional
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, string connEnvVar, int sqlCommandTimeout, bool optional = false)
        {
            return builder.Add(new DbConfigSource(builder, connEnvVar, null, sqlCommandTimeout, optional));
        }


        // Convar, appId,sqlTimeout,optional
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, string connEnvVar, string appId, int sqlCommandTimeout, bool optional = false)
        {
            return builder.Add(new DbConfigSource(builder, connEnvVar, appId, sqlCommandTimeout, optional));
        }



        //DbSource Configuration object parameter. Uses configuration settings to control the behaviour of DB Source and Provider.
        public static IConfigurationBuilder AddDbSource(this IConfigurationBuilder builder, IConfiguration config, bool optional = false)
        {
            return builder.Add(new DbConfigSource(builder, config, optional));
        }

        #endregion

        #region API ConfigSource -Default AUTH (WinAnon) 

        //Default Route Params
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, bool optional = false)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, null, null, new string[] { "" }, optional));
        }

        //Specified Route Params
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string[] routeParams, bool optional = false)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, null, null, routeParams, optional));
        }


        //Specified Query Params
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, Dictionary<string, string> qParams, bool optional = false)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, null, null, qParams, optional));
        }

        #endregion

        #region Api Config Source - CERTIFICATE and API AUTH 
        //Default Route Params 
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string authType, string authSecretVar, bool optional = false)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, authType, authSecretVar, new string[] { "" }, optional));
        }

        //Specified Route Params
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string authType, string authSecretVar, string[] routeParams, bool optional = false)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, authType, authSecretVar, routeParams, optional));
        }

        //Specified Query Params
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string authType, string authSecretVar, Dictionary<string, string> qParams, bool optional = false)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, authType, authSecretVar, qParams, optional));
        }

        #endregion

        #region Api Config Source - JWTBEARER AUTH 

        //- Default Route Params 
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string authority, string clientId, string clientSecret, string scope, bool optional = false)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, authority, clientId, clientSecret, scope, new string[] { "" }, optional));
        }

        //Specified Route Params
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string authority, string clientId, string clientSecret, string scope, string[] routeParams, bool optional = false)
        {
            return builder.Add(new ApiClientSource(builder, urlKeyVar, authority, clientId, clientSecret, scope, routeParams, optional));
        }

        //JWTBearer , Query Parameters
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, string urlKeyVar, string authority, string clientId, string clientSecret, string scope, Dictionary<string, string> qParams, bool optional = false)
        {

            return builder.Add(new ApiClientSource(builder, urlKeyVar, authority, clientId, clientSecret, scope, qParams, optional));
        }

        #endregion

        #region Api Config Source - IConfiguration Object
        //Api Source with Configuration object parameter. Uses configuration settings 
        public static IConfigurationBuilder AddApiSource(this IConfigurationBuilder builder, IConfiguration config, bool optional = false)
        {
            return builder.Add(new ApiClientSource(builder, config, optional));
        }

        #endregion
    }
}
