
using ConfigCore;
using ConfigCore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ConfigCore.ApiSource
{
    public class ApiConfigSource : IConfigurationSource
    {
        IHostingEnvironment _env;
        IConfiguration _config;
        bool _optional;
        HttpClient _client;


        public ApiConfigSource(IConfigurationBuilder builder, string configUrlVar, bool optional = false)
        {
            _optional = optional;

            try
            {
            //Create the apiOptions object
            ApiSourceOptions apiOptions = new ApiSourceOptions(configUrlVar,_optional);
            
            //Initialize the correct HTTP client for the Authentication type
                _client = HttpClientHelper.GetHttpClient(apiOptions);
             }
            catch(Exception e)
            {
                if (!optional)
                    throw e;
                return;
            }
        }

        public ApiConfigSource(IConfigurationBuilder builder, string configUrlVar, string appId,bool optional = false)
        {
            _optional = optional;
            try { 
            //Create the apiOptions object
            ApiSourceOptions apiOptions = new ApiSourceOptions(configUrlVar, appId, optional);
            //Initialize the correct HTTP client for the Authentication type
            _client = HttpClientHelper.GetHttpClient(apiOptions);
            }
            catch (Exception e)
            {
                if (!optional)
                    throw e;
                return;
            }

          
        }
        public ApiConfigSource(IConfigurationBuilder builder,  IConfiguration config, bool optional)
        {
          _optional = optional;
            try
            {
                //Create the apiOptions object
                ApiSourceOptions apiOptions = new ApiSourceOptions(config, optional);
                //Initialize the correct HTTP client for the Authentication type
                _client = HttpClientHelper.GetHttpClient(apiOptions);
            }
            catch (Exception e)
            {
                if (!optional)
                    throw e;
                return;
            }
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        { 
            //TODO pass httpclient
            return new ApiConfigProvider(_client,_optional);
        }

        private void SetClient()
        {

        }
    }
}
