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
            new ConfigEntity(){Id=1,AppId="ApiClient_EnvVarParam",SettingKey="Setting1",SettingValue="Setting 1 - Value for API configuration client"},
            new ConfigEntity(){Id=2,AppId="ApiClient_EnvVarParam",SettingKey="Setting2",SettingValue="Setting 2 - Value for API configuration client"},
            new ConfigEntity(){Id=3,AppId="ApiClient_EnvVarParam",SettingKey="Setting3",SettingValue="Setting 3 - Value for API configuration client"},
            new ConfigEntity(){Id=4,AppId="ApiClient_EnvVarParam",SettingKey="Setting4",SettingValue="Setting 4 - Value for API configuration client"},
            new ConfigEntity(){Id=5,AppId="ApiClient_EnvVarParam",SettingKey="Setting5",SettingValue="Setting 5 - Value for API configuration client"},
        };

        }
    }
}
