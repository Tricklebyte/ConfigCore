using ConfigCore.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace ConfigCore.Tests
{
    public static class TestHelper
    {
        public static List<ConfigSetting> GetListFromJsonFile(string path)
        {
            List<ConfigSetting> returnList = new List<ConfigSetting>();
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                returnList = JsonConvert.DeserializeObject<List<ConfigSetting>>(json);
            }
            return returnList;
        }

        public static bool SettingsAreEqual(List<ConfigSetting> list1, List<ConfigSetting> list2)
        {
            list1 = list1.OrderBy(x => x.SettingKey).ThenBy(x => x.SettingValue).ToList<ConfigSetting>();
            list2 = list2.OrderBy(x => x.SettingKey).ThenBy(x => x.SettingValue).ToList<ConfigSetting>();
            if (list1.Count != list2.Count)
                return false;
            for (int i = 0; i < list1.Count(); i++)
            {
                if (!list1[i].Equals(list2[i]))
                    return false;
            }
            return true;
        }

        public static bool ConfigsAreEqual(IConfiguration config1, IConfiguration config2)
        {
            var list1 = config1.GetConfigSettings();
            var list2 = config2.GetConfigSettings();
            return TestHelper.SettingsAreEqual(list1, list2);
        }


        public static IConfiguration GetFileConfig(string path)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile(path, false);
            return builder.Build();
        }
        public static string GetJsonFromList(List<ConfigSetting> list)
        {
            return JsonConvert.SerializeObject(list);
        }
    }
}
