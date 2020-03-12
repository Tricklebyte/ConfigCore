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

        #region AddApiSource Overloads with route parameters
        /// <summary>
        /// Test parameters ConfigUrlVar, AuthSecretVar, AuthType, AppId
        /// </summary>
        /// <param name="configUrlVar">Environment Variable name holding URL</param>
        /// <param name="authSecretVar">Enivoronment Variable name holding Authentication Secret</param>
        /// <param name="authType">Client authentication type - Certificate, ApiKey, Windows</param>
        /// <param name="appId"></param>
        /// <param name="optional"></param>
        /// <param name="testCase"></param>
       
      
        [InlineData("ConfigURL-Cert", "Certificate", "ConfigAuth-Cert", "1")]
        [InlineData("ConfigURL-Key", "ApiKey", "ConfigAuth-Key",  "1")]
        [Theory]
        public void RouteParams_DefAppId_Good(string configUrlVar, string authType, string authSecretVar,   string testCase)
        {
            var builder = new ConfigurationBuilder();


            IConfiguration actual = builder.AddApiSource(configUrlVar, authType, authSecretVar).Build();

            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        // Overload with parameters for URL environment variable name, auth settings, and non-default Application Id
        [InlineData("ConfigURL-Cert", "Certificate", "ConfigAuth-Cert", new string[] { "CustomAppName" }, "2")]
        [InlineData("ConfigURL-Key", "ApiKey", "ConfigAuth-Key", new string[] { "CustomAppName" }, "2")]
        [Theory]
        public void RouteParams_AppId_Good(string configUrlVar, string authType, string authSecretVar, string[] routeParams, string testCase)
        {
            var builder = new ConfigurationBuilder();
            IConfiguration actual = builder.AddApiSource(configUrlVar, authType, authSecretVar, routeParams).Build();
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        // Overload with parameters for URL environment variable name, auth settings,non-default Application Id, optional parameter
        [InlineData("ConfigURL-Cert", "Certificate", "ConfigAuth-Cert", new string[] { "CustomAppName" }, true, "2")]
        [InlineData("ConfigURL-Key", "ApiKey", "ConfigAuth-Key", new string[] { "CustomAppName" }, true, "2")]
        [Theory]
        public void RouteParams_AppIdOptional_Good(string configUrlVar, string authType, string authSecretVar, string[] routeParams, bool optional, string testCase)
        {
            var builder = new ConfigurationBuilder();
            IConfiguration actual = builder.AddApiSource(configUrlVar, authType, authSecretVar, routeParams, optional).Build();
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }

 #endregion
        //Assigns a valid, incorrect Url. Will fail on connect, so error will be thrown on build.
        [InlineData("ConfigApi-WrongUrl", "Certificate", "ConfigAuth-Cert", new string[] { "" }, true, "1")]
        [InlineData("ConfigApi-WrongUrl", "Certificate", "ConfigAuth-Cert", new string[] { "" }, false, "1")]
        [InlineData("ConfigApi-WrongUrl", "ApiKey", "ConfigAuth-Key", new string[] { "CustomAppName" }, true, "2")]
        [InlineData("ConfigApi-WrongUrl", "ApiKey", "ConfigAuth-Key", new string[] { "CustomAppName" }, false, "2")]
        [Theory]
        public void RouteParams_WrongUrl(string configUrlVar, string authType, string authSecretVar, string[] routeParams, bool optional, string testCase) //FAIL ON BUILD
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddApiSource(configUrlVar, authType, authSecretVar, routeParams, optional);

            if (optional)
            {
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"testCases\\ApiSource\\Cert-ApiKey\\OptParams\\WrongUrl\\expected{testCase}.json"));

                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else

                Assert.Throws<System.Net.Http.HttpRequestException>(() => actual = builder.Build());
        }
 

        #region API Cient Auth Fail

        // Cert Auth fail
        [InlineData("ConfigURL-Cert", "Certificate", "ConfigAuth-CertFail", new string[] { "" }, true, "1")]
        [InlineData("ConfigURL-Cert", "Certificate", "ConfigAuth-CertFail", new string[] { "" }, false, "1")]
        [Theory]
        // uses a Certificate that is installed on the client but not accepted by the Host
        public void RouteParams_Cert_AuthFail(string configUrlVar, string authType, string authSecretVar, string[] routeParams, bool optional,string testCase)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddApiSource(configUrlVar, authType, authSecretVar, routeParams, optional);

            if (optional)
            {
                actual = builder.Build();
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\Cert-ApiKey\\OptParams\\AuthFail\\expected{testCase}.json"));
                var listActual = actual.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
                Assert.Throws<System.AggregateException>(() => actual = builder.Build());
        }

        // Key Auth Fail
        [InlineData("ConfigURL-Key", "ApiKey", "ConfigAuth-KeyFail", new string[] { "" }, true,"2")]
        [InlineData("ConfigURL-Key", "ApiKey", "ConfigAuth-KeyFail",new string[] { "" }, false,"2")]
        [Theory]
        // Fails authentication with an invalid key value
        public void RouteParams_Key_AuthFail(string configUrlVar, string authType, string authSecretVar, string[] routeParams, bool optional, string testCase)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddApiSource(configUrlVar, authSecretVar, authType, routeParams, optional);

            if (optional)
            {
                actual = builder.Build();
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\Cert-ApiKey\\OptParams\\AuthFail\\expected{testCase}.json"));
                var listActual = actual.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
                Assert.Throws<System.Net.Http.HttpRequestException>(() => actual = builder.Build());
        }


        [InlineData("ConfigURL-Cert", "ConfigAuth-CertNotFound", "Certificate", new string[] { "" }, true,"1")]
        [InlineData("ConfigURL-Cert", "ConfigAuth-CertNotFound", "Certificate", new string[] { "" }, false,"1")]
        [Theory]
        // Auth Secret parameter does not identify a locally installed Certificate
        public void RouteParams_Cert_NotInstalled(string configUrlVar, string authType, string authSecretVar, string[] routeParams, bool optional,string testCase)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();

            if (optional)
            {
                builder.AddApiSource(configUrlVar, authSecretVar, authType, routeParams, optional);
                actual = builder.Build();
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\Cert-ApiKey\\OptParams\\CertNotInstalled\\expected{testCase}.json"));

                var listActual = actual.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, authSecretVar, authType, routeParams, optional));
        }

        // Auth Secret parameter not supplied
        [InlineData("ConfigURL-Cert", "Certificate", "", new string[] { "" }, true, "1")]
        [InlineData("ConfigURL-Cert", "Certificate", "", new string[] { "" }, false, "1")]
        [Theory]
        public void RouteParams_CertNoSecret(string configUrlVar, string authType, string authSecretVar, string[] routeParams, bool optional,string testCase)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
          
            if (optional)
            {
                builder.AddApiSource(configUrlVar, authType, authSecretVar, routeParams, optional);
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\Cert-ApiKey\\OptParams\\CertNoSecret\\expected{testCase}.json"));
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, authType, authSecretVar, routeParams, optional));
        }

            

        [Theory]
        [InlineData("ConfigURL-Key", "ApiKey", "", new string[] { "" }, true,"1")]
        [InlineData("ConfigURL-Key", "ApiKey", "", new string[] { "" }, false,"1")]
        // No parameter value supplied for Auth Secret
        public void RouteParams_Key_NoSecret(string configUrlVar, string authType, string authSecretVar, string [] routeParams, bool optional,string testCase)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();


            if (optional)
            {
                builder.AddApiSource(configUrlVar, authType, authSecretVar, routeParams, optional);
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\Cert-ApiKey\\OptParams\\KeyNoSecret\\expected{testCase}.json"));

                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, authType, authSecretVar, routeParams, optional));
        }
        #endregion


        #region AddApiSource Overloads with Query String Parameters

        [Theory]
        [InlineData("ConfigURL-Key", "ApiKey", "ConfigAuth-Key", "appId", "testhost", "idList", "1,3,5,", "1", true)]
        [InlineData("ConfigURL-Key", "ApiKey", "ConfigAuth-Key", "appId", "testhost", "idList", "1,3,5,", "1", false)]
        [InlineData("ConfigURL-Key", "ApiKey", "ConfigAuth-Key", "appId", "CustomAppName", "idList", "6,8,10", "2", true)]
        [InlineData("ConfigURL-Key", "ApiKey", "ConfigAuth-Key", "appId", "CustomAppName", "idList", "6,8,10", "2", false)]
        [InlineData("ConfigURL-Cert", "Certificate", "ConfigAuth-Cert", "appId", "testhost", "idList", "1,3,5,", "1", true)]
        [InlineData("ConfigURL-Cert", "Certificate", "ConfigAuth-Cert", "appId", "testhost", "idList", "1,3,5,", "1", false)]
        [InlineData("ConfigURL-Cert", "Certificate", "ConfigAuth-Cert", "appId", "CustomAppName", "idList", "6,8,10", "2", true)]
        [InlineData("ConfigURL-Cert", "Certificate", "ConfigAuth-Cert", "appId", "CustomAppName", "idList", "6,8,10", "2", false)]
        //TODO: ADD WIN TEST CASES
        public void QueryParams_Good(string configUrlVar, string authType, string authSecret, string param1Name, string param1Value, string param2Name, string param2Value, string testCase, bool optional)
        {
            var builder = new ConfigurationBuilder();
            Dictionary<string, string> dictParams = new Dictionary<string, string>();
            dictParams.Add(param1Name, param1Value);
            dictParams.Add(param2Name, param2Value);

            IConfiguration actual = builder.AddApiSource(configUrlVar, authType, authSecret, dictParams, optional).Build();

            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\Cert-ApiKey\\QueryParams\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }



        [Theory]
        [InlineData("ConfigURL-Key", "ApiKey", "ConfigAuth-Key", "appId", "testhost", "idList", null, "1", true)]
        [InlineData("ConfigURL-Key", "ApiKey", "ConfigAuth-Key", "appId", "testhost", "idList", null, "1", false)]
        [InlineData("ConfigURL-Key", "ApiKey", "ConfigAuth-Key", "appId", "CustomAppName", "idList", null, "2", true)]
        [InlineData("ConfigURL-Key", "ApiKey", "ConfigAuth-Key", "appId", "CustomAppName", "idList", null, "2", false)]

        [InlineData("ConfigURL-Cert", "Certificate", "ConfigAuth-Cert", "appId", "testhost", "idList", null, "1", true)]
        [InlineData("ConfigURL-Cert", "Certificate", "ConfigAuth-Cert", "appId", "testhost", "idList", null, "1", false)]
        [InlineData("ConfigURL-Cert", "Certificate", "ConfigAuth-Cert", "appId", "CustomAppName", "idList", null, "2", true)]
        [InlineData("ConfigURL-Cert", "Certificate", "ConfigAuth-Cert", "appId", "CustomAppName", "idList", null, "2", false)]
        public void QueryParams_InvalidParams(string configUrlVar, string authType,string authSecret, string param1Name, string param1Value, string param2Name, string param2Value, string testCase, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            Dictionary<string, string> dictParams = new Dictionary<string, string>();
            dictParams.Add(param1Name, null);
            dictParams.Add(param2Name, "");

            if (optional)
            {
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"testCases\\ApiSource\\Anon-Win\\QueryParams\\InvalidUrl\\expected{testCase}.json"));

                //   builder.AddApiSource(configUrlVar, authType, null, dictParams, optional);
                builder.AddApiSource(configUrlVar, authType, null, dictParams, optional);
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
                Assert.Throws<System.ArgumentNullException>(() => builder.AddApiSource(configUrlVar, authType, null, dictParams, optional));
        }




        #endregion


        #region Api Client Authentication with Query String Parameters



        #endregion


        #region AddApiSource Overloads with IConfiguration Parameter
        // Loads settings from Configuration section "ConfigOptions:ApiSource" to override default comm and auth settings for the HTTP Client.
        // Two good test cases for each authentication type, tested for optional true and false

        [InlineData("Cert","RParam", "1", true)]
        [InlineData("Cert", "RParam", "1", false)]
        [InlineData("Cert", "RParam", "2", true)]
        [InlineData("Cert", "RParam", "2", false)]
        [InlineData("Cert", "QParam", "1", true)]
        [InlineData("Cert", "QParam", "1", false)]
        [InlineData("Cert", "QParam", "2", true)]
        [InlineData("Cert", "QParam", "2", false)]
        [InlineData("Key", "RParam", "1", true)]
        [InlineData("Key", "RParam", "1", false)]
        [InlineData("Key", "RParam", "2", true)]
        [InlineData("Key", "RParam", "2", false)]
        [InlineData("Key", "QParam", "1", true)]
        [InlineData("Key", "QParam", "1", false)]
        [InlineData("Key", "QParam", "2", true)]
        [InlineData("Key", "QParam", "2", false)]
        [Theory]
        public void Config_Good(string testAuthType, string testArgType, string testCase,  bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\Cert-ApiKey\\ConfigParam\\Good\\input{testAuthType}{testArgType}{testCase}.json";

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
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\Cert-ApiKey\\ConfigParam\\Good\\expected{testArgType}{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        //Configuration section for ConfigOptions not found. Will fail on adding ApiSource
        [InlineData("1", true)]
        [InlineData( "1", false)]
        [Theory]
        public void Config_SectionNotFound( string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\Cert-ApiKey\\ConfigParam\\SectionNotFound\\input{testCase}.json";

            // Get initial config containing non-default database settings
            var initConfig = new ConfigurationBuilder().AddJsonFile(jsonPath, false).Build();


            // Create the final builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();

            if (optional)
            {
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"testCases\\ApiSource\\Cert-ApiKey\\ConfigParam\\SectionNotFound\\expected{testCase}.json"));
                builder.AddApiSource(initConfig, optional);
                IConfiguration actualConfig;
                actualConfig = builder.Build();
                var listActual = actualConfig.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
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
            string jsonPath = $"TestCases\\ApiSource\\Cert-ApiKey\\ConfigParam\\ConnectFail\\{testCase}.json";

            // Get initial config containing api connection and authentication info
            var initConfig = new ConfigurationBuilder().AddJsonFile(jsonPath, false).Build();

            // Create the final builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();
            IConfiguration actualConfig;
            builder.AddApiSource(initConfig, optional);

            if (optional)
            {
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"testCases\\ApiSource\\Cert-ApiKey\\ConfigParam\\ConnectFail\\expected.json"));
                actualConfig = builder.Build();
                var listActual = actualConfig.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
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
        public void Config_Cert_AuthFail(string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\Cert-ApiKey\\ConfigParam\\AuthFail\\{testCase}.json";

            // Get initial config containing api connection and authentication info
            var initConfig = new ConfigurationBuilder().AddJsonFile(jsonPath, false).Build();

            // Create the final builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();
            IConfiguration actualConfig;
            builder.AddApiSource(initConfig, optional);

            if (optional)
            {
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"testCases\\ApiSource\\Cert-ApiKey\\ConfigParam\\AuthFail\\expected{testCase}.json"));
                actualConfig = builder.Build();
                var listActual = actualConfig.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
            {
                
                Assert.Throws<System.AggregateException>(() => actualConfig = builder.Build());
            }
        }

        // Authentication Failure
        // Will fail on Build with AggregateException

        [InlineData("KeyAuthFail", true)]
        [InlineData("KeyAuthFail", false)]
        [Theory]
        public void Config_ApiKey_AuthFail(string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\Cert-ApiKey\\ConfigParam\\AuthFail\\{testCase}.json";

            // Get initial config containing api connection and authentication info
            var initConfig = new ConfigurationBuilder().AddJsonFile(jsonPath, false).Build();

            // Create the final builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();
            IConfiguration actualConfig;
            builder.AddApiSource(initConfig, optional);

            if (optional)
            {
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"testCases\\ApiSource\\Cert-ApiKey\\ConfigParam\\AuthFail\\expected{testCase}.json"));
                actualConfig = builder.Build();
                var listActual = actualConfig.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
            {
                Assert.Throws<System.Net.Http.HttpRequestException>(() => actualConfig = builder.Build());
            }
        }


        // Unable to create HTTP Request - either no secret or certificate not found
        // will fail on AddApiSource
        [InlineData("CertNoSecret", true)]
        [InlineData("CertNoSecret", false)]
        [InlineData("CertNotFound", true)]
        [InlineData("CertNotFound", false)]
        [InlineData("KeyNoSecret", true)]
        [InlineData("KeyNoSecret", false)]
        [Theory]
        public void Config_Auth_CreateRequestFail(string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\Cert-ApiKey\\ConfigParam\\CreateRequestFail\\{testCase}.json";

            // Get initial config containing api connection and authentication info
            var initConfig = new ConfigurationBuilder().AddJsonFile(jsonPath, false).Build();

            // Create the final builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();
            IConfiguration actualConfig;

            if (optional)
            {
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"testCases\\ApiSource\\Cert-ApiKey\\ConfigParam\\CreateRequestFail\\expected{testCase}.json"));

                builder.AddApiSource(initConfig, optional);
                actualConfig = builder.Build();
                var listActual = actualConfig.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(initConfig, optional));
        }




        #endregion


    }
}

