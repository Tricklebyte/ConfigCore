using ConfigCore.Models;
using ConfigCore.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace ConfigCore.Tests
{

    // REQUIREMENTS:
    // The following environment variables must be present in the test environment.
    // 
    // 1.) ConfigDb-Connection - The correct connection string of the configuration db
    //                 test value: Server=(localdb)\MSSQLLocalDB;Database=ConfigDb;Trusted_Connection=True;MultipleActiveResultSets=true
    //                 
    // 2.) ConfigDb-BadConnection - An incorrect connection string
    //                 test value: BadConnectionString
    // 
    public class DbSourceFixture : IDisposable
    {
        //  create environment variables ############################################
        List<EnvVar> _envVarList;
        public DbSourceFixture()
        {
            string path = Environment.CurrentDirectory + "\\TestCases\\DbSource\\DbEnvVars.json";
            _envVarList = JsonConvert.DeserializeObject<List<EnvVar>>(File.ReadAllText(path));
            TestHelper.CreateEnvVars(_envVarList);
        }
        public void Dispose()
        {
            TestHelper.DeleteEnvVars(_envVarList);
        }
    }

    public class DbSourceTests : DbSourceFixture
    {
        #region  Connection String Environment Variable Name Parameter
        // 
        [InlineData("ConfigDb-Connection", "1")]
        [Theory]
        public void EnvVar_Good(string conStringVar, string testCase)
        {
            var builder = new ConfigurationBuilder();
            //call AddDbSource extension method and build actual configuration
            IConfiguration actual = builder.AddDbSource(conStringVar).Build();
            //convert to list for comparison
            var listActual = actual.GetConfigSettings();
            //get expected settings list from file
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\DbSource\\AddDbSource\\EnvVar\\Good\\expected{testCase}.json"));
            //assert that lists are equal
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }

        [InlineData("UnknownVar")]
        [Theory]
        public void EnvVar_VarNotFound(string conStringVar)
        {
            var builder = new ConfigurationBuilder();
            Assert.Throws<System.Exception>(() => builder.AddDbSource(conStringVar));
        }

        [InlineData("ConfigDb-BadConnection")]
        [Theory]
        public void EnvVar_ConnectFail(string conStringVar)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddDbSource(conStringVar);
            Assert.Throws<System.ArgumentException>(() => actual = builder.Build());
        }

        #endregion

        #region Convar Optional

        // 
        [InlineData("ConfigDb-Connection", true, "1")]
        [InlineData("ConfigDb-Connection", false, "1")]
        [Theory]
        public void EnvVarOpt_Good(string conStringVar, bool optional, string testCase)
        {
            var builder = new ConfigurationBuilder();
            //call AddDbSource extension method and build actual configuration
            IConfiguration actual = builder.AddDbSource(conStringVar, optional).Build();
            //convert to list for comparison
            var listActual = actual.GetConfigSettings();
            //get expected settings list from file
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\DbSource\\AddDbSource\\EnvVar\\Good\\expected{testCase}.json"));
            //assert that lists are equal
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }

        [InlineData("UnknownVar", true)]
        [InlineData("UnknownVar", false)]
        [Theory]
        public void EnvVarOpt_VarNotFound(string conStringVar, bool optional)
        {
            var builder = new ConfigurationBuilder();
            if (optional)
            {
                // attempt to call .AddDbSource with a 
                builder.AddDbSource(conStringVar, optional);
                IConfiguration config;
                config = builder.Build();

                var actualList = config.GetConfigSettings();
                Assert.True(actualList.Count() == 0);
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddDbSource(conStringVar, optional));
        }

        [InlineData("ConfigDb-BadConnection", true)]
        [InlineData("ConfigDb-BadConnection", false)]
        [Theory]
        public void EnvVarOpt_ConnectFail(string conStringVar, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddDbSource(conStringVar, optional);

            if (optional)
            {
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count() == 0);
            }
            else
                Assert.Throws<System.ArgumentException>(() => actual = builder.Build());
        }

        #endregion

        #region Connection String Environment Variable Name and AppId Parameters

        // test overload with EnvVar and AppId parameters 
        [InlineData("ConfigDb-Connection", "testhost", "1")]
        [Theory]
        public void EnvVar_AppId_Good(string conStringVar, string appId, string testCase)
        {
            var builder = new ConfigurationBuilder();
            //build actual configuraiton
            IConfiguration actual = builder.AddDbSource(conStringVar, appId).Build();
            //convert actual configuration result to list for comparison
            var listActual = actual.GetConfigSettings();
            //get expected list of results from file
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\DbSource\\AddDbSource\\EnvVar_AppId\\Good\\expected{testCase}.json"));
            // assert lists are equal.
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        [InlineData("UnknownVar", "testhost")]
        [Theory]
        public void EnvVar_AppId_EnvVarNotFound(string conStringVar, string appId)
        {
            var builder = new ConfigurationBuilder();

            Assert.Throws<System.Exception>(() => builder.AddDbSource(conStringVar, appId));
        }

        [InlineData("ConfigDb-BadConnection", "testhost")]
        [Theory]
        public void EnvVar_AppId_ConnectFail(string conStringVar, string appId)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddDbSource(conStringVar, appId);

            Assert.Throws<System.ArgumentException>(() => actual = builder.Build());
        }

        [InlineData("ConfigDb-Connection", "UnknownApp")]
        [Theory]
        public void EnvVar_AppId_NoResults(string conStringVar, string appId)
        {
            var builder = new ConfigurationBuilder();
            IConfiguration actual = builder.AddDbSource(conStringVar, appId).Build();
            var listActual = actual.GetConfigSettings();
            Assert.True(listActual.Count() == 0);
        }


        #endregion


        #region Convar AppId Optional
        // test overload with EnvVar and AppId parameters 
        [InlineData("ConfigDb-Connection", "testhost", true, "1")]
        [InlineData("ConfigDb-Connection", "testhost", false, "1")]
        [Theory]
        public void EnvVarAppIdOpt_Good(string conStringVar, string appId, bool optional, string testCase)
        {
            var builder = new ConfigurationBuilder();
            //build actual configuraiton
            IConfiguration actual = builder.AddDbSource(conStringVar, appId, optional).Build();
            //convert actual configuration result to list for comparison
            var listActual = actual.GetConfigSettings();
            //get expected list of results from file
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\DbSource\\AddDbSource\\EnvVar_AppId\\Good\\expected{testCase}.json"));
            // assert lists are equal.
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        [InlineData("UnknownVar", "testhost", true)]
        [InlineData("UnknownVar", "testhost", false)]
        [Theory]
        public void EnvVarAppIdopt_EnvVarNotFound(string conStringVar, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            if (optional)
            {
                builder.AddDbSource(conStringVar, appId, optional);
                actual = builder.Build();
                var actualList = actual.GetConfigSettings();
                Assert.True(actualList.Count() == 0);
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddDbSource(conStringVar, appId, optional));
        }

        [InlineData("ConfigDb-BadConnection", "testhost", true)]
        [InlineData("ConfigDb-BadConnection", "testhost", false)]
        [Theory]
        public void EnvVar_AppIdOpt_ConnectFail(string conStringVar, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddDbSource(conStringVar, appId, optional);

            if (optional)
            {
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count() == 0);
            }
            else
            {
                Assert.Throws<System.ArgumentException>(() => actual = builder.Build());
            }
        }

        [InlineData("ConfigDb-Connection", "UnknownApp", true)]
        [InlineData("ConfigDb-Connection", "UnknownApp", false)]
        [Theory]
        public void EnvVar_AppIdOpt_NoResults(string conStringVar, string appId, bool optional)
        {
            var builder = new ConfigurationBuilder();
            IConfiguration actual = builder.AddDbSource(conStringVar, appId, 0, optional).Build();
            var listActual = actual.GetConfigSettings();
            Assert.True(listActual.Count() == 0);
        }

        #endregion

        #region Convar SqlTimeout
        // test overload with EnvVar and AppId parameters 
        [InlineData("ConfigDb-Connection", 0, "1")]
        [Theory]
        public void EnvVarSqlTo_Good(string conStringVar, int sqlTimeout, string testCase)
        {
            var builder = new ConfigurationBuilder();
            //build actual configuraiton
            IConfiguration actual = builder.AddDbSource(conStringVar, sqlTimeout).Build();
            //convert actual configuration result to list for comparison
            var listActual = actual.GetConfigSettings();
            //get expected list of results from file
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\DbSource\\AddDbSource\\EnvVar_AppId\\Good\\expected{testCase}.json"));
            // assert lists are equal.
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        [InlineData("UnknownVar", 0)]

        [Theory]
        public void EnvVarSqlTo_EnvVarNotFound(string conStringVar, int sqlTimeout)
        {
            var builder = new ConfigurationBuilder();
            Assert.Throws<System.Exception>(() => builder.AddDbSource(conStringVar, sqlTimeout));
        }

        [InlineData("ConfigDb-BadConnection", 0)]
        [Theory]
        public void EnvVarSqlTo_ConnectFail(string conStringVar, int sqlTimeout)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddDbSource(conStringVar, sqlTimeout);

            Assert.Throws<System.ArgumentException>(() => actual = builder.Build());
        }

   
       

        #endregion

        #region Convar SqlTimeout Optional
        // test overload with EnvVar and AppId parameters 
        [InlineData("ConfigDb-Connection", 0,true,"1")]
        [InlineData("ConfigDb-Connection", 0, false, "1")]
        [Theory]
        public void EnvVarSqlToOpt_Good(string conStringVar,int sqlTimeout, bool optional, string testCase)
        {
            var builder = new ConfigurationBuilder();
            //build actual configuraiton
            IConfiguration actual = builder.AddDbSource(conStringVar, sqlTimeout, optional).Build();
            //convert actual configuration result to list for comparison
            var listActual = actual.GetConfigSettings();
            //get expected list of results from file
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\DbSource\\AddDbSource\\EnvVar_AppId\\Good\\expected{testCase}.json"));
            // assert lists are equal.
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        [InlineData("UnknownVar",0, true)]
        [InlineData("UnknownVar",0, false)]
        [Theory]
        public void EnvVarSqlToOpt_EnvVarNotFound(string conStringVar, int SqlTimeout, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            if (optional)
            {
                builder.AddDbSource(conStringVar, SqlTimeout, optional);
                actual = builder.Build();
                var actualList = actual.GetConfigSettings();
                Assert.True(actualList.Count() == 0);
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddDbSource(conStringVar, SqlTimeout, optional));
        }

        [InlineData("ConfigDb-BadConnection", 0, true)]
        [InlineData("ConfigDb-BadConnection", 0, false)]
        [Theory]
        public void EnvVarSqlToOpt_ConnectFail(string conStringVar, int SqlTimeout, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddDbSource(conStringVar, SqlTimeout, optional);

            if (optional)
            {
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count() == 0);
            }
            else
            {
                Assert.Throws<System.ArgumentException>(() => actual = builder.Build());
            }
        }

        #endregion

       
        #region Convar AppId SqlTimeout
        // test overload with EnvVar and AppId parameters 
        [InlineData("ConfigDb-Connection", "testhost", 0, "1")]
        [InlineData("ConfigDb-Connection", "CustomAppName", 0, "2")]
        [Theory]
        public void EnvVarAppIdSqlTo_Good(string conStringVar, string appId, int sqlTimeout, string testCase)
        {
            var builder = new ConfigurationBuilder();
            //build actual configuraiton
            IConfiguration actual = builder.AddDbSource(conStringVar, appId, sqlTimeout).Build();
            //convert actual configuration result to list for comparison
            var listActual = actual.GetConfigSettings();
            //get expected list of results from file
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\DbSource\\AddDbSource\\EnvVar_AppId\\Good\\expected{testCase}.json"));
            // assert lists are equal.
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        [InlineData("UnknownVar", "testhost", true)]
        [InlineData("UnknownVar", "testhost", false)]
        [Theory]
        public void EnvVarAppIdSqlTo_EnvVarNotFound(string conStringVar, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            if (optional)
            {
                builder.AddDbSource(conStringVar, appId, 0, optional);
                actual = builder.Build();
                var actualList = actual.GetConfigSettings();
                Assert.True(actualList.Count() == 0);
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddDbSource(conStringVar, appId, 0, optional));
        }

        [InlineData("ConfigDb-BadConnection", "testhost", true)]
        [InlineData("ConfigDb-BadConnection", "testhost", false)]
        [Theory]
        public void EnvVarAppIdSqlTo_ConnectFail(string conStringVar, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddDbSource(conStringVar, appId, 0, optional);

            if (optional)
            {
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count() == 0);
            }
            else
            {
                Assert.Throws<System.ArgumentException>(() => actual = builder.Build());
            }
        }

        [InlineData("ConfigDb-Connection", "UnknownApp", true)]
        [InlineData("ConfigDb-Connection", "UnknownApp", false)]
        [Theory]
        public void EnvVarAppIdSqlTo_NoResults(string conStringVar, string appId, bool optional)
        {
            var builder = new ConfigurationBuilder();
            IConfiguration actual = builder.AddDbSource(conStringVar, appId, 0, optional).Build();
            var listActual = actual.GetConfigSettings();
            Assert.True(listActual.Count() == 0);
        }
        #endregion


        #region Convar AppId SqlTimeout Optional
        // test overload with EnvVar and AppId parameters 
        [InlineData("ConfigDb-Connection", "testhost", true, "1")]
        [InlineData("ConfigDb-Connection", "testhost", false, "1")]
        [Theory]
        public void EnvVarAppIdSqlToOpt_Good(string conStringVar, string appId, bool optional, string testCase)
        {
            var builder = new ConfigurationBuilder();
            //build actual configuraiton
            IConfiguration actual = builder.AddDbSource(conStringVar, appId, 0, optional).Build();
            //convert actual configuration result to list for comparison
            var listActual = actual.GetConfigSettings();
            //get expected list of results from file
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\DbSource\\AddDbSource\\EnvVar_AppId\\Good\\expected{testCase}.json"));
            // assert lists are equal.
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));
        }


        [InlineData("UnknownVar", "testhost", true)]
        [InlineData("UnknownVar", "testhost", false)]
        [Theory]
        public void EnvVarAppIdSqlToOpt_EnvVarNotFound(string conStringVar, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            if (optional)
            {
                builder.AddDbSource(conStringVar, appId, 0, optional);
                actual = builder.Build();
                var actualList = actual.GetConfigSettings();
                Assert.True(actualList.Count() == 0);
            }
            else
                Assert.Throws<System.Exception>(() => builder.AddDbSource(conStringVar, appId, 0, optional));
        }

        [InlineData("ConfigDb-BadConnection", "testhost", true)]
        [InlineData("ConfigDb-BadConnection", "testhost", false)]
        [Theory]
        public void EnvVarAppIdSqlToOpt_ConnectFail(string conStringVar, string appId, bool optional)
        {
            IConfiguration actual;
            var builder = new ConfigurationBuilder();
            builder.AddDbSource(conStringVar, appId, 0, optional);

            if (optional)
            {
                actual = builder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count() == 0);
            }
            else
            {
                Assert.Throws<System.ArgumentException>(() => actual = builder.Build());
            }
        }

        [InlineData("ConfigDb-Connection", "UnknownApp", true)]
        [InlineData("ConfigDb-Connection", "UnknownApp", false)]
        [Theory]
        public void EnvVarAppIdSqlToOpt_NoResults(string conStringVar, string appId, bool optional)
        {
            var builder = new ConfigurationBuilder();
            IConfiguration actual = builder.AddDbSource(conStringVar, appId, 0, optional).Build();
            var listActual = actual.GetConfigSettings();
            Assert.True(listActual.Count() == 0);
        }
        #endregion


        #region Configuration Parameter
        // These tests are for the overload that takes IConfiguration 
        // This is only required in cases where the default database settings of table and column names, or sqlcommandtimeout need to be overridden.
        // These initial settings would be loaded first using another builder before the final configuration builder builds the complete output.
        // The prebuild could simplyTestCases\\DbSource\\AddDbSource\\Config_Env be a configuration builder that is run in Program.cs prior to calling .ConfigureAppConfiguration
        // The prebuild could also come from the default configuration loaded into DTestCases\\DbSource\\AddDbSource\\CI in the Startup class, where the final builder would be called in ConfigureServices.

        [InlineData("1", false)]
        [InlineData("1", true)]
        [InlineData("2", false)]
        [InlineData("2", true)]
        [Theory]
        public void Config_Good(string testCase, bool optional)
        {
            // Create path to appsettings file
            string jsonPath = $"TestCases\\DbSource\\AddDbSource\\Config\\Good\\appsettings{testCase}.json";

            // Get initial config containing non-default database settings
            var initialConfig = TestHelper.GetFileConfig(jsonPath);


            // Create the final builder 
            IConfigurationBuilder finalBuilder = new ConfigurationBuilder();

            // Add the DBSource to the final builder
            finalBuilder.AddDbSource(initialConfig, optional);

            // Build the final config
            var actual = finalBuilder.Build();

            // Convert to lists and compare
            var listActual = actual.GetConfigSettings();
            var listExpected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\DbSource\\AddDbSource\\Config\\Good\\expected{testCase}.json"));
            Assert.True(TestHelper.SettingsAreEqual(listActual, listExpected));

        }


        [InlineData("1", false)]
        [InlineData("1", true)]
        [Theory]
        // test overload with Configuration Parameter
        public void Config_SectionNotFound(string testCase, bool optional)
        {
            IConfiguration actual;
            // Create path to appsettings file
            string jsonPath = $"TestCases\\DbSource\\AddDbSource\\Config\\SectionNotFound\\appsettings{testCase}.json";
            // Get initial config containing non-default database settings
            var initialConfig = TestHelper.GetFileConfig(jsonPath);
            // Create the final builder 
            IConfigurationBuilder finalBuilder = new ConfigurationBuilder();
            if (optional)
            {
                // Add the DBSource to the final builder
                finalBuilder.AddDbSource(initialConfig, optional);
                actual = finalBuilder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count() == 0);
            }
            else
                Assert.Throws<System.Exception>(() => finalBuilder.AddDbSource(initialConfig, optional));
        }


        [InlineData("1", false)]
        [InlineData("1", true)]
        [Theory]
        // test overload with Configuration Parameter
        public void Config_ConnectFail(string testCase, bool optional)
        {
            IConfiguration actual;
            // Create path to appsettings file
            string jsonPath = $"TestCases\\DbSource\\AddDbSource\\Config\\ConnectFail\\appsettings{testCase}.json";
            // Get initial config containing non-default database settings
            var initialConfig = TestHelper.GetFileConfig(jsonPath);
            // Create the final builder 
            IConfigurationBuilder finalBuilder = new ConfigurationBuilder();
            // Add the DBSource to the final builder
            finalBuilder.AddDbSource(initialConfig, optional);

            if (optional)
            {
                actual = finalBuilder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count() == 0);
            }
            else
            {
                Assert.Throws<System.Data.SqlClient.SqlException>(() => actual = finalBuilder.Build());
            }

        }


        [InlineData("testhost", false, "1")]
        [InlineData("testhost", true, "1")]
        [Theory]
        // test overload with Configuration parameter
        public void Config_QueryFail(string appId, bool optional, string testCase)
        {
            IConfiguration actual;
            // Create path to appsettings file
            string jsonPath = $"TestCases\\DbSource\\AddDbSource\\Config\\QueryFail\\appsettings{testCase}.json";
            // Get initial config containing non-default database settings
            var initialConfig = TestHelper.GetFileConfig(jsonPath);

            // Create the final builder 
            IConfigurationBuilder finalBuilder = new ConfigurationBuilder();
            // Add the DBSource to the final builder
            finalBuilder.AddDbSource(initialConfig, optional);

            if (optional)
            {
                actual = finalBuilder.Build();
                var listActual = actual.GetConfigSettings();
                Assert.True(listActual.Count() == 0);
            }
            else
                Assert.Throws<System.Data.SqlClient.SqlException>(() => actual = finalBuilder.Build());
        }

        #endregion

    }
}
