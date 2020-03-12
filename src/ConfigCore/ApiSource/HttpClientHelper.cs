using ConfigCore.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ConfigCore.ApiSource
{
    public static class HttpClientHelper
    {
        // This method is used when a client is needed, with or without handler. Handles all auth types except API key
        public static HttpClient GetHttpClient(ApiSourceOptions options)
        {
            var baseAddress = new Uri($"{options.ConfigUrl}");
            HttpClient returnClient = null;
            HttpClientHandler handler = null;
            switch (options.AuthType)
            {
                case "Certificate":
                    X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                    certStore.Open(OpenFlags.ReadOnly);
                    X509Certificate2Collection certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, options.AuthSecret, false);
                    if (certCollection.Count > 0)
                    {
                        X509Certificate2 cert = certCollection[0];
                        handler = new HttpClientHandler();
                        handler.ClientCertificates.Add(cert);
                    }
                    else
                        throw new Exception($"Certificate not found for thumbprint: {options.AuthSecret}");
            
                   
                    returnClient = new HttpClient(handler)
                    {
                        BaseAddress = baseAddress
                    };
                    break;

                case "ApiKey":
                    returnClient = new HttpClient() { BaseAddress = baseAddress };
                    break;

                case "JwtBearer":
                    returnClient = new HttpClient() { BaseAddress = baseAddress };
                    returnClient.SetBearerToken(GetBearerToken(options.JWTBearerOptions));
                    
                    break;
                case "Windows":
                     handler = new HttpClientHandler
                    {
                        Credentials = CredentialCache.DefaultNetworkCredentials
                    };
                    returnClient = new HttpClient(handler)
                    {
                        BaseAddress = baseAddress
                    };
                    break;

                case "Anon":
                default:

                    returnClient = new HttpClient() { BaseAddress = baseAddress };
                    break;
            }

            return returnClient;

        }
        
        //This method is used to add the API Key header using a request 
        public static HttpRequestMessage GetHttpRequest(ApiSourceOptions apiOptions)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, apiOptions.ConfigUrl);

            switch (apiOptions.AuthType)
            {
                case "ApiKey":
                    request.Headers.Add("X-API-Key", apiOptions.AuthSecret);
                    break;
                case "Windows":
                case "Certificate":
                case "Anon":
                case "Bearer":
                    break;
            }

            return request;

        }

         static string GetBearerToken(JWTBearerOptions bConfig) {
            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync(bConfig.Authority).Result;
            if (disco.IsError)
            {
                throw new Exception($"Get Discovery Document Error: {disco.Exception.Message}");
            }

            var tokenResponse =  client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = bConfig.ClientId,
                ClientSecret = bConfig.ClientSecret,
                Scope = bConfig.Scope
            }).Result;

            if (tokenResponse.IsError)
            {
                throw new Exception($"Get Token Error: {tokenResponse.Error}");
               
            }

            return tokenResponse.AccessToken;
        }

    }
}
