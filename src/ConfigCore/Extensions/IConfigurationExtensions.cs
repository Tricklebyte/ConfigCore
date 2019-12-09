using ConfigCore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ConfigCore
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
       
       
        
    }
}
