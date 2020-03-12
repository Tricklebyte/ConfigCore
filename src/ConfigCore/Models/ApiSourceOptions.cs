using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using static ConfigCore.Models.Enums;

namespace ConfigCore.Models
{
    public class ApiSourceOptions
    {
        public string AuthType { get; set; }
        public string AuthSecret { get; set; }

        public string ConfigUrlKey { get; set; }
        public string ConfigUrl { get; set; }

        public string ConfigSourceError { get; set; }
        public JWTBearerOptions JWTBearerOptions;

        bool _optional;
        string[] _routeParams;
        Dictionary<string, string> _queryParams;


        /// <summary>
        /// // All Auth Types except JWTBearer, route parameters
        /// </summary>
        /// <param name="configUrlVar">Name of Environment Variable containing the full URL of the Configuration API's get action (without parameters)</param>
        /// <param name="authSecretVar">Name of Environment Variable conataing the authorization secret</param>
        /// <param name="optional">When optional is false, an exception will be thrown if the client is unable obtain configuration data from the API</param>
        /// <param name="authType"></param>
        /// <param name="appId"></param>
        public ApiSourceOptions(string configUrlVar, string authType, string authSecretVar, string[] rParams, bool optional)
        {
            _optional = optional;
            AuthType = authType;
            _routeParams = rParams;


            SetDefaults();
            SetConfigUrlFromEnvVar(configUrlVar);
            SetRouteParametersWithDefault();
            SetAuthSecretFromEnvVar(authSecretVar);
        }

        // All Auth Types except JWTB, query string parameters
        public ApiSourceOptions(string configUrlVar, string authType, string authSecretVar, Dictionary<string, string> qParams, bool optional)
        {
            _optional = optional;
            AuthType = authType;
            _queryParams = qParams;
            SetDefaults();
            SetConfigUrlFromEnvVar(configUrlVar);
            SetQueryParameters();
            SetAuthSecretFromEnvVar(authSecretVar);
        }


        // JwtBearer Authorization, Route Parameters
        public ApiSourceOptions(string configUrlVar, JWTBearerOptions bConfig, string[] rParams, bool optional)
        {
            _optional = optional;
            JWTBearerOptions = bConfig;
            AuthType = "JwtBearer";
            _routeParams = rParams;
            SetDefaults();
            SetConfigUrlFromEnvVar(configUrlVar);
            SetRouteParametersWithDefault();
            ValidateBearerConfig();
        }

        // JwtBearer Authorization, Query String Parameters
        public ApiSourceOptions(string configUrlVar, JWTBearerOptions bConfig, Dictionary<string, string> qParams, bool optional)
        {
            _optional = optional;
            _queryParams = qParams;

            JWTBearerOptions = bConfig;
            AuthType = "JwtBearer";

            SetDefaults();
            SetConfigUrlFromEnvVar(configUrlVar);
            SetQueryParameters();

            ValidateBearerConfig();

        }




        /// <summary>
        /// Accepts Configuration as parameter. Used to override default api client settings 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="optional"></param>
        public ApiSourceOptions(IConfiguration config, bool optional)
        {
            //Bearer Auth
            //TODO Query Params
            IConfigurationSection apiSection = config.GetSection("ConfigOptions:ApiSource");
            if (!apiSection.Exists())
            {
                throw new Exception("ApiSource section not found in configuration");
            }

            ConfigUrlKey = apiSection["ConfigUrlKey"];
            ConfigUrl = config[ConfigUrlKey];
            AuthType = apiSection["AuthType"];

            // SET and validate Auth Secret for Cert and Api Key
            if (AuthType == "Certificate" || AuthType == "ApiKey")
            {
                AuthSecret = apiSection["AuthSecret"];
                if (string.IsNullOrEmpty(AuthSecret))
                {
                    throw new Exception($"Authentication secret not found in configuration setting - 'ConfigOptions:ApiSource:AuthSecret'");
                }
            }

            // Set and validate IDP options for JWTBearer
            if (AuthType == "JwtBearer")
            {
                {
                    JWTBearerOptions = new JWTBearerOptions(
                    apiSection["Authority"],
                    apiSection["ClientId"],
                    apiSection["ClientSecret"],
                    apiSection["Scope"]
                    );

                    ValidateBearerConfig();
                }
            }

            //Add defaults for any required values that were not supplied
            SetDefaults();

            //Set Full Configuration API URI including action and Parmater
            ConfigUrl = config[ConfigUrlKey];

            if (string.IsNullOrEmpty(ConfigUrl))
                throw new Exception($"Configuration setting not found: '{ConfigUrlKey}'");



            //Set QueryParameter member from configuration section first before checking if the default Route Parameter should  be added
            //   IConfigurationSection qParamSection = config.GetSection("ConfigOptions:ApiSource:QueryParams");
            // if (qParamSection != null)
            _queryParams = config.GetSection("ConfigOptions:ApiSource:QueryParams")?.GetChildren().ToDictionary(x => x.Key, x => x.Value);

            //  var children = qParamSection?.GetChildren();
            // IF the query parameters section has values, convert to dictionary for SetQueryParameters Method
            //if (_queryParams != null)
            //{
            //    // if query paramaters have been supplied, add them as query string parameters
            //    _queryParams = new Dictionary<string, string>();
            //    foreach (var child in children)
            //    {
            //        _queryParams.Add(child.Key, child.Value);
            //    }
            SetQueryParameters();
            //}

            //Set RouteParameter member from configuration section
            IConfigurationSection rParamSection = config.GetSection("ConfigOptions:ApiSource:RouteParams");

            //if (rParamSection != null)
            //{
            _routeParams = rParamSection?.Get<string[]>();

            //}
            // Set Route Parameters if present, or default route parameter if needed
            SetRouteParametersWithDefault();

        }




