using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigCore.Models
{
   public class BearerConfig
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }
}
