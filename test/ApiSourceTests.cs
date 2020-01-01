using Microsoft.Extensions.Configuration;
using ConfigCore.Extensions;
using System;
using Xunit;
using Newtonsoft.Json;
using System.Collections.Generic;
using ConfigCore.Models;
using System.IO;
using System.Linq;

namespace ConfigCore.Tests
{/// <summary>
/// Creates required Environment Variables prior to testing and removes them upon disposal.
/// </summary>
    public class ApiSourceFixture : IDisposable
    {

        //  create environment variables ############################################
         List<EnvVar> _envVarList;       
                    
        public ApiSourceFixture()
        {
            string path = Environment.CurrentDirectory + "\\TestCases\\ApiSource\\ApiEnvVars.json";
            _envVarList = JsonConvert.DeserializeObject<List<EnvVar>>(File.ReadAllText(path));
            TestHelper.CreateEnvVars(_envVarList);
        }

        public void Dispose()
        {
            TestHelper.DeleteEnvVars(_envVarList);
        }
    }



    public class ApiSourceTests : IClassFixture<ApiSourceFixture>
    {

        #region AddApiSource Overloads with options parameters
        /// <summary>
        /// Test parameters ConfigUrlVar, AuthSecretVar, AuthType, AppId
        /// </summary>
        /// <param name="configUrlVar">Environment Variable name holding URL</param>
        /// <param name="authSecretVar">Enivoronment Variable name holding Authentication Secret</param>
        /// <param name="authType">Client authentication type - Certificate, ApiKey, Windows</param>
        /// <param name="appId"></param>
        /// <param name="optional"></param>
        /// <param name="testCase"></param>
        [InlineData("ConfigURL-Anon", null, "Anon", null, true, "1")]
        [InlineData("ConfigURL-Anon", null, "Anon", null, false, "1")]
        [InlineData("ConfigURL-Anon", null, "Anon", "CustomAppName", true, "2")]
        [InlineData("ConfigURL-Anon", null, "Anon", "CustomAppName", false, "2")]
        [InlineData("ConfigURL-Win", null, "Windows", null, true, "1")]
        [InlineData("ConfigURL-Win", null, "Windows", null, false, "1")]
        [InlineData("ConfigURL-Win", null, "Windows", "CustomAppName", true, "2")]
        [InlineData("ConfigURL-Win", null, "Windows", "CustomAppName", false, "2")]
        [InlineData("ConfigURL-Cert", "ConfigAuth-Cert", "Certificate", null, true, "1")]
        [InlineData("ConfigURL-Cert", "ConfigAuth-Cert", "Certificate", null, false, "1")]
        [InlineData("ConfigURL-Cert", "ConfigAuth-Cert", "Certificate", "CustomAppName", true, "2")]
        [InlineData("ConfigURL-Cert", "ConfigAuth-Cert", "Certificate", "CustomAppName", false, "2")]
        [InlineData("ConfigURL-Key", "ConfigAuth-Key", "ApiKey", null, true, "1")]
        [InlineData("ConfigURL-Key", "ConfigAuth-Key", "ApiKey", null, false, "1")]
        [InlineData("ConfigURL-Key", "ConfigAuth-Key", "ApiKey", "CustomAppName", true, "2")]
        [InlineData("ConfigURL-Key", "ConfigAuth-Key", "ApiKey", "CustomAppName", false, "2")]
        [Theory]
        public void OptParams_Good(string configUrlVar, string authSecretVar, string authType, string appId, bool optional, string testCase)
        {
            var builder = new ConfigurationBuilder();
            IConfiguration actual = builder.AddApiSource(configUrlVar, authSecretVar, authType, appId, optional).Build();
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\AddApiSource\\OptParams\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        // TODO OptParams no URL Var supplied
        
        // Environment Variable for ConfigURL not found. Will fail on adding ApiSource
        [InlineData("ConfigApi-UnknownVar", null, "Anon", null, true)]
        [InlineData("ConfigApi-UnknownVar", null, "Anon", null, false)]
        [InlineData("ConfigApi-UnknownVar", null, "Windows", null, true)]
        [InlineData("ConfigApi-UnknownVar", null, "Windows", null, false)]
        [InlineData("ConfigApi-UnknownVar", "ConfigAuth-Cert", "Certificate", null, true)]
        [InlineData("ConfigApi-UnknownVar", "ConfigAuth-Cert", "Certificate", null, false)]
        [InlineData("ConfigApi-UnknownVar", "ConfigAuth-Key", "ApiKey", null, true)]
        [InlineData("ConfigApi-UnknownVar", "ConfigAuth-Key", "ApiKey", null, false)]
        [Theory]
        public void OptParams_VarNotFound(string configUrlVar, string authSecretVar, string authType, string appId, bool optional)
        {
            var builder = new ConfigurationBuilder();
            if (optional)
            {
                builder.AddApiSource(configUrlVar, authSecretVar, authType, appId, optional);
                IConfiguration config;
                config = builder.Build();
                var actualList = config.GetConfigSettings();
                Assert.True(actualList.Count == 0);
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, null, authType, appId, optional));
        }


