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
{

    // REQUIREMENTS:
    // The following environment variables must be present in the test environment.
    // 
    // 1.) ConfigApi-Url - The correct full URL of the GET{appId} method action without parameters
    //                 Sample project value: https://localhost:44397/iapi/ConfigSettings/
    //                 
    // 2.) ConfigApi-WrongUrl - A validly formatted, but incorrect URL
    //                 Sample project value: https://localhost:44397/iapi/unknown/
    // 
    // 3.) ConfigApi-InvalidUrl - An invalid URL 
    //                 Sample project value: https://localhost:12345/
    public class ApiSourceTests
    {      
        
        #region Config API URL Environment Variable Name Parameter
        /// <summary>

        ///                
        /// </summary>
        /// <param name="configUrlVar"></param>
        /// <param name="optional"></param>
        /// <param name="testCase"></param>


        // Test overload where environment variable is only parameter
        // 
        [InlineData("ConfigApi-Url", true, "1")]
        [InlineData("ConfigApi-Url", false, "1")]
        [Theory]
        public void EnvVar_Good(string configUrlVar, bool optional, string testCase)
        {
            var builder = new ConfigurationBuilder();
            IConfiguration actual = builder.AddApiSource(configUrlVar, optional).Build();
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\AddApiSource\\EnvVar\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }

        //
        [InlineData("ConfigApi-UnknownVar", true)]
        [InlineData("ConfigApi-UnknownVar", false)]
        [Theory]
        public void EnvVar_VarNotFound(string configUrlVar, bool optional)
        {
            var builder = new ConfigurationBuilder();
            if (optional)
            {
                builder.AddApiSource(configUrlVar, optional);
                IConfiguration config;
                config = builder.Build();
                var actualList = config.GetConfigSettings();
                Assert.True(actualList.Count == 0);
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, optional));
        }

        //Assigns a valid, incorrect Url
        [InlineData("ConfigApi-WrongUrl", true)]
        [InlineData("ConfigApi-WrongUrl", false)]
        [Theory]
        public void EnvVar_WrongUrl(string configUrlVar, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddApiSource(configUrlVar, optional);

            if (optional)
            {
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.Net.Http.HttpRequestException>(() => actual = builder.Build());
        }


        // Tries to assign an invalid URI value
        [InlineData("ConfigApi-InvalidUrl", true)]
        [InlineData("ConfigApi-InvalidUrl", false)]
        [Theory]
        public void EnvVar_InvalidUrl(string configUrlVar, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();

            if (optional)
            {
                builder.AddApiSource(configUrlVar, optional);
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.UriFormatException>(() => builder.AddApiSource(configUrlVar, optional));
        }

        #endregion



        #region Overload with ConfigApiUrl Environment Variable Name and App Id Parameters

        [InlineData("ConfigApi-Url","testhost", true, "1")]
        [InlineData("ConfigApi-Url", "testhost",false, "1")]
        [Theory]
        public void EnvVarAppId_Good(string configUrlVar, string appId,bool optional, string testCase)
        {
            var builder = new ConfigurationBuilder();
            IConfiguration actual = builder.AddApiSource(configUrlVar,appId, optional).Build();
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\AddApiSource\\EnvVar\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }
        
        //Environment Variable for ConfigURL not found. Will fail on adding ApiSource
        [InlineData("ConfigApi-UnknownVar","testhost", true)]
        [InlineData("ConfigApi-UnknownVar", "testhost",false)]
        [Theory]
        public void EnvVarAppId_VarNotFound(string configUrlVar,string appId, bool optional)
        {
            var builder = new ConfigurationBuilder();
            if (optional)
            {
                builder.AddApiSource(configUrlVar,appId, optional);
                IConfiguration config;
                config = builder.Build();
                var actualList = config.GetConfigSettings();
                Assert.True(actualList.Count == 0);
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar,appId, optional));
        }

        //Assigns a valid, incorrect Url. Will fail on connect, so error will be thrown on build.
        [InlineData("ConfigApi-WrongUrl", "testhost",true)]
        [InlineData("ConfigApi-WrongUrl", "testhost",false)]
        [Theory]
        public void EnvVarAppId_WrongUrl(string configUrlVar,string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddApiSource(configUrlVar,appId, optional);

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
        [InlineData("ConfigApi-InvalidUrl","testhost", true)]
        [InlineData("ConfigApi-InvalidUrl", "testhost", false)]
        [Theory]
        public void EnvVarAppId_InvalidUrl(string configUrlVar, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();

            if (optional)
            {
                builder.AddApiSource(configUrlVar,appId, optional);
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.UriFormatException>(() => builder.AddApiSource(configUrlVar, optional));
        }

        #endregion



        #region Overload with IConfiguration Object Parameter
        // Loads settings from Configuration section "ConfigOptions:ApiSource" to override default comm and auth settings for the HTTP Client.

        [InlineData("1", true)]
        [InlineData("1", false)]
        [InlineData("2", true)]
        [InlineData("2", false)]
        [Theory]
        public void Config_Good(string testCase, bool optional )
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\AddApiSource\\Config\\Good\\appsettings{testCase}.json";

            // Get initial config containing non-default database settings
            var initialConfig = TestHelper.GetFileConfig(jsonPath);


            // Create the final builder 
            IConfigurationBuilder finalBuilder = new ConfigurationBuilder();

            // Add the DBSource to the final builder
            finalBuilder.AddApiSource(initialConfig, optional);

            // Build the final config
            var actual = finalBuilder.Build();

            // Convert to lists and compare
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\AddApiSource\\Config\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }

        //Environment Variable for ConfigURL not found. Will fail on adding ApiSource
        [InlineData("ConfigApi-UnknownVar", "testhost", true)]
        [InlineData("ConfigApi-UnknownVar", "testhost", false)]
        [Theory]
        public void Config_SectionNotFound(string configUrlVar, string appId, bool optional)
        {
            var builder = new ConfigurationBuilder();
            if (optional)
            {
                builder.AddApiSource(configUrlVar, appId, optional);
                IConfiguration config;
                config = builder.Build();
                var actualList = config.GetConfigSettings();
                Assert.True(actualList.Count == 0);
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, appId, optional));
        }

        //Assigns a valid, incorrect Url. Will fail on connect, so error will be thrown on build.
        [InlineData("ConfigApi-WrongUrl", "testhost", true)]
        [InlineData("ConfigApi-WrongUrl", "testhost", false)]
        [Theory]
        public void Config_WrongUrl(string configUrlVar, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddApiSource(configUrlVar, appId, optional);

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
        [InlineData("ConfigApi-InvalidUrl", "testhost", true)]
        [InlineData("ConfigApi-InvalidUrl", "testhost", false)]
        [Theory]
        public void Config_InvalidUrl(string configUrlVar, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();

            if (optional)
            {
                builder.AddApiSource(configUrlVar, appId, optional);
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count == 0);
            }
            else
                Assert.Throws<System.UriFormatException>(() => builder.AddApiSource(configUrlVar, optional));
        }

        #endregion

    }
}
