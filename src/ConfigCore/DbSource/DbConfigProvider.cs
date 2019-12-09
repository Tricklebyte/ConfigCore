using ConfigCore.DbSource;
using ConfigCore.Models;

using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigCore.DbSource
{
    public class DbConfigProvider : ConfigurationProvider
    {

        ISqlClientAdo _sqlCient;
        bool _optional;

        public DbConfigProvider( ISqlClientAdo sqlCient, bool optional)
        {
            _optional = optional;
            _sqlCient = sqlCient;
        }

        public override void Load()
        {
            
            Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            // Query the list of settings from the database for this application only, and add each setting to the configuration data dictionary.
            try
            {
                List<ConfigSetting> settings = _sqlCient.GetDbRows();
                //check for sqlClient Errors
                //if no errors,
                for (int i = 0; i < settings.Count(); i++)
                {
                    Data.Add(new KeyValuePair<string, string>(settings[i].SettingKey, settings[i].SettingValue));
                }
            }
            catch (Exception e)
            {
                if (!_optional)
                {
                    throw (e);
                }
            }
        }
    }
}
