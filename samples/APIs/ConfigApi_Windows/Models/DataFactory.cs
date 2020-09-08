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
            new ConfigEntity(){Id=1,AppId="testhost",SettingKey="Setting1",SettingValue="Setting 1 - Value for testhost"},
            new ConfigEntity(){Id=2,AppId= "testhost", SettingKey="Setting2",SettingValue="Setting 2 - Value for testhost"},
            new ConfigEntity(){Id=3,AppId="testhost",SettingKey="Setting3",SettingValue="Setting 3 - Value for testhost"},
            new ConfigEntity(){Id=4,AppId="testhost",SettingKey="Setting4",SettingValue="Setting 4 - Value for testhost"},
            new ConfigEntity(){Id=5,AppId="testhost",SettingKey="Setting5",SettingValue="Setting 5 - Value for testhost"},

            new ConfigEntity(){Id=6,AppId="ConfigApiClient_Windows",SettingKey="Setting1",SettingValue="Setting 1 - Value for ConfigApiClient_Windows"},
            new ConfigEntity(){Id=7,AppId="ConfigApiClient_Windows",SettingKey="Setting2",SettingValue="Setting 2 - Value for ConfigApiClient_Windows"},
            new ConfigEntity(){Id=8,AppId="ConfigApiClient_Windows",SettingKey="Setting3",SettingValue="Setting 3 - Value for ConfigApiClient_Windows"},
            new ConfigEntity(){Id=9,AppId="ConfigApiClient_Windows",SettingKey="Setting4",SettingValue="Setting 4 - Value for ConfigApiClient_Windows"},
            new ConfigEntity(){Id=10,AppId="ConfigApiClient_Windows",SettingKey="Setting5",SettingValue="Setting 5 - Value for ConfigApiClient_Windows"}
        };

        }
    }
}
