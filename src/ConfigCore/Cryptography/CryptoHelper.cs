using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ConfigCore.Cryptography
{
    public class CryptoHelper : ICryptoHelper
    {
        private readonly IDataProtectionProvider _provider;

        // Create byte array for additional entropy when using Protect method.
        // static byte[] s_aditionalEntropy = { 9, 8, 7, 6, 5 };

        public CryptoHelper(IDataProtectionProvider provider)
        {
            _provider = provider;
        }
        public CryptoHelper()
        {

        }


        //Encrypt string value
        public virtual string Protect(string purpose, string plainText, string encValPrefix )
        {
            string response = "";
            var protector = _provider.CreateProtector(purpose);

            try
            {
                string protectedPayload = protector.Protect(plainText);
                if (!String.IsNullOrEmpty(protectedPayload))
                {
                    if (encValPrefix.Length > 0)
                    {
                        protectedPayload = AddEncValPrefix(encValPrefix, protectedPayload);
                    }
                    response = protectedPayload;
                }
            }
            catch (CryptographicException e)
            {
                throw e;
            }

            return response;

        }

      
        //Decrypt string value
        public string Unprotect(string purpose, string encryptedValue, string encValPrefix)
        {
            string response = "";

            var protector = _provider.CreateProtector(purpose);
      
            if (encValPrefix.Length > 0)
            {
                encryptedValue = RemoveEncValPrefix(encValPrefix, encryptedValue);
            }

            try
            {
                string unprotectedPayload = protector.Unprotect(encryptedValue);
                if (!String.IsNullOrEmpty(unprotectedPayload))
                    response = unprotectedPayload;
            }
            catch (CryptographicException e)
            {
                throw e;
            }
            return response;
        }

    

        public string AddEncValPrefix(string encValPrefix, string encryptedValue)
        {
            return encValPrefix + encryptedValue;
        }

        public string RemoveEncValPrefix(string encValPrefix, string encryptedValue)
        {
            return encryptedValue.Replace(encValPrefix, "");
        }





    }
}
