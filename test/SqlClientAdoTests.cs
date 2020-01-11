using ConfigCore.Models;
using ConfigCore.DbSource;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;
using System.Linq;

namespace ConfigCore.Tests
{
    public class SqlClientAdoTests
    {
        [InlineData("Server = (localdb)\\MSSQLLocalDB;Database = ConfigDb;Trusted_Connection = True;MultipleActiveResultSets = true","ConfigSetting", "AppId", "SettingKey","SettingValue", "ConfigDbClient","1")]
        [InlineData("Server = (localdb)\\MSSQLLocalDB;Database = ConfigDb;Trusted_Connection = True;MultipleActiveResultSets = true", "ConfigSetting", "AppId", "SettingKey", "SettingValue", "testhost", "2")]
        [InlineData("Server = (localdb)\\MSSQLLocalDB;Database = ConfigDb;Trusted_Connection = True;MultipleActiveResultSets = true", "ConfigSetting", "AppId", "SettingKey", "SettingValue", "CustomAppName", "3")]
        [Theory]
        public void GetDbRows_Good(string connString, string tableName, string appIdCol, string keyCol, string valCol, string appIdVal, string testCase)
        {
            DbSourceOptions options = new DbSourceOptions()
            {
                ConnString = connString,
                TableName = tableName,
                AppIdCol = appIdCol,
                KeyCol = keyCol,
                ValCol = valCol,
                AppIdVal = appIdVal
            };
            ISqlClientAdo sqlClientDAL = new SqlClientAdo(options);
            var actual = sqlClientDAL.GetDbRows();
            var expected = JsonConvert.DeserializeObject<List<ConfigSetting>>(File.ReadAllText($"TestCases\\DbSource\\SqlClientAdo\\GetDbRows\\Good\\expected{testCase}.json")); 
            Assert.True(TestHelper.SettingsAreEqual(expected, actual));
            
        }

        


    }
}
