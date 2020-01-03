using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigCore.Models
{
    public class DbDefault
    {
        public const string ConnKey = "ConfigOptions:DbSource:ConnStringKey";
        public const string TableName = "ConfigSetting";
        public const string AppIdCol = "AppId";
        public const string KeyCol = "SettingKey";
        public const string ValCol = "SettingValue";
    }
    public class ApiDefault
    {
        public const string ConfigUrlKey = "ConfigOptions:ApiSource:ConfigUrl";
        public const string AuthType = "Windows";
        
    }
}
