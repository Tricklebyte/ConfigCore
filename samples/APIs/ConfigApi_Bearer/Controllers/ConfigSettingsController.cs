using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ConfigApi_Bearer.Models;
using ConfigCore.ApiSource;
using ConfigCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConfigApi_Bearer.Controllers
{
  
    [Authorize]
    [ApiController]
    [Route("iapi/[controller]")]
    public class ConfigSettingsController : ControllerBase
    {
        List<ConfigEntity> _listSettings; 

        IConfiguration _config;
        public ConfigSettingsController(IConfiguration config) {
            _config = config;
            _listSettings= DataFactory.GetSettingList();
        }

        [HttpGet]
        public ActionResult<List<ConfigSetting>> Get([FromQuery(Name ="AppId")] string appId)
        {
            // returns configsettings for this appId only
            List<ConfigSetting> retList = new List<ConfigSetting>();
            retList = _listSettings.Where(x => x.AppId == appId).Select(s => new ConfigSetting() { SettingKey = s.SettingKey, SettingValue = s.SettingValue }).ToList();
            return retList;
        }


        /// <summary>
        /// Simplified API example for demonstration and testing purposes only
        /// Uses an in-memory list populated by the data factory.
        /// 
        ///
        /// </summary>
        /// <param name="appId"></param>
        /// <returns>List</returns>
        [HttpGet("{appId}")]
        public ActionResult<List<ConfigSetting>> GetByAppId(string appId)
        {
            // returns configsettings for this appId only
            List<ConfigSetting> retList = new List<ConfigSetting>();
            retList = _listSettings.Where(x=>x.AppId==appId).Select(s => new ConfigSetting() { SettingKey = s.SettingKey, SettingValue = s.SettingValue }).ToList();
            return retList;
        }
    }
}
