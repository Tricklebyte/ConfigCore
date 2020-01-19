using ConfigCore.Cryptography;
using ConfigCore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace ConfigCore.Tests
{
    public class IConfigurationExtensionTests : DataProtectionFixture
    {
        [InlineData("1")]
        [Theory]
       public void Decrypt(string testCase)
        {

         //create crypto helper using test provider that was created in fixture
          ICryptoHelper cryptoHelper = new CryptoHelper(Provider);

        // Get configuration from test folder - not for encryption settings - those are handled by fixture. 
        // Create path to appsettings file
        string jsonPath = $"TestCases\\IConfigurationExtensions\\Decrypt\\appsettings{testCase}.json";

        // Get initial config containing non-default database settings
        var configEncrypted = TestHelper.GetFileConfig(jsonPath);
        var configDecrypted = configEncrypted.Decrypt(cryptoHelper);
        var listActual = configDecrypted.GetConfigSettings();
        var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\IConfigurationExtensions\\Decrypt\\expected{testCase}.json"));
           
            Assert.True(TestHelper.SettingsAreEqual(listExpected, listActual));

            // get expected settings list

            // assert


        }
        //Decrypt
        // build configuration
        // decrypt configuration
        // convert config to actual settings list
        // get expected settings list
        // assert

    }
}
