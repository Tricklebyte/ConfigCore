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
    public class ApiSource_CertKey_Fixture : IDisposable
    {

        //  create environment variables ############################################
        readonly List<EnvVar> _envVarList;

        public ApiSource_CertKey_Fixture()
        {
            string path = Environment.CurrentDirectory + "\\TestCases\\ApiSource\\Cert-ApiKey\\EnvVars.json";
            _envVarList = JsonConvert.DeserializeObject<List<EnvVar>>(File.ReadAllText(path));
            TestHelper.CreateEnvVars(_envVarList);
        }

        public void Dispose()
        {
            TestHelper.DeleteEnvVars(_envVarList);
        }
    }


    /// <summary>
    /// Many external and internal methods are shared by Windows, Anonymous, Certificate, and Key auth types.
    /// Thorough testing of all shared methods is done in test file AnonWin_ApiSource_Tests.cs
    /// Those tests are not all repeated here. These tests target Certificate and ApiKey Authorization only.
    /// 
    /// </summary>
    public class ApiSource_CertKey_Tests : IClassFixture<ApiSource_CertKey_Fixture>
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


            IConfiguration actual = builder.AddApiSource(configUrlVar, authType, authSecretVar, new string[] {appId}, optional).Build();

            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
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
            builder.AddApiSource(configUrlVar, authType, authSecretVar, new string[] { appId }, optional);

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
                builder.AddApiSource(configUrlVar, authSecretVar, authType, new string[] { appId }, optional);
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, authSecretVar, authType, new string[] { appId }, optional));
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
                builder.AddApiSource(configUrlVar, authType, authSecretVar, new string[] { appId }, optional);
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, authType, authSecretVar, new string[] { appId }, optional));
        }


        [InlineData("ConfigURL-Key", "ConfigAuth-KeyFail", "ApiKey", null, true)]
        [InlineData("ConfigURL-Cert", "ConfigAuth-KeyFail", "ApiKey", null, false)]
        [Theory]
        // Fails authentication with an invalid key value
        public void OptParams_Key_AuthFail(string configUrlVar, string authType, string authSecretVar, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddApiSource(configUrlVar, authSecretVar, authType, new string[] { appId }, optional);

            if (optional)
            {
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.AggregateException>(() => actual = builder.Build());
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
                builder.AddApiSource(configUrlVar, authType, authSecretVar, new string[] { appId }, optional);
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, authType, authSecretVar, new string[] { appId }, optional));
        }
        #endregion

        
        #region AddApiSource Overloads with IConfiguration Parameter
        // Loads settings from Configuration section "ConfigOptions:ApiSource" to override default comm and auth settings for the HTTP Client.
        // Two good test cases for each authentication type, tested for optional true and false
       
        [InlineData("CertGood", "1", true)]
        [InlineData("CertGood", "1", false)]
        [InlineData("CertGood", "2", true)]
        [InlineData("CertGood", "2", false)]
        [InlineData("KeyGood", "1", true)]
        [InlineData("KeyGood", "1", false)]
        [InlineData("KeyGood", "2", true)]
        [InlineData("KeyGood", "2", false)]
     
        [Theory]
        public void Config_Good(string testAuthType, string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\Config\\Good\\{testAuthType}{testCase}.json";

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
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\Config\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        //Environment Variable for ConfigURL not found. Will fail on adding ApiSource
        [InlineData("1", true)]
        [InlineData("1", false)]
        [Theory]
        public void Config_SectionNotFound(string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\Config\\SectionNotFound\\appsettings{testCase}.json";

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
    
        [InlineData("CertConnFail", true)]
        [InlineData("CertConnFail", false)]
        [InlineData("KeyConnFail", true)]
        [InlineData("KeyConnFail", false)]
     
        [Theory]
        public void Config_ConnectFail(string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\Config\\ConnectFail\\{testCase}.json";

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

      

        #endregion


        #region Api Client Authentication with IConfiguration Parameter


        [InlineData("CertAuthFail", true)]
        [InlineData("CertAuthFail", false)]
     
        [Theory]
        public void Cert_AuthFail(string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\Config\\AuthFail\\{testCase}.json";

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

        [InlineData("KeyAuthFail", true)]
        [InlineData("KeyAuthFail", false)]
        [Theory]
        public void ApiKey_AuthFail(string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\Config\\AuthFail\\{testCase}.json";

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
            string jsonPath = $"TestCases\\ApiSource\\Config\\CreateRequestFail\\{testCase}.json";

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


        //Bearer Auth Fail
        //Bearer Create Request Fail


        #endregion


        #region AddApiSource Overloads with Query String Parameters
        // Exact Match, AND

        //URL, Dict
        [Theory]
        [InlineData("ConfigURL-Anon", "Anon", null, "1", true)]
        [InlineData("ConfigURL-Anon", "Anon", null, "1", false)]
        [InlineData("ConfigURL-Anon", "Anon", null, "2", true)]
        [InlineData("ConfigURL-Anon", "Anon", null, "2", false)]
        public void QParams_Good(string configUrlVar, string authType, string authSecret, string testCase, bool optional)
        {
            var builder = new ConfigurationBuilder();
            //  var listParams = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\QueryStringParams\\Good\\input{testCase}.json"));
            Dictionary<string, string> dictParams = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText($"TestCases\\ApiSource\\Cert-ApiKey\\QueryParams\\Good\\input{testCase}.json"));

            IConfiguration actual = builder.AddApiSource(configUrlVar, authType, authSecret, dictParams, optional).Build();

            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\QueryParams\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        //QParams_BadParameterName -  invalid parameter name
        //QParams_BadParameterValue - invalid parameter value
        //QParams_EmptyParamDictionary 

        //




        #endregion


        #region Api Client Authentication with Query String Parameters
        
        
        
        #endregion


    }
}

