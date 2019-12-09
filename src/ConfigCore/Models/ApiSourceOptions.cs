using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace ConfigCore.Models
{
    public class ApiSourceOptions
    {
        
        public string AuthType { get; set; }
        public string AuthClaimType { get; set; }
        public string AuthClaimValue { get; set; }

        public string ConfigSettingUrlKey { get; set; }
        public string ConfigSettingUrl { get; set; }
        public string AppId { get; set; }



        public ApiSourceOptions(string configUrlVar, bool optional)
        {
            SetConfigUrlFromEnvVar(configUrlVar, optional);
            AddDefaults();
        }
        public ApiSourceOptions(string configUrlVar, string appId, bool optional)
        {
            SetConfigUrlFromEnvVar(configUrlVar, optional);
            AppId = AppId;
            AddDefaults();
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
            // if user did not supply value for urlKey, use default setting key
            ConfigSettingUrlKey = apiSection["ConfigApiUrlKey"];
            ConfigSettingUrl = config[ConfigSettingUrlKey];
            AuthType = apiSection["AuthType"];
            AuthClaimType = apiSection["AuthClaimType"];
            AuthClaimValue = apiSection["AuthClaimValue"];
            AppId = apiSection["AppId"];

            //Add defaults for any required values that were not supplied
            AddDefaults();

            //Set Full Configuration API URL including action and Parmater
            ConfigSettingUrl = config[ConfigSettingUrlKey];
            if (string.IsNullOrEmpty(ConfigSettingUrl))
                throw new Exception($"Config API Url not found at configuration setting key: '{ConfigSettingUrlKey}'");
            else
                ConfigSettingUrl += AppId;
        }

        // Configuration API URL - by Environment Variable name
        // Use the Environment Variable name parameter to get the URL of the ConfigurationAPI.
        public void SetConfigUrlFromEnvVar(string configUrlVar, bool optional)
        {
            ConfigSettingUrl = Environment.GetEnvironmentVariable(configUrlVar);
            if (ConfigSettingUrl == null && optional == false)
                throw new Exception($"Unable to create Api Source Options, Environment Variable: '{configUrlVar}' not found.");
            else
                AddDefaults();

                ConfigSettingUrl += AppId;
        }

        private void AddDefaults()
        {
            if (string.IsNullOrEmpty(ConfigSettingUrlKey))
                ConfigSettingUrlKey = "ConfigOptions:ApiSource:ConfigApiUrl";

            if (string.IsNullOrEmpty(AuthType))
                AuthType = ApiDefault.AuthType;

            if (string.IsNullOrEmpty(AuthClaimType))
                AuthClaimType = ApiDefault.AuthClaimType;

            if (string.IsNullOrEmpty(AuthClaimValue))
                AuthClaimValue = ApiDefault.AuthClaimValue;

            if (string.IsNullOrEmpty(AppId))
                AppId =  System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
        }
    }
}
