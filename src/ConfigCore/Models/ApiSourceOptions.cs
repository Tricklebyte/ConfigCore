using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
        public string AppId { get; set; }
        public BearerConfig BearerConfig { get; set; }

        bool _optional;
      

        //Accepts parameters for basic configuration values, Add the ApiSource to the IConfigurationBuilder in a single step and does not require a pre-built configuration object as a paramter.
      /// <summary>
      /// 
      /// </summary>
      /// <param name="configUrlVar">Name of Environment Variable containing the full URL of the Configuration API's get action (without parameters)</param>
      /// <param name="authSecretVar"></param>
      /// <param name="optional"></param>
      /// <param name="authType"></param>
      /// <param name="appId"></param>
        public ApiSourceOptions(string configUrlVar,string authType,  string authSecretVar, string appId, bool optional)
        {
            _optional = optional;
            AuthType = authType;
            AppId = appId;
            SetDefaults();
            SetConfigUrlFromEnvVar(configUrlVar);
            string[] rparams = { AppId};
            SetRouteParameters(rparams);
            SetAuthSecretFromEnvVar(authSecretVar);
        }

        public ApiSourceOptions (string configUrlVar,string authType,  string authSecretVar, Dictionary<string,string> qParams, bool optional) {
            _optional = optional;
            AuthType = authType;
        
            SetDefaults();
            SetConfigUrlFromEnvVar(configUrlVar);
           
            SetQueryParameters(qParams);

            SetAuthSecretFromEnvVar(authSecretVar);
        }

        // Bearer Token Config Options parameter
        public ApiSourceOptions(string configUrlVar, BearerConfig bConfig, string appId, bool optional) {
       
            _optional = optional;
            BearerConfig = bConfig;
            AuthType = "Bearer";
            AppId = appId;
            SetDefaults();
            SetConfigUrlFromEnvVar(configUrlVar);
            string[] rparams = { AppId };
            SetRouteParameters(rparams);
            ValidateBearerConfig();
            //Check for valid Bearer token settings
        }

        public ApiSourceOptions(string configUrlVar, BearerConfig bConfig, Dictionary<string, string> qParams, bool optional) { }

        /// <summary>
        /// Accepts Configuration as parameter. Used to override default api client settings 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="optional"></param>
        public ApiSourceOptions(IConfiguration config, bool optional)
        {
            IConfigurationSection apiSection = config.GetSection("ConfigOptions:ApiSource");
            if (!apiSection.Exists())
            {
                    throw new Exception("ApiSource section not found in configuration");
            }
            // assign user setting options
            ConfigUrlKey = apiSection["ConfigUrlKey"];
            ConfigUrl = config[ConfigUrlKey];
            AuthType = apiSection["AuthType"];
            AuthSecret = apiSection["AuthSecret"];
            AppId = apiSection["AppId"];
         
            
            //Add defaults for any required values that were not supplied
            SetDefaults();

            //Set Full Configuration API URL including action and Parmater
            ConfigUrl = config[ConfigUrlKey];
            SetRouteParameters(new string[] { AppId });
            
            if (string.IsNullOrEmpty(ConfigUrl))
                throw new Exception($"Configuration setting not found: '{ConfigUrlKey}'");
          
          
            // Check if Authorization secret is required and present
            if (AuthType == "Certificate" || AuthType == "ApiKey")
            {
                if (string.IsNullOrEmpty(AuthSecret))
                {
                    throw new Exception($"Authentication secret not found in configuration setting - 'ConfigOptions:ApiSource:AuthSecret'");
                }
            }

        }


        public void ValidateBearerConfig()
        {
            string ErrMsg="";
            if (String.IsNullOrEmpty(BearerConfig.Authority))
                ErrMsg += "Bearer Token Authority URL not found.\n";
            if (!Uri.IsWellFormedUriString(BearerConfig.Authority, UriKind.Absolute))
                ErrMsg += "Bearer Token Authority URL not well formed.\n";
            if (String.IsNullOrEmpty(BearerConfig.ClientId))
                ErrMsg += "Bearer Token Client Id not found.\n";
            if (String.IsNullOrEmpty(BearerConfig.ClientSecret))
                ErrMsg += "Bearer Token Client Secret not found.\n";
            if (String.IsNullOrEmpty(BearerConfig.Scope))
                ErrMsg += "Bearer Token Scope not found.\n";
            if (ErrMsg.Length>1)
                throw (new System.ArgumentException(ErrMsg));

        }

        // Configuration API URL - by Environment Variable name
        // Use the Environment Variable name parameter to get the URL of the ConfigurationAPI.
        public void SetConfigUrlFromEnvVar(string configUrlVar)
        {
            ConfigUrl = Environment.GetEnvironmentVariable(configUrlVar);

            if (ConfigUrl == null && _optional == false)
                throw new Exception($"Unable to create Api Source Options, Environment Variable: '{configUrlVar}' not found.");
            else { 
                SetDefaults();
         
            }
        }

        public void SetAuthSecretFromEnvVar(string authSecretVar)
        {
            // Get Defaults must have already been called.

            // If the authentication type requires a secret, verify the secret is there
            if (AuthType == "ApiKey" || AuthType =="Certificate")
            {
                AuthSecret = Environment.GetEnvironmentVariable(authSecretVar);
            
            if (AuthSecret == null)
                throw new Exception($"Environment Variable: '{authSecretVar}' was not found.");
            }
        }

        public void SetQueryParameters(Dictionary<string,string> qParams)
        {
            // if query paramaters have been supplied, add them as query string parameters
            
            var newUrl = new Uri(QueryHelpers.AddQueryString(ConfigUrl, qParams));
            ConfigUrl = newUrl.ToString();
        }
       
        public void SetRouteParameters(string [] rParams)
        {
            for(int i = 0;i<rParams.Length;i++)
            ConfigUrl = ConfigUrl.TrimEnd('/') + "/" + rParams[i];
        }
        
        private void SetDefaults()
        {
            if (string.IsNullOrEmpty(ConfigUrlKey))
                ConfigUrlKey = ApiDefault.ConfigUrlKey;

            if (string.IsNullOrEmpty(AuthType))
                AuthType = ApiDefault.AuthType;

            if (string.IsNullOrEmpty(AppId))
                AppId = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
        }
    }
}
