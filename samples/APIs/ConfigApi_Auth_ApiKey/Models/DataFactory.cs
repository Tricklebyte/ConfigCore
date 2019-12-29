using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigApi_Auth_ApiKey.Models
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

            new ConfigEntity(){Id=7,AppId="ConfigClient_ApiDefault",SettingKey="Setting7",SettingValue="Setting 7 - In-Memory Value"},
            new ConfigEntity(){Id=8,AppId="ConfigClient_ApiDefault",SettingKey="Setting8",SettingValue="Setting 8 - In-Memory Value"},
            new ConfigEntity(){Id=9,AppId="ConfigClient_ApiDefault",SettingKey="Setting9",SettingValue="Setting 9 - In-Memory Value"},
            new ConfigEntity(){Id=10,AppId="ConfigClient_ApiDefault",SettingKey="Setting10",SettingValue="Setting 10 - In-Memory Value"},
            new ConfigEntity(){Id=11,AppId="ConfigClient_ApiCustom",SettingKey="Setting11",SettingValue="Setting 11 - In-Memory Value"},
            new ConfigEntity(){Id=12,AppId="ConfigClient_ApiCustom",SettingKey="Setting12",SettingValue="Setting 12 - In-Memory Value"},
            new ConfigEntity(){Id=13,AppId="ConfigClient_ApiCustom",SettingKey="Setting13",SettingValue="Setting 13 - In-Memory Value"},
            new ConfigEntity(){Id=14,AppId="ConfigClient_ApiCustom",SettingKey="Setting14",SettingValue="Setting 14 - In-Memory Value"},
            new ConfigEntity(){Id=15,AppId="ConfigClient_ApiCustom",SettingKey="Setting15",SettingValue="Setting 15 - In-Memory Value"},
            new ConfigEntity(){Id=16,AppId="ConfigClient_ApiStartup", SettingKey="Setting16",SettingValue="Setting 16 - In-Memory Value"},
            new ConfigEntity(){Id=17,AppId="ConfigClient_ApiStartup", SettingKey="Setting12",SettingValue="Setting 12 - In-Memory Value"},
            new ConfigEntity(){Id=18,AppId="ConfigClient_ApiStartup", SettingKey="Setting13",SettingValue="Setting 13 - In-Memory Value"},
            new ConfigEntity(){Id=19,AppId="ConfigClient_ApiStartup", SettingKey="Setting14",SettingValue="Setting 14 - In-Memory Value"},
            new ConfigEntity(){Id=20,AppId="ConfigClient_ApiStartup", SettingKey="Setting15",SettingValue="Setting 15 - In-Memory Value"},
        };

        }
    }
}
