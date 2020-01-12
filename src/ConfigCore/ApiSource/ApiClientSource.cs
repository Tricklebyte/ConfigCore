
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


        /// <summary>
        /// Overload with all options
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configUrlVar">Name of Host Environment Variable holding the Configuration URL</param>
        /// <param name="authSecretVar">Certificate Thumbprint, or APIKey - not needed for Windows</param>
        /// <param name="optional">Prevent program from loading if the APIProvider does not build successfully (default: 'false')</param>
        /// <param name="authType">Authentication type: Windows, Certificate, ApiKey, or Anon (default: Certificate)</param>
        /// <param name="appId">Filter search string for application Id - (default: Assembly Name)</param>
        public ApiClientSource(IConfigurationBuilder builder, string configUrlVar, string authType = null, string authSecretVar = null, string appId = null, bool optional = false)
        {
            _optional = optional;
            try
            {
                //Create the apiOptions object
                ApiSourceOptions apiOptions = new ApiSourceOptions(configUrlVar, authType,authSecretVar,  appId, _optional);

                //Initialize the correct HTTP client for the Authentication type
                _client = HttpClientHelper.GetHttpClient(apiOptions);
                _request = HttpClientHelper.GetHttpRequest(apiOptions);

            }
            catch (Exception e)
            {
                if (!optional)
                    throw e;
                return;
            }
        }

        public ApiClientSource(IConfigurationBuilder builder,  IConfiguration config, bool optional)
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
                return;
            }
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        { 
            //TODO pass httpclient
            return new ApiClientProvider(_client, _request,_optional);
        }

        private void SetClient()
        {

        }
    }
}
