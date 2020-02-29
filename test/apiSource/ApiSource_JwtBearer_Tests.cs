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
            string path = Environment.CurrentDirectory + "\\TestCases\\ApiSource\\JwtBearerEnvVars.json";
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
        [InlineData("ConfigURL-Bearer", "https://demo.identityserver.io", "m2m.short", "secret", "api", "testhost", "1", true)]
        [InlineData("ConfigURL-Bearer", "https://demo.identityserver.io", "m2m.short", "secret", "api", "testhost", "1", false)]
        [Theory]
        public void JwtBearer_OptParams_Good(string configUrlVar, string authority, string clientId, string clientSecret, string scope, string appId, string testCase, bool optional)
        {
   
            // Create the  builder 
            IConfigurationBuilder builder = new ConfigurationBuilder();

            // Add the DBSource to the final builder
            builder.AddApiSource(configUrlVar, authority, clientId, clientSecret, scope, new string[] { appId }, optional);

            // Build the final config
            var actual = builder.Build();

            // Convert to lists and compare
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\ApiSource\\Config\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }

        //JwtBearer ConfigParams Good
                    
       

        //JwtBearer QParam Good

        
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

