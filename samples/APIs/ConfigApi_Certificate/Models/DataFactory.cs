using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigApi_Certificate.Models
{
    public class DataFactory
    {
        public static List<ConfigEntity> GetSettingList()
        {
            return new List<ConfigEntity>()
        {
            new ConfigEntity(){Id=1,AppId="ConfigApiClient_Certificate",SettingKey="Setting1",SettingValue="Setting 1 - Api Configuration Client using Certificate Auth"},
            new ConfigEntity(){Id=2,AppId="ConfigApiClient_Certificate",SettingKey="Setting2",SettingValue="Setting 2 - Api Configuration Client using Certificate Auth"},
            new ConfigEntity(){Id=3,AppId="ConfigApiClient_Certificate",SettingKey="Setting3",SettingValue="Setting 3 - Api Configuration Client using Certificate Auth"},
            new ConfigEntity(){Id=4,AppId="ConfigApiClient_Certificate",SettingKey="Setting4",SettingValue="Setting 4 - Api Configuration Client using Certificate Auth"},
            new ConfigEntity(){Id=5,AppId="ConfigApiClient_Certificate",SettingKey="Setting5",SettingValue="Setting 5 - Api Configuration Client using Certificate Auth"},

            new ConfigEntity(){Id=6,AppId="testhost",SettingKey="Setting1",SettingValue="Setting 1 - Value for testhost"},
            new ConfigEntity(){Id=7,AppId="testhost",SettingKey="Setting2",SettingValue="Setting 2 - Value for testhost"},
            new ConfigEntity(){Id=8,AppId="testhost",SettingKey="Setting3",SettingValue="Setting 3 - Value for testhost"},
            new ConfigEntity(){Id=9,AppId="testhost",SettingKey="Setting4",SettingValue="Setting 4 - Value for testhost"},
            new ConfigEntity(){Id=10,AppId="testhost",SettingKey="Setting5",SettingValue="Setting 5 - Value for testhost"}
        };

        }
    }
}
