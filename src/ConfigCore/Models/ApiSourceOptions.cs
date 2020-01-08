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
            SetAuthSecretFromEnvVar(authType,authSecretVar);
        }

        
        /// <summary>
        /// Accepts Configuration as parameter. Used to override default apiclient settings 
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
           
            if (string.IsNullOrEmpty(ConfigUrl))
                throw new Exception($"Configuration setting not found: '{ConfigUrlKey}'");
            else
                // Verify trailing slash and add parameter to url string
                ConfigUrl = ConfigUrl.TrimEnd('/') + @"/" + AppId;
          
            // Check if Authorization secret is required and present
            if (AuthType == "Certificate" || AuthType == "ApiKey")
            {
                if (string.IsNullOrEmpty(AuthSecret))
                {
                    throw new Exception($"Authentication secret not found in configuration setting - 'ConfigOptions:ApiSource:AuthSecret'");
                }
            }

        }

        // Configuration API URL - by Environment Variable name
        // Use the Environment Variable name parameter to get the URL of the ConfigurationAPI.
        public void SetConfigUrlFromEnvVar(string configUrlVar)
        {
            ConfigUrl = Environment.GetEnvironmentVariable(configUrlVar);

            if (ConfigUrl == null && _optional == false)
                throw new Exception($"Unable to create Api Source Options, Environment Variable: '{configUrlVar}' not found.");
            else
                SetDefaults();
            // Verify trailing slash and add parameter to url string
            ConfigUrl = ConfigUrl.TrimEnd('/') + "/" + AppId;
            
        }

        public void SetAuthSecretFromEnvVar(string authType, string authSecretVar)
        {
            // Get Defaults must have already been called.

            // If the authentication type requires a secret, verify the secret is there
            if (authType == "ApiKey" || authType =="Certificate")
            {
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

            if (string.IsNullOrEmpty(AppId))
                AppId = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
        }
    }
}
