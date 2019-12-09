using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ConfigCore.ApiSource;
using ConfigCore.Data;
using ConfigCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConfigApi.Controllers
{
    [ApiController]
    [Route("iapi/[controller]")]
    public class ConfigSettingsController : ControllerBase
    {
        IConfiguration _config;
        public ConfigSettingsController(IConfiguration config) {
            _config = config;
        }

        /// <summary>
        /// Simplified API example for demonstration and testing purposes only
        /// Queries SQL server for client configuration settings filtered by the AppId Parameter
        /// Refer to the SQL folder for scripts to build and populate the sample db
        ///
        /// </summary>
        /// <param name="appId"></param>
        /// <returns>List</returns>
        [HttpGet("{appId}")]
        public ActionResult<List<ConfigSetting>> GetByAppId(string appId)
        {
            List<ConfigSetting> retList = new List<ConfigSetting>();

            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection"))) 
            {
                string sqlQuery = $"SELECT SettingKey, SettingValue from ConfigSetting WHERE AppId = '{appId}'"; 
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
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
