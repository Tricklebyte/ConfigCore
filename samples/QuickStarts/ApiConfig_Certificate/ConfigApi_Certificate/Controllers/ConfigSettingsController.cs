using System.Collections.Generic;
using System.Linq;
using ConfigApi_Certificate.Models;
using ConfigCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ConfigApi_Certificate.Controllers
{


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
        public ActionResult<List<ConfigSetting>> Get(string appId, string idList)
        {
            // Test multiple query parameters
            // Just check that the csv list of Ids contains three elements and return all by name
            string[] ids = idList.Split(",");

            List<ConfigSetting> retList = new List<ConfigSetting>();
            if (ids.Count() == 3)
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
