using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigCore.Cryptography
{
    public interface ICryptoHelper
    {
        string Protect(string settingName, string plainText,string encValPrefix);

        string Unprotect(string settingName, string encryptedText,string encValPrefix );

        string AddEncValPrefix(string encValPrefix, string encryptedValue);

        string RemoveEncValPrefix(string encValPrefix, string encryptedValue);
      
    }
}