        public void ValidateBearerConfig()
        {
            string errMsg = "";
            if (string.IsNullOrEmpty(JWTBearerOptions.Authority))
            {
                errMsg += "JwtBearer Authority URL not found\n";
            }
            if (string.IsNullOrEmpty(JWTBearerOptions.ClientId))
            {
                errMsg = "JwtBearer ClientId not found\n";
            }
            if (string.IsNullOrEmpty(JWTBearerOptions.ClientSecret))
            {
                errMsg = "JwtBearer ClientSecret not found\n";
            }
            if (string.IsNullOrEmpty(JWTBearerOptions.Scope))
            {
                errMsg = "JwtBearer Scope not found";
            }
            if (!string.IsNullOrEmpty(errMsg))
                throw new Exception(errMsg);
        }

        // Configuration API URL - by Environment Variable name
        // Use the Environment Variable name parameter to get the URL of the ConfigurationAPI.
        public void SetConfigUrlFromEnvVar(string configUrlVar)
        {
            if (string.IsNullOrEmpty(configUrlVar))
            {
                throw new Exception($"Null argument exception for parameter configUrlVar");
            }

            ConfigUrl = Environment.GetEnvironmentVariable(configUrlVar);

            if (ConfigUrl == null)
                throw new Exception($"Unable to create Api Source Options, Environment Variable: '{configUrlVar}' not found or is empty.");

        }

        public void SetAuthSecretFromEnvVar(string authSecretVar)
        {
            // If the authentication type requires a secret, verify the secret is there
            if (AuthType == "ApiKey" || AuthType == "Certificate")
            {
                if (string.IsNullOrEmpty(authSecretVar))
                    throw new Exception("Unable to create ApiSourceOptions, Auth secret environment variable name not specified");

                AuthSecret = Environment.GetEnvironmentVariable(authSecretVar);

                if (AuthSecret == null)
                    throw new Exception($"Environment Variable: '{authSecretVar}' was not found.");
            }
        }





        private void SetDefaults()
        {
            if (string.IsNullOrEmpty(ConfigUrlKey))
                ConfigUrlKey = ApiDefault.ConfigUrlKey;

            if (string.IsNullOrEmpty(AuthType))
                AuthType = ApiDefault.AuthType;
        }


        // Add Query String Parameters
        public void SetQueryParameters()
        {
            if (_queryParams == null || _queryParams.Count == 0)
                return;

            var newUrl = new Uri(QueryHelpers.AddQueryString(ConfigUrl, _queryParams));
            ConfigUrl = newUrl.ToString();
        }

        // Add Route Parameter Values to ConfigURL in order of array
        public void SetRouteParametersWithDefault()
        {
            // if route parameters have been supplied, add them as route parameters

            // when the default appId is being used, there will be one parameter with zero length.
            // Assign the default value before adding the URI
            if (_routeParams == null && (_queryParams == null || _queryParams?.Count == 0))
                _routeParams = new string[] { "" };

            // Add default appId route if there are no query parameters and there is the single empty route parameter (indicating default route)
            if ((_queryParams == null || _queryParams?.Count == 0) && _routeParams.Length == 1 && _routeParams[0] == "")
                _routeParams[0] = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;

            //Add route params (supplied or default) to the Configuration URL
            if (_routeParams != null && _routeParams.Length > 0)
            {
                for (int i = 0; i < _routeParams.Length; i++)
                {
                    if (_routeParams[i].Length > 0)
                    {
                        ConfigUrl = ConfigUrl.TrimEnd('/') + "/" + _routeParams[i];
                    }
                }
            }
        }
    }
}