        // Environment Variable for ConfigURL not found. Will fail on adding ApiSource
        [InlineData(null, null, "Anon", null, true)]
        [InlineData(null, null, "Anon", null, false)]
        [InlineData(null, null, "Windows", null, true)]
        [InlineData(null, null, "Windows", null, false)]
        [InlineData(null, "ConfigAuth-Cert", "Certificate", null, true)]
        [InlineData(null, "ConfigAuth-Cert", "Certificate", null, false)]
        [InlineData(null, "ConfigAuth-Key", "ApiKey", null, true)]
        [InlineData(null, "ConfigAuth-Key", "ApiKey", null, false)]
        [Theory]
        public void OptParams_NoUrlVar(string configUrlVar, string authSecretVar, string authType, string appId, bool optional)
        {
            var builder = new ConfigurationBuilder();
            if (optional)
            {
                builder.AddApiSource(configUrlVar, authSecretVar, authType, appId, optional);
                IConfiguration config;
                config = builder.Build();
                var actualList = config.GetConfigSettings();
                Assert.True(actualList.Count == 0);
            }
            else
                Assert.Throws<System.ArgumentNullException>(() => builder.AddApiSource(configUrlVar, null, authType, appId, optional));
        }

        //Assigns a valid, incorrect Url. Will fail on connect, so error will be thrown on build.
        [InlineData("ConfigApi-WrongUrl", null, "Anon", null, true)]
        [InlineData("ConfigApi-WrongUrl", null, "Anon", null, false)]
        [InlineData("ConfigApi-WrongUrl", null, "Windows", null, true)]
        [InlineData("ConfigApi-WrongUrl", null, "Windows", null, false)]
        [InlineData("ConfigApi-WrongUrl", "ConfigAuth-Cert", "Certificate", null, true)]
        [InlineData("ConfigApi-WrongUrl", "ConfigAuth-Cert", "Certificate", null, false)]
        [InlineData("ConfigApi-WrongUrl", "ConfigAuth-Key", "ApiKey", null, true)]
        [InlineData("ConfigApi-WrongUrl", "ConfigAuth-Key", "ApiKey", null, false)]
        [Theory]
        public void OptParams_WrongUrl(string configUrlVar, string authSecretVar, string authType, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddApiSource(configUrlVar, authSecretVar, authType, appId, optional);

            if (optional)
            {
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.Net.Http.HttpRequestException>(() => actual = builder.Build());
        }

        // Tries to assign an invalid URI value. Will fail on adding ApiSource
        [InlineData("ConfigApi-InvalidUrl", null, "Anon", null, true)]
        [InlineData("ConfigApi-InvalidUrl", null, "Anon", null, false)]
        [InlineData("ConfigApi-InvalidUrl", null, "Windows", null, true)]
        [InlineData("ConfigApi-InvalidUrl", null, "Windows", null, false)]
        [InlineData("ConfigApi-InvalidUrl", "ConfigAuth-Cert", "Certificate", null, true)]
        [InlineData("ConfigApi-InvalidUrl", "ConfigAuth-Cert", "Certificate", null, false)]
        [InlineData("ConfigApi-InvalidUrl", "ConfigAuth-Key", "ApiKey", null, true)]
        [InlineData("ConfigApi-InvalidUrl", "ConfigAuth-Key", "ApiKey", null, false)]
        [Theory]
        public void OptParams_InvalidUrl(string configUrlVar, string authSecretVar, string authType, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();

            if (optional)
            {
                builder.AddApiSource(configUrlVar, authSecretVar, authType, appId, optional);
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.UriFormatException>(() => builder.AddApiSource(configUrlVar, authSecretVar, authType, appId, optional));
        }
        #endregion


        #region API Cient Authentication with options parameters

        [InlineData("ConfigURL-Cert", "ConfigAuth-CertFail", "Certificate", null, true)]
        [InlineData("ConfigURL-Cert", "ConfigAuth-CertFail", "Certificate", null, false)]
        [Theory]
        // uses a Certificate that is installed on the client but not accepted by the Host
        public void OptParams_Cert_AuthFail(string configUrlVar, string authSecretVar, string authType, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddApiSource(configUrlVar, authSecretVar, authType, appId, optional);

            if (optional)
            {
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.AggregateException>(() => actual = builder.Build());
        }


        [InlineData("ConfigURL-Cert", "ConfigAuth-CertNotFound", "Certificate", null, true)]
        [InlineData("ConfigURL-Cert", "ConfigAuth-CertNotFound", "Certificate", null, false)]
        [Theory]
        // Auth Secret parameter does not identify a locally installed Certificate
        public void OptParams_Cert_NotInstalled(string configUrlVar, string authSecretVar, string authType, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();


            if (optional)
            {
                builder.AddApiSource(configUrlVar, authSecretVar, authType, appId, optional);
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, authSecretVar, authType, appId, optional));
        }




        // Auth Secret parameter not supplied
        [InlineData("ConfigURL-Cert", "", "Certificate", null, true)]
        [InlineData("ConfigURL-Cert", "", "Certificate", null, false)]
        [Theory]
        public void OptParams_CertNoSecret(string configUrlVar, string authSecretVar, string authType, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            if (optional)
            {
                builder.AddApiSource(configUrlVar, authSecretVar, authType, appId, optional);
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, authSecretVar, authType, appId, optional));
        }


        [InlineData("ConfigURL-Key", "ConfigAuth-KeyFail", "ApiKey", null, true)]
        [InlineData("ConfigURL-Cert", "ConfigAuth-KeyFail", "ApiKey", null, false)]
        // Fails authentication with an invalid key value
        public void OptParams_Key_AuthFail(string configUrlVar, string authSecretVar, string authType, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddApiSource(configUrlVar, authSecretVar, authType, appId, optional);

            if (optional)
            {
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.Net.Http.HttpRequestException>(() => actual = builder.Build());
        }

        // Fails authentication - windows credentials not supplied to Host
        public void OptParams_Windows_AuthFail()
        {
            throw new NotImplementedException();
        }



        // No parameter value supplied for Auth Secret
        public void OptParams_Key_NoSecret()
        {
            throw new NotImplementedException();
        }


        #endregion


        #region AddApiSource Overloads with IConfiguration Parameter
        // Loads settings from Configuration section "ConfigOptions:ApiSource" to override default comm and auth settings for the HTTP Client.
        // Two good test cases for each authentication type, tested for optional true and false
        [InlineData("Anon", "1", true)]
        [InlineData("Anon", "1", false)]
        [InlineData("Anon", "2", true)]
        [InlineData("Anon", "2", false)]
        [InlineData("Cert", "1", true)]
        [InlineData("Cert", "1", false)]
        [InlineData("Cert", "2", true)]
        [InlineData("Cert", "2", false)]
        [InlineData("Key", "1", true)]
        [InlineData("Key", "1", false)]
        [InlineData("Key", "2", true)]
        [InlineData("Key", "2", false)]
        [InlineData("Win", "1", true)]
        [InlineData("Win", "1", false)]
        [InlineData("Win", "2", true)]
        [InlineData("Win", "2", false)]
        [Theory]
        public void Config_Good(string testAuthType, string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\AddApiSource\\Config\\Good\\appsettings{testAuthType}{testCase}.json";

            // Get initial config containing non-default database settings
            var initConfig = new ConfigurationBuilder().AddJsonFile(jsonPath, false).Build();

            // Create the final builder 
            IConfigurationBuilder finalBuilder = new ConfigurationBuilder();

            // Add the DBSource to the final builder
            finalBuilder.AddApiSource(initConfig, optional);

            // Build the final config
            var actual = finalBuilder.Build();

            // Convert to lists and compare
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\AddApiSource\\Config\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        //Environment Variable for ConfigURL not found. Will fail on adding ApiSource
        [InlineData("1", true)]
        [InlineData("1", false)]
        [Theory]
        public void Config_SectionNotFound(string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\AddApiSource\\Config\\SectionNotFound\\appsettings{testCase}.json";

            // Get initial config containing non-default database settings
            var initConfig = new ConfigurationBuilder().AddJsonFile(jsonPath, false).Build();


            // Create the final builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();

            if (optional)
            {
                builder.AddApiSource(initConfig, optional);
                IConfiguration actualConfig;
                actualConfig = builder.Build();
                var actualList = actualConfig.GetConfigSettings();
                Assert.True(actualList.Count == 0);
            }
            else
            {
                Assert.Throws<System.Exception>(() => builder.AddApiSource(initConfig, optional));
            }
        }

        //Assigns a valid, incorrect Url. Will fail on connect, so error will be thrown on build.
        [InlineData("Anon", "1", true)]
        [InlineData("Anon", "1", false)]
        [InlineData("Cert", "1", true)]
        [InlineData("Cert", "1", false)]
        [InlineData("Key", "1", true)]
        [InlineData("Key", "1", false)]
        [InlineData("Win", "1", true)]
        [InlineData("Win", "1", false)]
        [Theory]
        public void Config_ConnectFail(string testAuthType, string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\AddApiSource\\Config\\ConnectFail\\appsettings{testAuthType}{testCase}.json";

            // Get initial config containing non-default database settings
            var initConfig = new ConfigurationBuilder().AddJsonFile(jsonPath, false).Build();

            // Create the final builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();
            IConfiguration actualConfig;
            builder.AddApiSource(initConfig, optional);

            if (optional)
            {
                actualConfig = builder.Build();
                var actualList = actualConfig.GetConfigSettings();
                Assert.True(actualList.Count == 0);
            }
            else
            {
                Assert.Throws<System.AggregateException>(() => actualConfig = builder.Build());
            }
        }

        // Tries to assign an invalid URI value. Will fail on adding ApiSource
        [InlineData("Anon", "1", true)]
        [InlineData("Anon", "1", false)]
        [Theory]
        public void Config_InvalidUrl(string testAuthType, string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\AddApiSource\\Config\\InvalidUrl\\appsettings{testAuthType}{testCase}.json";

            // Get initial config containing non-default database settings
            var initConfig = new ConfigurationBuilder().AddJsonFile(jsonPath, false).Build();

            // Create the final builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();
            IConfiguration actualConfig;

            if (optional)
            {
                builder.AddApiSource(initConfig, optional);
                actualConfig = builder.Build();
                var listActual = actualConfig.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.UriFormatException>(() => builder.AddApiSource(initConfig, optional));
        }


        #endregion


        #region Api Client Authentication with IConfiguration Parameter


        // uses a Certificate that is installed on the client but not accepted by the Host
        // Will fail on Build
        [InlineData("Cert", "1", true)]
        [InlineData("Cert", "1", false)]
        [Theory]
        public void Config_Auth_CertAuthFail(string testAuthType, string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\AddApiSource\\Config\\AuthFail\\appsettings{testAuthType}{testCase}.json";

            // Get initial config containing non-default database settings
            var initConfig = new ConfigurationBuilder().AddJsonFile(jsonPath, false).Build();

            // Create the final builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();
            IConfiguration actualConfig;
            builder.AddApiSource(initConfig, optional);

            if (optional)
            {
                actualConfig = builder.Build();
                var actualList = actualConfig.GetConfigSettings();
                Assert.True(actualList.Count == 0);
            }
            else
            {
                Assert.Throws<System.AggregateException>(() => actualConfig = builder.Build());
            }
        }

        // Fails authentication with an invalid key value
        // Will fail on Build
        [InlineData("Key", "1", true)]
        [InlineData("Key", "1", false)]
        [Theory]
        public void Config_Auth_KeyAuthFail(string testAuthType, string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\Config\\AuthFail\\appsettings{testAuthType}{testCase}.json";

            // Get initial config containing non-default database settings
            var initConfig = new ConfigurationBuilder().AddJsonFile(jsonPath, false).Build();

            // Create the final builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();
            IConfiguration actualConfig;
            builder.AddApiSource(initConfig, optional);

            if (optional)
            {
                actualConfig = builder.Build();
                var actualList = actualConfig.GetConfigSettings();
                Assert.True(actualList.Count == 0);
            }
            else
            {
                Assert.Throws<System.AggregateException>(() => actualConfig = builder.Build());
            }


        }


        // Sends to Windows API URL, but with security config for Anon api
        // Will fail on Build
        [InlineData("Windows", "1", true)]
        [InlineData("Windows", "1", false)]
        [Theory]
        public void Config_AuthFail_Windows(string testAuthType, string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\Config\\AuthFail\\appsettings{testAuthType}{testCase}.json";

            // Get initial config containing non-default database settings
            var initConfig = new ConfigurationBuilder().AddJsonFile(jsonPath, false).Build();

            // Create the final builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();
            IConfiguration actualConfig;
            builder.AddApiSource(initConfig, optional);

            if (optional)
            {
                actualConfig = builder.Build();
                var actualList = actualConfig.GetConfigSettings();
                Assert.True(actualList.Count == 0);
            }
            else
            {
                Assert.Throws<System.AggregateException>(() => actualConfig = builder.Build());
            }
        }

        // Secret not found in Config for Certificate auth type
        // will fail on AddApiSource
        [InlineData("Cert","1", true)]
        [InlineData("Cert", "2", false)]
        [Theory]
        public void Config_AuthFail_Cert_NoSecret(string testAuthType, string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\AddApiSource\\Config\\AuthFail\\appsettings{testAuthType}{testCase}.json";

            // Get initial config containing non-default database settings
            var initConfig = new ConfigurationBuilder().AddJsonFile(jsonPath, false).Build();

            // Create the final builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();
            IConfiguration actualConfig;

            if (optional)
            {
                builder.AddApiSource(initConfig, optional);
                actualConfig = builder.Build();
                var listActual = actualConfig.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.UriFormatException>(() => builder.AddApiSource(initConfig, optional));
        }

        // Secret not found in Config for ApiKey auth type
        // will fail on AddApiSource
        [InlineData("1", true)]
        [InlineData("2", false)]
        [Theory]
        public void Config_AuthFail_Key_NoSecret(string testCase, bool optional)
        {
            throw new NotImplementedException();
        }

        // Certificate is not installed on the client
        //will fail on AddApiSource
        [InlineData("1", true)]
        [InlineData("2", false)]
        [Theory]
        public void Config_AuthFail_Cert_NotInstalled(string testCase, bool optional)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
