using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ConfigApi_Auth_Windows.Models
{
    public class ConfigEntity
    {
        public int Id { get; set; }
        public string AppId { get; set; }
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }
    }
}
