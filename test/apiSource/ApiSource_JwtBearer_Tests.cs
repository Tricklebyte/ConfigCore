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
    public class ApiSource_JwtBearer_Fixture : IDisposable
    {

        //  create environment variables ############################################
        readonly List<EnvVar> _envVarList;

        public ApiSource_JwtBearer_Fixture()
        {
            string path = Environment.CurrentDirectory + "\\TestCases\\ApiSource\\JwtBearer\\EnvVars.json";
            _envVarList = JsonConvert.DeserializeObject<List<EnvVar>>(File.ReadAllText(path));
            TestHelper.CreateEnvVars(_envVarList);
        }

        public void Dispose()
        {
            TestHelper.DeleteEnvVars(_envVarList);
        }
    }



    public class ApiSource_JwtBearer_Tests : IClassFixture<ApiSource_JwtBearer_Fixture>
    {

        #region Route Parameter Data tests
        // Default param
        // Route param
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "1")]
        [Theory]
        public void JwtBearer_RParam_Default_Good(string configUrlVar, string authorityVar, string clientIdVar, string clientSecretVar, string clientScopeVar, string testCase)
        {

            // Create the  builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();

            // Add the DBSource to the final builder
            builder.AddApiSource(configUrlVar, authorityVar, clientIdVar, clientSecretVar, clientScopeVar);

            // Build the final config
            var actual = builder.Build();

            // Convert to lists and compare
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\JwtBearer\\OptParams\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "1", true)]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "1", false)]
        [Theory]
        public void JwtBearer_RParam_Default_Good_Optional(string configUrlVar, string authorityVar, string clientIdVar, string clientSecretVar, string clientScopeVar, string testCase, bool optional)
        {

            // Create the  builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();

            // Add the DBSource to the final builder
            builder.AddApiSource(configUrlVar, authorityVar, clientIdVar, clientSecretVar, clientScopeVar, optional);

            // Build the final config
            var actual = builder.Build();

            // Convert to lists and compare
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\JwtBearer\\OptParams\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "", "1")]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope",  "testhost", "1")]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "ConfigApiClient_JwtBearer", "2")]
        [Theory]
        public void JwtBearer_RParam_Good(string configUrlVar, string authorityVar, string clientIdVar, string clientSecretVar, string clientScopeVar, string appId, string testCase)
        {

            // Create the  builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();

            // Add the DBSource to the final builder
            builder.AddApiSource(configUrlVar, authorityVar, clientIdVar, clientSecretVar, clientScopeVar, new string[] { appId });

            // Build the final config
            var actual = builder.Build();

            // Convert to lists and compare
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\JwtBearer\\OptParams\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "", "1", true)]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "", "1", false)]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "testhost", "1", true)]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "testhost", "1", false)]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "ConfigApiClient_JwtBearer", "2", true)]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "ConfigApiClient_JwtBearer", "2", false)]
        [Theory]
        public void JwtBearer_RParam_Good_Optional(string configUrlVar, string authorityVar, string clientIdVar, string clientSecretVar, string clientScopeVar, string appId, string testCase, bool optional)
        {

            // Create the  builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();

            // Add the DBSource to the final builder
            builder.AddApiSource(configUrlVar,  authorityVar, clientIdVar, clientSecretVar, clientScopeVar, new string[] { appId }, optional);

            // Build the final config
            var actual = builder.Build();

            // Convert to lists and compare
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\JwtBearer\\OptParams\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }

        #endregion

        #region Query Parameter Data Tests
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "appId", "testhost", "idList", "1,3,5", "1")]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "appId", "ConfigApiClient_JwtBearer", "idList", "6,8,10", "2")]
        [Theory]
        public void JwtBearer_QParam_Good(string configUrlVar, string authorityVar, string clientIdVar, string clientSecretVar, string clientScopeVar, string param1Name, string param1Value, string param2Name, string param2Value, string testCase)
        {
            var builder = new ConfigurationBuilder();
            Dictionary<string, string> dictParams = new Dictionary<string, string>();
            dictParams.Add(param1Name, param1Value);
            dictParams.Add(param2Name, param2Value);

            IConfiguration actual = builder.AddApiSource(configUrlVar, authorityVar, clientIdVar, clientSecretVar, clientScopeVar, dictParams).Build();

            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\JwtBearer\\QueryParams\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "appId", "testhost", "idList", "1,3,5", "1", true)]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "appId", "testhost", "idList", "1,3,5", "1", false)]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "appId", "ConfigApiClient_JwtBearer", "idList", "1,3,5", "2", true)]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "appId", "ConfigApiClient_JwtBearer", "idList", "1,3,5", "2", false)]
        [Theory]
        public void JwtBearer_QParam_Good_Optional(string configUrlVar, string authorityVar, string clientIdVar, string clientSecretVar, string clientScopeVar, string param1Name, string param1Value, string param2Name, string param2Value, string testCase, bool optional)
        {
            var builder = new ConfigurationBuilder();
            Dictionary<string, string> dictParams = new Dictionary<string, string>();
            dictParams.Add(param1Name, param1Value);
            dictParams.Add(param2Name, param2Value);

            IConfiguration actual = builder.AddApiSource(configUrlVar, authorityVar, clientIdVar, clientSecretVar, clientScopeVar, dictParams, optional).Build();

            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\JwtBearer\\QueryParams\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }

        #endregion

        #region ConfigParam Data Tests

        [InlineData("RParam", "1", true)]
        [InlineData("RParam", "1", false)]
        [InlineData("RParam", "2", true)]
        [InlineData("RParam", "2", false)]
        [InlineData("QParam", "1", true)]
        [InlineData("QParam", "1", false)]
        [InlineData("QParam", "2", true)]
        [InlineData("QParam", "2", false)]
        [Theory]
        public void JwtBearer_ConfigParam_Good(string testArgType, string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\ApiSource\\JwtBearer\\ConfigParam\\Good\\input{testArgType}{testCase}.json";

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
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\JwtBearer\\ConfigParam\\Good\\expected{testArgType}{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }



        #endregion


        #region IDP Fail tests

        //AuthFormatFail
        [Theory]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerInvalidFormatAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "1", true)]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerInvalidFormatAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope","1", false)]
        public void JwtBearer_InvalidFormatAuthority(string configUrlVar, string authorityVar, string clientIdVar, string clientSecretVar, string clientScopeVar, string testCase, bool optional)
        {
            IConfiguration actual;
            // Create the  builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();

            if (optional)
            {
                builder.AddApiSource(configUrlVar, authorityVar, clientIdVar, clientSecretVar, clientScopeVar, optional);
                actual = builder.Build();
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\JwtBearer\\OptParams\\InvalidFormatAuthority\\expected{testCase}.json"));
                var listActual = actual.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
                Assert.Throws<System.ArgumentException>(() => builder.AddApiSource(configUrlVar, authorityVar, clientIdVar, clientSecretVar, clientScopeVar, optional));
        }

        //AuthConnectFail
        //AuthFailScope
        [Theory]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerInvalidAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "1", true)]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerInvalidAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "1", false)]

        public void JwtBearer_WrongAuthority(string configUrlVar, string authorityVar, string clientIdVar, string clientSecretVar, string clientScopeVar, string testCase, bool optional)
        {
            IConfiguration actual;
            // Create the  builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();

            if (optional)
            {
                builder.AddApiSource(configUrlVar, authorityVar, clientIdVar, clientSecretVar, clientScopeVar, optional);
                actual = builder.Build();
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\JwtBearer\\OptParams\\WrongAuthority\\expected{testCase}.json"));
                var listActual = actual.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, authorityVar, clientIdVar, clientSecretVar, clientScopeVar, optional));
        }



        //AuthFailScope
        [Theory]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerInvalidScope", "1", true)]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerInvalidScope", "1", false)]

        public void JwtBearer_InvalidScope(string configUrlVar, string authorityVar, string clientIdVar, string clientSecretVar, string clientScopeVar, string testCase, bool optional)
        {
            IConfiguration actual;
            // Create the  builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();

            if (optional)
            {
                builder.AddApiSource(configUrlVar, authorityVar, clientIdVar, clientSecretVar, clientScopeVar, optional);
                actual = builder.Build();
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\JwtBearer\\OptParams\\BadScope\\expected{testCase}.json"));
                var listActual = actual.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, authorityVar, clientIdVar, clientSecretVar, clientScopeVar, optional));
        }


        //AuthFail Client Id
        [Theory]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerInvalidClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "1", true)]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerInvalidClientId", "ConfigApi-JwtBearerClientSecret", "ConfigApi-JwtBearerClientScope", "1", false)]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerInvalidSecret", "ConfigApi-JwtBearerClientScope", "1", true)]
        [InlineData("ConfigURL-JwtBearer", "ConfigApi-JwtBearerAuthority", "ConfigApi-JwtBearerClientId", "ConfigApi-JwtBearerInvalidSecret", "ConfigApi-JwtBearerClientScope", "1", false)]

        public void JwtBearer_BadClient(string configUrlVar, string authority, string clientId, string clientSecret, string scope, string testCase, bool optional)
        {
            IConfiguration actual;
            // Create the  builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();   

            if (optional)
            {            
                builder.AddApiSource(configUrlVar, authority, clientId, clientSecret, scope, optional);
                actual = builder.Build();
                var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\JwtBearer\\OptParams\\BadClient\\expected{testCase}.json"));
                var listActual = actual.GetConfigSettings();
                Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, authority, clientId, clientSecret, scope, optional));
        }

      
        #endregion

      
        

        //WrongURL
        //AuthFail
        //AuthConnectFail
        //AuthMissing
        //ClientId Missing
        //Incorrect Client Id
        //ClientSecret Missing
        //Incorrect Client Secret
        //Client scope missing
        //Incorrect Client Scope


        //JwtBearer - CONFIG PARAM Good
        //WrongURL
        //AuthFail
        //AuthConnectFail
        //AuthMissing
        //ClientId Missing
        //Incorrect Client Id
        //ClientSecret Missing
        //Incorrect Client Secret
        //Client scope missing
        //Incorrect Client Scope


    }









    //#region Api Client Authentication JwtBearer with options parameters
    //[InlineData("ConfigURL-JwtBearer", "https://demo.identityserver.io", "m2m.short", "secret", "api", "testhost", "1", true)]
    //[InlineData("ConfigURL-JwtBearer", "https://demo.identityserver.io", "m2m.short", "secret", "api", "testhost", "1", false)]
    //[Theory]
    //public void OptParams_JwtBearer_Good(string configUrlVar, string authority, string clientId, string clientSecret, string scope, string appId, string testCase, bool optional)
    //{
    //    // Create the  builder 
    //    IConfigurationBuilder builder = new ConfigurationBuilder();

    //    // Add the DBSource to the final builder
    //    builder.AddApiSource(configUrlVar, authority, clientId, clientSecret, scope, appId, optional);

    //    // Build the final config
    //    var actual = builder.Build();

    //    // Convert to lists and compare
    //    var listActual = actual.GetConfigSettings();
    //    var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\Config\\Good\\expected{testCase}.json"));
    //    Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
    //}

    ////TODO FIX
    ////[InlineData("ConfigURL-JwtBearer", "https://demo.identityserver.io", "m2m.short", "secret", "api", "testhost", "1", true)]
    ////[InlineData("ConfigURL-JwtBearer", "https://demo.identityserver.io", "m2m.short", "secret", "api", "testhost", "1", false)]
    ////[Theory]
    //public void QParams_JwtBearer_Good(string configUrlVar, string authority, string clientId, string clientSecret, string scope, string appId, string testCase, bool optional)
    //{
    //    var builder = new ConfigurationBuilder();
    //    // Create the Bearer Config options
    //    JWTBearerOptions bConfig = new JWTBearerOptions(authority, clientId, clientSecret, scope);
    //    Dictionary<string, string> dictParams = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText($"TestCases\\ApiSource\\QueryStringParams\\Good\\input{testCase}.json"));

    //    IConfiguration actual = builder.AddApiSource(configUrlVar, authority, clientId, clientSecret, scope, dictParams, optional).Build();

    //}

    //// Badly formed Api URL
    //[InlineData("ConfigURL-JwtBearer", "httpx://demo.identityserver.io", "m2m.short", "secret", "api", "testhost", "1", true)]
    //[InlineData("ConfigURL-JwtBearer", "httpx://demo.identityserver.io", "m2m.short", "secret", "api", "testhost", "1", false)]
    //[Theory]
    //public void OptParams_JwtBearer_BadAuthUrl(string configUrlVar, string authority, string clientId, string clientSecret, string scope, string appId, string testCase, bool optional)
    //{
    //    // Create the  builder 
    //    IConfigurationBuilder builder = new ConfigurationBuilder();


    //    IConfiguration actualConfig;

    //    if (optional)
    //    {
    //        builder.AddApiSource(configUrlVar, authority, clientId, clientSecret, scope, appId, optional);
    //        actualConfig = builder.Build();
    //        var listActual = actualConfig.GetConfigSettings();
    //        Assert.True(listActual.Count == 1);
    //        //TODO  assert value of error metadata returned
    //    }
    //    else
    //        Assert.Throws<System.Exception>(() => builder.AddApiSource(configUrlVar, authority, clientId, clientSecret, scope, appId, optional));

    //    try
    //    {


    //        // Add the DBSource to the final builder


    //        // Build the final config
    //        var actual = builder.Build();


    //        // Convert to lists and compare
    //        var listActual = actual.GetConfigSettings();
    //        var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\Config\\Good\\expected{testCase}.json"));
    //        Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
    //    }
    //    catch (Exception e)
    //    {

    //    }
    //}


    // Missing ConfigURL-Bearer EnvVar
    // Missing ConfigURL Val
    // Missing Authority url
    // Badly formed authority url
    // incorrect authority url
    // missing client Id
    // missing client secret
    // missing scope




    //  #endregion






}

