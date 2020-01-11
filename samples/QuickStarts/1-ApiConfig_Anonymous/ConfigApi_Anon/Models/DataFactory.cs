using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigApi_Anon.Models
{
    public class DataFactory
    {
        public static List<ConfigEntity> GetSettingList()
        {
            return new List<ConfigEntity>()
        {
            new ConfigEntity(){Id=1,AppId="ConfigApiClient_Anon",SettingKey="Setting1",SettingValue="Setting 1 - Example Client using Anonymous Auth"},
            new ConfigEntity(){Id=2,AppId="ConfigApiClient_Anon",SettingKey="Setting2",SettingValue="Setting 2 - Example Client using Anonymous Auth"},
            new ConfigEntity(){Id=3,AppId="ConfigApiClient_Anon",SettingKey="Setting3",SettingValue="Setting 3 - Example Client using Anonymous Auth"},
            new ConfigEntity(){Id=4,AppId="ConfigApiClient_Anon",SettingKey="Setting4",SettingValue="Setting 4 - Example Client using Anonymous Auth"},
            new ConfigEntity(){Id=5,AppId="ConfigApiClient_Anon",SettingKey="Setting5",SettingValue="Setting 5 - Example Client using Anonymous Auth"},
        };

        }
    }
}
