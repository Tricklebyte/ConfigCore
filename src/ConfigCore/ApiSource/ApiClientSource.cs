
using ConfigCore;
using Microsoft.Extensions.Hosting;
using ConfigCore.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ConfigCore.ApiSource
{
    public class ApiClientSource : IConfigurationSource
    {
        //   IWebHostEnvironment _env;
        IConfiguration _config;
        bool _optional;
        HttpClient _client;
        HttpRequestMessage _request;
        string _apiClientSourceError;

        /// <summary>
        /// All auth types except JWTBearer
        /// Route Parameters
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configUrlVar">Name of Host Environment Variable holding the Configuration URL</param>
        /// <param name="authType">Authentication type: Windows, Certificate, ApiKey, or Anon (default: Windows)</param>
        /// <param name="authSecretVar">Certificate Thumbprint, or APIKey - not needed for Windows</param>
        /// <param name="rParams">Route paramters to be added to the ConfigURL. When no Query or Route parameters are present the default route parameter will be applied: {"AssemblyName"}</param>
        /// <param name="optional">Prevent program from loading if the APIProvider does not build successfully (default: 'false')</param>

        public ApiClientSource(IConfigurationBuilder builder, string configUrlVar, string authType = null, string authSecretVar = null, string[] rParams = null, bool optional = false)
        {
            _optional = optional;
            try
            {
                //Create the apiOptions object
                ApiSourceOptions apiOptions = new ApiSourceOptions(configUrlVar, authType, authSecretVar, rParams, _optional);

                //Initialize the correct HTTP client for the Authentication type
                _client = HttpClientHelper.GetHttpClient(apiOptions);
                _request = HttpClientHelper.GetHttpRequest(apiOptions);

            }
            catch (Exception e)
            {
                if (!optional)
                {
                    throw e;
                }
                else
                    _apiClientSourceError = e.Message;
                return;
            }
        }


        /// <summary>
        /// All auth types except JWTBearer
        /// Query String Parameters
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configUrlVar">Name of Host Environment Variable holding the Configuration URL</param>
        /// <param name="authType"></param>
        /// <param name="authSecretVar"></param>
        /// <param name="qParams"></param>
        /// <param name="optional"></param>
        public ApiClientSource(IConfigurationBuilder builder, string configUrlVar, string authType, string authSecretVar, Dictionary<string, string> qParams, bool optional)
        {
            _optional = optional;

            try
            {
                //Create the apiOptions object
                ApiSourceOptions apiOptions = new ApiSourceOptions(configUrlVar, authType, authSecretVar, qParams, optional);
                //Initialize the correct HTTP client for the Authentication type
                _client = HttpClientHelper.GetHttpClient(apiOptions);
                _request = HttpClientHelper.GetHttpRequest(apiOptions);
            }
            catch (Exception e)
            {
                if (!optional)
                    throw e;
                else
                    _apiClientSourceError = e.Message;
            }
        }



        // JwtBearer Authentication with Route Params
        public ApiClientSource(IConfigurationBuilder builder, string configUrlVar, string authority, string clientId, string clientSecret, string scope, string[] routeParams = null, bool optional = false)
        {
            _optional = optional;
            try
            {
                JWTBearerOptions bConfig = new JWTBearerOptions(authority, clientId, clientSecret, scope);
                //Create the apiOptions object
                ApiSourceOptions apiOptions = new ApiSourceOptions(configUrlVar, bConfig, routeParams, optional);
                //Initialize the correct HTTP client for the Authentication type
                _client = HttpClientHelper.GetHttpClient(apiOptions);
                _request = HttpClientHelper.GetHttpRequest(apiOptions);
            }
            catch (Exception e)
            {
                if (!optional)
                    throw e;
                else
                    _apiClientSourceError = e.Message;
            }
        }
       
       
        // JwtBearer Authentication with Query String params
        public ApiClientSource(IConfigurationBuilder builder, string configUrlVar, string authority, string clientId, string clientSecret, string scope, Dictionary<string, string> qParams,bool optional =false)
        {
            _optional = optional;

            try
            {
                JWTBearerOptions bConfig = new JWTBearerOptions(authority, clientId, clientSecret, scope);
                //Create the apiOptions object
                ApiSourceOptions apiOptions = new ApiSourceOptions(configUrlVar, bConfig, qParams, optional);
                //Initialize the correct HTTP client for the Authentication type
                _client = HttpClientHelper.GetHttpClient(apiOptions);
                _request = HttpClientHelper.GetHttpRequest(apiOptions);
            }
            catch (Exception e)
            {
                if (!optional)
                    throw e;
                else
                    _apiClientSourceError = e.Message;
            }
        }


        //Configuration Object Parameter - supports all authentication and parameter types - Requires configuration section ConfigOptions:ApiSource
        public ApiClientSource(IConfigurationBuilder builder, IConfiguration config, bool optional = false)
        {
            _optional = optional;
            try
            {
                //Create the apiOptions object
                ApiSourceOptions apiOptions = new ApiSourceOptions(config, optional);
                //Initialize the correct HTTP client for the Authentication type
                _client = HttpClientHelper.GetHttpClient(apiOptions);
                _request = HttpClientHelper.GetHttpRequest(apiOptions);
            }
            catch (Exception e)
            {
                if (!optional)
                    throw e;
                else
                    _apiClientSourceError = e.Message;
            }
        }


        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ApiClientProvider(_client, _request, _optional,_apiClientSourceError);
        }


    }


}
