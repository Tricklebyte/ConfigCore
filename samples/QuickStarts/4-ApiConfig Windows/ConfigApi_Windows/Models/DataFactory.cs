using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigApi_Windows.Models
{
    public class DataFactory
    {
        public static List<ConfigEntity> GetSettingList()
        {
            return new List<ConfigEntity>()
        {
            new ConfigEntity(){Id=1,AppId="ConfigApiClient_Windows",SettingKey="Setting1",SettingValue="Setting 1 - Value for ConfigApiClient_Windows"},
            new ConfigEntity(){Id=2,AppId="ConfigApiClient_Windows",SettingKey="Setting2",SettingValue="Setting 2 - Value for ConfigApiClient_Windows"},
            new ConfigEntity(){Id=3,AppId="ConfigApiClient_Windows",SettingKey="Setting3",SettingValue="Setting 3 - Value for ConfigApiClient_Windows"},
            new ConfigEntity(){Id=4,AppId="ConfigApiClient_Windows",SettingKey="Setting4",SettingValue="Setting 4 - Value for ConfigApiClient_Windows"},
            new ConfigEntity(){Id=5,AppId="ConfigApiClient_Windows",SettingKey="Setting5",SettingValue="Setting 5 - Value for ConfigApiClient_Windows"}
        };

        }
    }
}
