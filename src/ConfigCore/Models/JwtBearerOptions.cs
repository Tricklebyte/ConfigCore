using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigCore.Models
{
   public class JWTBearerOptions
    {

        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }

        public JWTBearerOptions(string authority, string clientId, string clientSecret, string scope) {
            string ErrMsg = "";
            if (!Uri.IsWellFormedUriString(authority, UriKind.Absolute))
            {
                ErrMsg += "Bearer Token Authority URL not well formed.\n";
            }
            else
            {
                Uri uri = new Uri(authority);
                if (!(uri.Scheme.ToLower() == "https") || (uri.Scheme.ToLower() == "http"))
                    ErrMsg += $"URI scheme '{uri.Scheme}' is not supported";
            }

            if (ErrMsg.Length > 1)
                throw (new System.ArgumentException(ErrMsg));
        }
    }
}
