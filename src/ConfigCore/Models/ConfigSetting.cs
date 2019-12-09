using System;

namespace ConfigCore.Models
{
    // Basic model example without temporal fields. 

    public class ConfigSetting
    {
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }

        public ConfigSetting() { }

        public ConfigSetting(string key, string value)
        {
            SettingKey = key;
            SettingValue = value;
        }
        public bool Equals(ConfigSetting other)
        {
            return (this.SettingKey == other.SettingKey && this.SettingValue == other.SettingValue);
        }
    }
}
