using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using ConfigCore.Models;
using System.Runtime.Serialization.Json;
using System.Text.Json;

namespace ConfigCore.ApiSource
{
    public class ApiConfigProvider : ConfigurationProvider

    {
        private readonly HttpClient _client;
        private readonly HttpRequestMessage _request;
        private readonly bool _optional;
        public ApiConfigProvider(HttpClient client, HttpRequestMessage request, bool optional)
        {
            _client = client;
            _request = request;
            _optional = optional;
        }

        public override void Load()
        {
            List<ConfigSetting> settingList = new List<ConfigSetting>();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            try
            {
                var response = _client.SendAsync(_request).Result;
                response.EnsureSuccessStatusCode();

                if (response.Content.Headers.ContentLength > 0)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    settingList = JsonSerializer.Deserialize<List<ConfigSetting>>(json,options);
                }
            }
            catch (Exception e)
            {
                if (!_optional)
                    throw (e);
            }

            if (settingList != null && settingList.Count > 0)
            {

                Data = settingList.ToDictionary(c => c.SettingKey, c => c.SettingValue);
            }

        }
    }

}


