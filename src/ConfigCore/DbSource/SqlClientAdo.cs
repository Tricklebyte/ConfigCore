
using ConfigCore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Text;

namespace ConfigCore.DbSource
{
    public interface ISqlClientAdo
    {
        List<ConfigSetting> GetDbRows();


    }

    public class SqlClientAdo : ISqlClientAdo
    {
        DbSourceOptions _options;
        public SqlClientAdo (DbSourceOptions options) {
            _options = options;
        }


        public List<ConfigSetting> GetDbRows()
        {
            List<ConfigSetting> retList = new List<ConfigSetting>();
           
                using (SqlConnection con = new SqlConnection(_options.ConnString))
                {
                    string sqlQuery = $"SELECT {_options.KeyCol} AS SettingKey, {_options.ValCol} AS SettingValue from {_options.TableName} ";
                    sqlQuery += $"WHERE {_options.AppIdCol} = '{_options.AppIdVal}'";

                    SqlCommand cmd = new SqlCommand(sqlQuery, con);
                    cmd.CommandTimeout = _options.SqlCmdTimeout;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                  
                    while (rdr.Read())
                    {
                        var setting = new ConfigSetting()
                        {
                            SettingKey = (string)rdr["SettingKey"],
                            SettingValue = (string)rdr["SettingValue"]
                        };
                        retList.Add(setting);
                    }
                    con.Close();
                }
           
            

            return retList;
        }

      
    }
}
