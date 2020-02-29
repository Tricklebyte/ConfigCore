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
    public class ApiClientProvider : ConfigurationProvider

    {
        private readonly HttpClient _client;
        private readonly HttpRequestMessage _request;
        private readonly bool _optional;
        private readonly string _apiClientSourceError;
        public ApiClientProvider(HttpClient client, HttpRequestMessage request, bool optional,string apiClientSourceError)
        {
            _client = client;
            _request = request;
            _optional = optional;
            _apiClientSourceError = apiClientSourceError;
        }

        public override void Load()
        {
            List<ConfigSetting> settingList = new List<ConfigSetting>();
            HttpResponseMessage response = null;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (string.IsNullOrEmpty(_apiClientSourceError))
            {
                try
                {
                    response = _client.SendAsync(_request).Result;
                    response.EnsureSuccessStatusCode();

                    if (response.Content.Headers.ContentLength > 0)
                    {
                        var json = response.Content.ReadAsStringAsync().Result;
                        settingList = JsonSerializer.Deserialize<List<ConfigSetting>>(json, options);
                    }
                }
                catch (Exception e)
                {
                    if (!_optional)
                        throw (e);
                    else
                    {
                        Data.Add("ConfigurationMetaData:ApiSource:RequestURI", _client.BaseAddress.AbsoluteUri);
                        Data.Add("ConfigurationMetaData:ApiSource:ExceptionMessage", e.Message.ToString());
                    }
                }
            }
            else
            {
                Data.Add("ConfigurationMetaData:ApiSource:ExceptionMessage", _apiClientSourceError);
            }

            if (settingList != null && settingList.Count > 0)
            {

                Data = settingList.ToDictionary(c => c.SettingKey, c => c.SettingValue);
            }

        }
    }

}


