using ConfigCore.Cryptography;
using ConfigCore.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace ConfigCore.Extensions
{
    public static class IConfigurationExtensions
    {

        public static List<ConfigSetting> GetConfigSettings(this IConfiguration config)
        {
            var list = new List<ConfigSetting>();
            config.GetChildren().AsParallel().ToList()
                .ForEach(x => x.GetConfigSettings(list));
            return list;
        }

        private static void GetConfigSettings(this IConfigurationSection section,
            List<ConfigSetting> list, string parentKey = "")
        {
            if (section.Value == null)
                section.GetChildren().AsParallel().ToList()
                    .ForEach(x => x.GetConfigSettings(list, $"{parentKey}{section.Key}:"));
            else
            {
                list.Add(new ConfigSetting($"{parentKey}{section.Key}", section.Value));
            }
        }

        public static IConfiguration Decrypt(this IConfiguration config, ICryptoHelper crypto)
        {
            string key;
            string foundVal;
            string decryptedVal;
            List<ConfigSetting> configList = config.GetConfigSettings();
            string encPrefix = config["ConfigOptions:Cryptography:EncValPrefix"];

            for (int i = 0; i < configList.Count; i++)
            {
                foundVal = configList[i].SettingValue;
                if (foundVal.StartsWith(encPrefix) && foundVal!=encPrefix)
                {
                    key = configList[i].SettingKey;
                    decryptedVal = crypto.Unprotect(key, foundVal, encPrefix);
                    config[key] = decryptedVal;
                }
            }
            return config;

        }

    }
}
