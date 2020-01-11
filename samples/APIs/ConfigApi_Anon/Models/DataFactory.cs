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
            new ConfigEntity(){Id=1,AppId="testhost",SettingKey="Setting1",SettingValue="Setting 1 - Value for test host"},
            new ConfigEntity(){Id=2,AppId="testhost",SettingKey="Setting2",SettingValue="Setting 2 - Value for test host"},
            new ConfigEntity(){Id=3,AppId="testhost",SettingKey="Setting3",SettingValue="Setting 3 - Value for test host"},
            new ConfigEntity(){Id=4,AppId="testhost",SettingKey="Setting4",SettingValue="Setting 4 - Value for test host"},
            new ConfigEntity(){Id=5,AppId="testhost",SettingKey="Setting5",SettingValue="Setting 5 - Value for test host"},

            new ConfigEntity(){Id=6,AppId="CustomAppName",SettingKey="Setting1",SettingValue="Setting 1 - Value for CustomAppName"},
            new ConfigEntity(){Id=7,AppId="CustomAppName",SettingKey="Setting2",SettingValue="Setting 2 - Value for CustomAppName"},
            new ConfigEntity(){Id=8,AppId="CustomAppName",SettingKey="Setting3",SettingValue="Setting 3 - Value for CustomAppName"},
            new ConfigEntity(){Id=9,AppId="CustomAppName",SettingKey="Setting4",SettingValue="Setting 4 - Value for CustomAppName"},
            new ConfigEntity(){Id=10,AppId="CustomAppName",SettingKey="Setting5",SettingValue="Setting 5 - Value for CustomAppName"},

        };

        }
    }
}
