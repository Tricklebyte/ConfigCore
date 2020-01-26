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
        readonly List<EnvVar> _envVarList;

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
        [InlineData("ConfigURL-Anon", "Anon", null, null, true, "1")]
        [InlineData("ConfigURL-Anon", "Anon", null, null, false, "1")]
        [InlineData("ConfigURL-Anon", "Anon", null, "CustomAppName", true, "2")]
        [InlineData("ConfigURL-Anon", "Anon", null, "CustomAppName", false, "2")]
        [InlineData("ConfigURL-Win", "Windows", null, null, true, "1")]
        [InlineData("ConfigURL-Win", "Windows", null, null, false, "1")]
        [InlineData("ConfigURL-Win", "Windows", null, "CustomAppName", true, "2")]
        [InlineData("ConfigURL-Win", "Windows", null, "CustomAppName", false, "2")]
        [InlineData("ConfigURL-Cert", "Certificate", "ConfigAuth-Cert", null, true, "1")]
        [InlineData("ConfigURL-Cert", "Certificate", "ConfigAuth-Cert", null, false, "1")]
        [InlineData("ConfigURL-Cert", "Certificate", "ConfigAuth-Cert", "CustomAppName", true, "2")]
        [InlineData("ConfigURL-Cert", "Certificate", "ConfigAuth-Cert", "CustomAppName", false, "2")]
        [InlineData("ConfigURL-Key", "ApiKey", "ConfigAuth-Key", null, true, "1")]
        [InlineData("ConfigURL-Key", "ApiKey", "ConfigAuth-Key", null, false, "1")]
        [InlineData("ConfigURL-Key", "ApiKey", "ConfigAuth-Key", "CustomAppName", true, "2")]
        [InlineData("ConfigURL-Key", "ApiKey", "ConfigAuth-Key", "CustomAppName", false, "2")]
        [Theory]
        public void OptParams_Good(string configUrlVar, string authType, string authSecretVar, string appId, bool optional, string testCase)
        {
            var builder = new ConfigurationBuilder();


            IConfiguration actual = builder.AddApiSource(configUrlVar, authType, authSecretVar, appId, optional).Build();


            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\AddApiSource\\OptParams\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }

        //Overload with single required parameter for the URL of the API. Uses Windows Auth by default, optional False by default
        [InlineData("ConfigURL-Win", "1")]
        [InlineData("ConfigURL-Anon", "1")]
        [Theory]
        public void OptParamsWinDef_Good(string configUrlVar, string testCase)
        {

            var builder = new ConfigurationBuilder();
            IConfiguration actual = builder.AddApiSource(configUrlVar).Build();
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\AddApiSource\\OptParams\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }

        //Overload with single required parameter for the URL of the API and one optional parameter for Optional. Uses Windows Auth by default.
        [InlineData("ConfigURL-Win", true, "1")]
        [InlineData("ConfigURL-Anon", true, "1")]
        [Theory]
        public void OptParamsWinDefOptional_Good(string configUrlVar, bool optional, string testCase)
        {

            var builder = new ConfigurationBuilder();
            IConfiguration actual = builder.AddApiSource(configUrlVar, optional).Build();
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\AddApiSource\\OptParams\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        // Overload with parameters for URL environment variable name and non-default Application Id
        [InlineData("ConfigURL-Win", "CustomAppName", "2")]
        [InlineData("ConfigURL-Anon", "CustomAppName", "2")]
        [Theory]
        public void OptParamsWinAppId_Good(string configUrlVar, string appId, string testCase)
        {
            var builder = new ConfigurationBuilder();
            IConfiguration actual = builder.AddApiSource(configUrlVar, appId).Build();
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\AddApiSource\\OptParams\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }

        // Overload with parameters for URL environment variable name,  non-default Application Id, optional parameter
        [InlineData("ConfigURL-Win", "CustomAppName", true, "2")]
        [InlineData("ConfigURL-Anon", "CustomAppName", true, "2")]
        [Theory]
        public void OptParamsWinAppIdOpt_Good(string configUrlVar, string appId, bool optional, string testCase)
        {
            var builder = new ConfigurationBuilder();
            IConfiguration actual = builder.AddApiSource(configUrlVar, appId, optional).Build();
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\AddApiSource\\OptParams\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }

        // TODO OptParams no URL Var supplied

        // Environment Variable for ConfigURL not found. Will fail on adding ApiSource
        [InlineData("ConfigApi-UnknownVar", "Anon", null, null, true)]
        [InlineData("ConfigApi-UnknownVar", "Anon", null, null, false)]
        [InlineData("ConfigApi-UnknownVar", "Windows", null, null, true)]
        [InlineData("ConfigApi-UnknownVar", "Windows", null, null, false)]
        [InlineData("ConfigApi-UnknownVar", "Certificate", "ConfigAuth-Cert", null, true)]
        [InlineData("ConfigApi-UnknownVar", "Certificate", "ConfigAuth-Cert", null, false)]
        [InlineData("ConfigApi-UnknownVar", "ApiKey", "ConfigAuth-Key", null, true)]
        [InlineData("ConfigApi-UnknownVar", "ApiKey", "ConfigAuth-Key", null, false)]
        [Theory]
        public void OptParams_VarNotFound(string configUrlVar, string authType, string authSecretVar, string appId, bool optional)
        {
            var builder = new ConfigurationBuilder();
            if (optional)
            {
                builder.AddApiSource(configUrlVar, authType, authSecretVar, appId, optional);
                IConfiguration config;
                config = builder.Build();
                var actualList = config.GetConfigSettings();
                Assert.True(actualList.Count == 0);
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, null, authType, appId, optional));
        }





        // Environment Variable for ConfigURL not found. Will fail on adding ApiSource
        [InlineData(null, "Anon", null, null, true)]
        [InlineData(null, "Anon", null, null, false)]
        [InlineData(null, "Windows", null, null, true)]
        [InlineData(null, "Windows", null, null, false)]
        [InlineData(null, "Certificate", "ConfigAuth-Cert", null, true)]
        [InlineData(null, "Certificate", "ConfigAuth-Cert", null, false)]
        [InlineData(null, "ApiKey", "ConfigAuth-Key", null, true)]
        [InlineData(null, "ApiKey", "ConfigAuth-Key", null, false)]
        [Theory]
        public void OptParams_NoUrlVar(string configUrlVar, string authType, string authSecretVar, string appId, bool optional)
        {
            var builder = new ConfigurationBuilder();
            if (optional)
            {
                builder.AddApiSource(configUrlVar, authType, authSecretVar, appId, optional);
                IConfiguration config;
                config = builder.Build();
                var actualList = config.GetConfigSettings();
                Assert.True(actualList.Count == 0);
            }
            else
                Assert.Throws<System.ArgumentNullException>(() => builder.AddApiSource(configUrlVar, null, authType, appId, optional));
        }

        //Assigns a valid, incorrect Url. Will fail on connect, so error will be thrown on build.
        [InlineData("ConfigApi-WrongUrl", "Anon", null, null, true)]
        [InlineData("ConfigApi-WrongUrl", "Anon", null, null, false)]
        [InlineData("ConfigApi-WrongUrl", "Windows", null, null, true)]
        [InlineData("ConfigApi-WrongUrl", "Windows", null, null, false)]
        [InlineData("ConfigApi-WrongUrl", "Certificate", "ConfigAuth-Cert", null, true)]
        [InlineData("ConfigApi-WrongUrl", "Certificate", "ConfigAuth-Cert", null, false)]
        [InlineData("ConfigApi-WrongUrl", "ApiKey", "ConfigAuth-Key", null, true)]
        [InlineData("ConfigApi-WrongUrl", "ApiKey", "ConfigAuth-Key", null, false)]
        [Theory]
        public void OptParams_WrongUrl(string configUrlVar, string authType, string authSecretVar, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddApiSource(configUrlVar, authType, authSecretVar, appId, optional);

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
        [InlineData("ConfigApi-InvalidUrl", "Anon", null, null, true)]
        [InlineData("ConfigApi-InvalidUrl", "Anon", null, null, false)]
        [InlineData("ConfigApi-InvalidUrl", "Windows", null, null, true)]
        [InlineData("ConfigApi-InvalidUrl", "Windows", null, null, false)]
        [InlineData("ConfigApi-InvalidUrl", "Certificate", "ConfigAuth-Cert", null, true)]
        [InlineData("ConfigApi-InvalidUrl", "Certificate", "ConfigAuth-Cert", null, false)]
        [InlineData("ConfigApi-InvalidUrl", "ApiKey", "ConfigAuth-Key", null, true)]
        [InlineData("ConfigApi-InvalidUrl", "ApiKey", "ConfigAuth-Key", null, false)]
        [Theory]
        public void OptParams_InvalidUrl(string configUrlVar, string authType, string authSecretVar, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();

            if (optional)
            {
                builder.AddApiSource(configUrlVar, authType, authSecretVar, appId, optional);
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.UriFormatException>(() => builder.AddApiSource(configUrlVar, authSecretVar, authType, appId, optional));
        }
        #endregion


        #region API Cient Authentication with options parameters

        [InlineData("ConfigURL-Cert", "Certificate", "ConfigAuth-CertFail", null, true)]
        [InlineData("ConfigURL-Cert", "Certificate", "ConfigAuth-CertFail", null, false)]
        [Theory]
        // uses a Certificate that is installed on the client but not accepted by the Host
        public void OptParams_Cert_AuthFail(string configUrlVar, string authType, string authSecretVar, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddApiSource(configUrlVar, authType, authSecretVar, appId, optional);

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
        public void OptParams_Cert_NotInstalled(string configUrlVar, string authType, string authSecretVar, string appId, bool optional)
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
        [InlineData("ConfigURL-Cert", "Certificate", "", null, true)]
        [InlineData("ConfigURL-Cert", "Certificate", "", null, false)]
        [Theory]
        public void OptParams_CertNoSecret(string configUrlVar, string authType, string authSecretVar, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            if (optional)
            {
                builder.AddApiSource(configUrlVar, authType, authSecretVar, appId, optional);
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, authType, authSecretVar, appId, optional));
        }


        [InlineData("ConfigURL-Key", "ConfigAuth-KeyFail", "ApiKey", null, true)]
        [InlineData("ConfigURL-Cert", "ConfigAuth-KeyFail", "ApiKey", null, false)]
        // Fails authentication with an invalid key value
        public void OptParams_Key_AuthFail(string configUrlVar, string authType, string authSecretVar, string appId, bool optional)
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

        [Theory]
        [InlineData("ConfigURL-Win", "ApiKey", "ConfigAuth-Key", null, true)]
        [InlineData("ConfigURL-Win", "ApiKey", "ConfigAuth-Key", null, false)]
        // Fails authentication - windows credentials not supplied to Host
        // using the windows url, with APIKey headers
        public void OptParams_Windows_AuthFail(string configUrlVar, string authType, string authSecretVar, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddApiSource(configUrlVar, authType, authSecretVar, appId, optional);

            if (optional)
            {
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.Net.Http.HttpRequestException>(() => actual = builder.Build());
        }


        [Theory]
        [InlineData("ConfigURL-ApiKey", "ApiKey", null, null, true)]
        [InlineData("ConfigURL-ApiKey", "ApiKey", null, null, false)]
        // No parameter value supplied for Auth Secret
        public void OptParams_Key_NoSecret(string configUrlVar, string authType, string authSecretVar, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
        

            if (optional)
            {
                builder.AddApiSource(configUrlVar, authType, authSecretVar, appId, optional);
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, authType, authSecretVar, appId, optional));
        }



        #endregion


        #region AddApiSource Overloads with IConfiguration Parameter
        // Loads settings from Configuration section "ConfigOptions:ApiSource" to override default comm and auth settings for the HTTP Client.
        // Two good test cases for each authentication type, tested for optional true and false
        [InlineData("AnonGood", "1", true)]
        [InlineData("AnonGood", "1", false)]
        [InlineData("AnonGood", "2", true)]
        [InlineData("AnonGood", "2", false)]
        [InlineData("CertGood", "1", true)]
        [InlineData("CertGood", "1", false)]
        [InlineData("CertGood", "2", true)]
        [InlineData("CertGood", "2", false)]
        [InlineData("KeyGood", "1", true)]
        [InlineData("KeyGood", "1", false)]
        [InlineData("KeyGood", "2", true)]
        [InlineData("KeyGood", "2", false)]
        [InlineData("WinGood", "1", true)]
        [InlineData("WinGood", "1", false)]
        [InlineData("WinGood", "2", true)]
        [InlineData("WinGood", "2", false)]
        [Theory]
        public void Config_Good(string testAuthType, string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\AddApiSource\\Config\\Good\\{testAuthType}{testCase}.json";

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
        [InlineData("AnonConnFail", true)]
        [InlineData("AnonConnFail", false)]
        [InlineData("CertConnFail", true)]
        [InlineData("CertConnFail", false)]
        [InlineData("KeyConnFail", true)]
        [InlineData("KeyConnFail", false)]
        [InlineData("WinConnFail", true)]
        [InlineData("WinConnFail", false)]
        [Theory]
        public void Config_ConnectFail(string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\AddApiSource\\Config\\ConnectFail\\{testCase}.json";

            // Get initial config containing api connection and authentication info
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

        [InlineData("AnonInvalidUrl", true)]
        [InlineData("AnonInvalidUrl", false)]
        [Theory]
        public void Config_InvalidUrl(string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\AddApiSource\\Config\\InvalidUrl\\{testCase}.json";

            // Get initial config containing api connection and authentication info
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


        // Authentication Failure
        // Will fail on Build with HTTPRequestException


        [InlineData("CertAuthFail", true)]
        [InlineData("CertAuthFail", false)]
        [InlineData("WinAuthFail", true)]
        [InlineData("WinAuthFail", false)]
        [Theory]
        public void Config_AuthFail_RequestException(string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\AddApiSource\\Config\\AuthFail\\{testCase}.json";

            // Get initial config containing api connection and authentication info
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
                Assert.Throws<System.Net.Http.HttpRequestException>(() => actualConfig = builder.Build());
            }
        }

        // Authentication Failure
        // Will fail on Build with AggregateException
        [InlineData("AnonAuthFail", true)]
        [InlineData("AnonAuthFail", false)]
        [InlineData("KeyAuthFail", true)]
        [InlineData("KeyAuthFail", false)]
        [Theory]
        public void Config_AuthFail_AggregateException(string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\AddApiSource\\Config\\AuthFail\\{testCase}.json";

            // Get initial config containing api connection and authentication info
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


        // Unable to create HTTP Request - either no secret or certificate not found
        // will fail on AddApiSource
        [InlineData("CertNoSecret", true)]
        [InlineData("CertNoSecret", false)]
        [InlineData("CertNotInstalled", true)]
        [InlineData("CertNotInstalled", false)]
        [InlineData("KeyNoSecret", true)]
        [InlineData("KeyNoSecret", false)]
        [Theory]
        public void Config_Auth_CreateRequestFail(string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\AddApiSource\\Config\\CreateRequestFail\\{testCase}.json";

            // Get initial config containing api connection and authentication info
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
                Assert.Throws<System.Exception>(() => builder.AddApiSource(initConfig, optional));
        }

        #endregion

    }
}

