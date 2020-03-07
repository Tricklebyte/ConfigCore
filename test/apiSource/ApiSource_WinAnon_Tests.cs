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
    public class ApiSource_WinAnon_Fixture : IDisposable
    {

        //  create environment variables ############################################
        readonly List<EnvVar> _envVarList;

        public ApiSource_WinAnon_Fixture()
        {
            string path = Environment.CurrentDirectory + "\\TestCases\\ApiSource\\Anon-Win\\EnvVars.json";
            _envVarList = JsonConvert.DeserializeObject<List<EnvVar>>(File.ReadAllText(path));
            TestHelper.CreateEnvVars(_envVarList);
        }

        public void Dispose()
        {
            TestHelper.DeleteEnvVars(_envVarList);
        }
    }


    public class ApiSource_WinAnon_Tests : IClassFixture<ApiSource_WinAnon_Fixture>
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
        [InlineData("ConfigURL-Anon", "Anon", null, new string[] { "" }, true, "1")]
        [InlineData("ConfigURL-Anon", "Anon", null, new string[] { "" }, false, "1")]
        [InlineData("ConfigURL-Anon", "Anon", null, new string[] { "CustomAppName" }, true, "2")]
        [InlineData("ConfigURL-Anon", "Anon", null, new string[] { "CustomAppName" }, false, "2")]
        [InlineData("ConfigURL-Win", "Windows", null, new string[] { "" }, true, "1")]
        [InlineData("ConfigURL-Win", "Windows", null, new string[] { "" }, false, "1")]
        [InlineData("ConfigURL-Win", "Windows", null, new string[] { "CustomAppName" }, true, "2")]
        [InlineData("ConfigURL-Win", "Windows", null, new string[] { "CustomAppName" }, false, "2")]

        [Theory]
        public void RouteParams_Good(string configUrlVar, string authType, string authSecretVar, string[] routeParams, bool optional, string testCase)
        {
            var builder = new ConfigurationBuilder();

            IConfiguration actual = builder.AddApiSource(configUrlVar, authType, authSecretVar, routeParams, optional).Build();
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }

        //Overload with single required parameter for the URL of the API. Uses Windows Auth by default, optional False by default
        [InlineData("ConfigURL-Win", "1")]
        [InlineData("ConfigURL-Anon", "1")]
        [Theory]
        public void RouteParams_WinDef_Good(string configUrlVar, string testCase)
        {

            var builder = new ConfigurationBuilder();
            IConfiguration actual = builder.AddApiSource(configUrlVar).Build();
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }

        //Overload with single required parameter for the URL of the API and one optional parameter for Optional. Uses Windows Auth by default.
        [InlineData("ConfigURL-Win", true, "1")]
        [InlineData("ConfigURL-Win", false, "1")]
        [InlineData("ConfigURL-Anon", true, "1")]
        [InlineData("ConfigURL-Anon", false, "1")]
        [Theory]
        public void RouteParams_WinDefOptional_Good(string configUrlVar, bool optional, string testCase)
        {

            var builder = new ConfigurationBuilder();
            IConfiguration actual = builder.AddApiSource(configUrlVar, optional).Build();
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        // Overload with parameters for URL environment variable name and non-default Application Id
        [InlineData("ConfigURL-Win", new string[] { "CustomAppName" }, "2")]
        [InlineData("ConfigURL-Anon", new string[] { "CustomAppName" }, "2")]
        [Theory]
        public void RouteParams_WinAppId_Good(string configUrlVar, string[] routeParams, string testCase)
        {
            var builder = new ConfigurationBuilder();
            IConfiguration actual = builder.AddApiSource(configUrlVar, routeParams).Build();
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }

        // Overload with parameters for URL environment variable name,  non-default Application Id, optional parameter
        [InlineData("ConfigURL-Win", new string[] { "CustomAppName" }, true, "2")]
        [InlineData("ConfigURL-Anon", new string[] { "CustomAppName" }, true, "2")]
        [Theory]
        public void RouteParams_WinAppIdOptional_Good(string configUrlVar, string[] routeParams, bool optional, string testCase)
        {
            var builder = new ConfigurationBuilder();
            IConfiguration actual = builder.AddApiSource(configUrlVar, routeParams, optional).Build();
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        // Environment Variable for ConfigURL not found. Will fail on adding ApiSource
        [InlineData("ConfigApi-UnknownVar", "Anon", null, null, true, "1")]
        [InlineData("ConfigApi-UnknownVar", "Anon", null, null, false, "1")]
        [InlineData("ConfigApi-UnknownVar", "Windows", null, null, true, "1")]
        [InlineData("ConfigApi-UnknownVar", "Windows", null, null, false, "1")]
        [Theory]
        public void RouteParams_EnvVarNotFound(string configUrlVar, string authType, string authSecretVar, string[] routeParams, bool optional, string testCase)
        {
            var builder = new ConfigurationBuilder();
            if (optional)
            {
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"testCases\\ApiSource\\Anon-Win\\OptParams\\EnvVarNotFound\\expected{testCase}.json"));
                builder.AddApiSource(configUrlVar, authType, authSecretVar, routeParams, optional);
                IConfiguration config;
                config = builder.Build();
                var listActual = config.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, null, authType, routeParams, optional));
        }


        // Environment Variable for ConfigURL not found. Will fail on adding ApiSource
        [InlineData(null, "Anon", null, null, true, "1")]
        [InlineData(null, "Anon", null, null, false, "1")]
        [InlineData(null, "Windows", null, null, true, "1")]
        [InlineData(null, "Windows", null, null, false, "1")]
        [Theory]
        public void RouteParams_NullEnvVar(string configUrlVar, string authType, string authSecretVar, string[] routeParams, bool optional, string testCase)
        {
            var builder = new ConfigurationBuilder();
            if (optional)
            {
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"testCases\\ApiSource\\Anon-Win\\OptParams\\NullUrlVarParam\\expected{testCase}.json"));
                builder.AddApiSource(configUrlVar, authType, authSecretVar, routeParams, optional);
                IConfiguration config;
                config = builder.Build();
                var listActual = config.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, null, authType, routeParams, optional));
        }

        //Assigns a valid, incorrect Url. Will fail on connect, so error will be thrown on build.
        [InlineData("ConfigApi-WrongUrl", "Anon", null, new string[] { "" }, true, "1")]
        [InlineData("ConfigApi-WrongUrl", "Anon", null, new string[] { "" }, false, "1")]
        [InlineData("ConfigApi-WrongUrl", "Windows", null, new string[] { "" }, true, "1")]
        [InlineData("ConfigApi-WrongUrl", "Windows", null, new string[] { "" }, false, "1")]

        [Theory]
        public void RouteParams_WrongUrl(string configUrlVar, string authType, string authSecretVar, string[] routeParams, bool optional, string testCase) //FAIL ON BUILD
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddApiSource(configUrlVar, authType, authSecretVar, routeParams, optional);

            if (optional)
            {
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"testCases\\ApiSource\\Anon-Win\\OptParams\\WrongUrl\\expected{testCase}.json"));

                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else

                Assert.Throws<System.Net.Http.HttpRequestException>(() => actual = builder.Build());
        }

        // Tries to assign an invalid URI value. Will fail on adding ApiSource
        [InlineData("ConfigApi-InvalidUrl", "Anon", null, new string[] {""}, true,"1")]
        [InlineData("ConfigApi-InvalidUrl", "Anon", null, new string[] { "" }, false, "1")]
        [InlineData("ConfigApi-InvalidUrl", "Windows", null, new string[] { "" }, true, "1")]
        [InlineData("ConfigApi-InvalidUrl", "Windows", null, new string[] { "" }, false, "1")]
        [Theory]
        public void RouteParams_InvalidUrl(string configUrlVar, string authType, string authSecretVar, string[] routeParams, bool optional, string testCase)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();

            if (optional)
            {
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"testCases\\ApiSource\\Anon-Win\\OptParams\\InvalidUrl\\expected{testCase}.json"));

                builder.AddApiSource(configUrlVar, authType, authSecretVar, routeParams, optional);
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
                Assert.Throws<System.UriFormatException>(() => builder.AddApiSource(configUrlVar, authSecretVar, authType, routeParams, optional));
        }
       
        
        
        #endregion


        #region API Cient Authentication with route parameters

        [Theory]
        [InlineData("ConfigURL-Win", "ApiKey", "ConfigAuth-Key", new string[] { "" }, true,"1")]
        [InlineData("ConfigURL-Win", "ApiKey", "ConfigAuth-Key", new string[] { "" }, false,"1")]

        // Fails authentication - windows credentials not supplied to Host
        // using the windows url, with APIKey headers
        public void RouteParams_AuthFail(string configUrlVar, string authType, string authSecretVar, string[] routeParams, bool optional,string testCase)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddApiSource(configUrlVar, authType, authSecretVar, routeParams, optional);

            if (optional)
            {
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\Anon-Win\\OptParams\\AuthFail\\expected{testCase}.json"));
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
                Assert.Throws<System.Net.Http.HttpRequestException>(() => actual = builder.Build());
        }

        #endregion
        


        #region AddApiSource Overloads with Query String Parameters
        // Exact Match APP ID, AND remaining parameters

        //URL, Dict
        [Theory]
        [InlineData("ConfigURL-Anon", "Anon", "appId","testhost","idList","1,3,5," ,"1", true)]
        [InlineData("ConfigURL-Anon", "Anon", "appId", "testhost", "idList", "1,3,5,","1", false)]
        [InlineData("ConfigURL-Anon", "Anon", "appId", "CustomAppName", "idList", "6,8,10", "2", true)]
        [InlineData("ConfigURL-Anon", "Anon", "appId", "CustomAppName", "idList", "6,8,10", "2", false)]
        //TODO: ADD WIN TEST CASES
        public void QueryParams_Good(string configUrlVar, string authType,string param1Name, string param1Value, string param2Name, string param2Value, string testCase, bool optional)
        {
            var builder = new ConfigurationBuilder();
            Dictionary<string, string> dictParams = new Dictionary<string, string>();
            dictParams.Add(param1Name, param1Value);
            dictParams.Add(param2Name, param2Value);

            IConfiguration actual = builder.AddApiSource(configUrlVar, authType, null, dictParams, optional).Build();

            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\Anon-Win\\QueryParams\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }

        
        
        [Theory]
        [InlineData("ConfigURL-Anon", "Anon", "appId", "testhost", "idList", null, "1", true)]
        [InlineData("ConfigURL-Anon", "Anon", "appId", "testhost", "idList", null, "1", false)]
        [InlineData("ConfigURL-Anon", "Anon", "appId", "CustomAppName", "idList", null, "2", true)]
        [InlineData("ConfigURL-Anon", "Anon", "appId", "CustomAppName", "idList", null, "2", false)]
        public void QueryParams_InvalidParams(string configUrlVar, string authType, string param1Name, string param1Value, string param2Name, string param2Value, string testCase, bool optional)
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
                Assert.Throws<System.ArgumentNullException>(() => builder.AddApiSource(configUrlVar, authType, null,dictParams, optional));
        }



        #endregion


        #region AddApiSource Overloads with IConfiguration Parameter
        // Loads settings from Configuration section "ConfigOptions:ApiSource" to override default comm and auth settings for the HTTP Client.
        // Two good test cases for each authentication type, tested for optional true and false
        [InlineData( "1", true)]
        [InlineData( "1", false)]
        [InlineData("2", true)]
        [InlineData( "2", false)]

        [Theory]
        public void Config_Good(string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\Anon-Win\\ConfigParam\\Good\\input{testCase}.json";

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
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\Anon-Win\\ConfigParam\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }
        // TODO: test case 2 above has mixed up hostname

        //Environment Variable for ConfigURL not found. Will fail on adding ApiSource
        [InlineData("1", true)]
        [InlineData("1", false)]
        [Theory]
        public void Config_SectionNotFound(string testCase, bool optional)
        {
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"testCases\\ApiSource\\Anon-Win\\ConfigParam\\SectionNotFound\\expected{testCase}.json"));

            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\Anon-Win\\ConfigParam\\SectionNotFound\\input{testCase}.json";

            // Get initial config containing non-default database settings
            var initConfig = new ConfigurationBuilder().AddJsonFile(jsonPath, false).Build();

            // Create the final builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();

            if (optional)
            {
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
        [InlineData("1", true)]
        [InlineData("1", false)]
        [Theory]
        public void Config_ConnectFail(string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonInput = $"TestCases\\ApiSource\\Anon-Win\\ConfigParam\\ConnectFail\\input{testCase}.json";

            // Get initial config containing api connection and authentication info
            var initConfig = new ConfigurationBuilder().AddJsonFile(jsonInput, false).Build();

            // Create the final builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();
            IConfiguration configActual;
            builder.AddApiSource(initConfig, optional);

            if (optional)
            {
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"testCases\\ApiSource\\Anon-Win\\ConfigParam\\ConnectFail\\expected{testCase}.json"));

                configActual = builder.Build();
                var listActual = configActual.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
            {
                Assert.Throws<System.Net.Http.HttpRequestException>(() => configActual = builder.Build());
            }
        }

        // Tries to assign an invalid URI value. Will fail on adding ApiSource

        [InlineData("1", true)]
        [InlineData("1", false)]
        [Theory]
        public void Config_InvalidUrl(string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\Anon-Win\\ConfigParam\\InvalidUrl\\input{testCase}.json";
            
            // Get initial config containing api connection and authentication info
            var initConfig = new ConfigurationBuilder().AddJsonFile(jsonPath, false).Build();

            // Create the final builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();
            IConfiguration actualConfig;

            if (optional)
            {
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\Anon-Win\\ConfigParam\\InvalidUrl\\expected{testCase}.json"));
               
                builder.AddApiSource(initConfig, optional);
                actualConfig = builder.Build();
                var listActual = actualConfig.GetConfigSettings();
                
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
                Assert.Throws<System.UriFormatException>(() => builder.AddApiSource(initConfig, optional));
        }


        [InlineData("1", true)]
        [InlineData("1", false)]
        [InlineData("2", true)]
        [InlineData("2", false)]
        [Theory]
        public void Config_AuthFail(string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\Anon-Win\\ConfigParam\\AuthFail\\input{testCase}.json";

            // Get initial config containing api connection and authentication info
            var initConfig = new ConfigurationBuilder().AddJsonFile(jsonPath, false).Build();

            // Create the final builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();
            IConfiguration actualConfig;
            builder.AddApiSource(initConfig, optional);

            if (optional)
            {
                actualConfig = builder.Build();
                var listActual = actualConfig.GetConfigSettings();
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\Anon-Win\\ConfigParam\\AuthFail\\expected{testCase}.json"));
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
            {
                Assert.Throws<System.Net.Http.HttpRequestException>(() => actualConfig = builder.Build());
            }
        }


        #endregion





    }
}

