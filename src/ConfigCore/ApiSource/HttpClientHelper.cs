using ConfigCore.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ConfigCore.ApiSource
{
    public static class HttpClientHelper
    {
        public static HttpClient GetHttpClient(ApiSourceOptions options)
        {
            var baseAddress = new Uri($"{options.ConfigSettingUrl}");
            HttpClient returnClient = null;
            switch (options.AuthType)
            {
                case "Windows":
                    var handler = new HttpClientHandler
                    {
                        Credentials = CredentialCache.DefaultNetworkCredentials
                    };
                    returnClient = new HttpClient(handler)
                    {
                        BaseAddress = baseAddress
                    };
                    break;

                case "None":
                default:
                   
                    returnClient = new HttpClient() { BaseAddress = baseAddress };
                    break;
            }

            return returnClient;

        }
    }
}
