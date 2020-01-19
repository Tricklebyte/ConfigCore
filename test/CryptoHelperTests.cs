using ConfigCore.Cryptography;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace ConfigCore.Tests 
{
   public class CryptoHelperTests : DataProtectionFixture
    {
        [InlineData("ConfigCore.Cryptography.Tests:ProtectUnprotect","Value to Protect", "<(*_*)>")]
        [InlineData("ConnectionStrings:DefaultConnection", "Server = (localdb)\\mssqllocaldb;Database=ConfigDb;Trusted_Connection=True;MultipleActiveResultSets=True;", "<(*_*)>")]
        [InlineData("Auth:IdentityServer:ClientSecret", "83204560-ebfe-4f56-890d-e2530cc6c30e", "<(*_*)>")]
        
        [Theory]
        public void ProtectUnProtect(string purpose, string unprotectedText, string encValPrefix)
        {
            // ARRANGE
            //create crypto helper using test provider that was created in fixture
            ICryptoHelper cryptoHelper = new CryptoHelper(Provider);

            //ACT 
            // Protect plain text value
            string protectedText = cryptoHelper.Protect(purpose, unprotectedText, encValPrefix);

            // verify encryption by checking for the presence of the encryption prefix and ensuring that the original value is no longer found in the string
            bool hasPrefix = protectedText.StartsWith(encValPrefix);
            bool valProtected = !protectedText.Contains(unprotectedText);
            Assert.True(hasPrefix && valProtected);

            // Unencrypt 
            string actual = cryptoHelper.Unprotect(purpose, protectedText, encValPrefix);
            string expected = unprotectedText;

            //ASSERT
            // verify plain text value matches original:
            Assert.True(expected==actual);
        }

        [InlineData("prefix_","value","prefix_value")]
        [InlineData("<(*_*)>", "CfDJ8MEam1FDHgxEvfUJJtSmsUZyykRA2AEdmJQWWgun0RcScpUFM6DDwbY_GS39AU4vV26B9XbDA5F8gYt7fxnWiWDNiKHh-mLRkoTduTc_LxN66AR2zMhofyTBFKtOBNG4QskS9QEccjqDHd15E0RBumI4kbQLgKRsFjit3y-jFJC4vWRTJAd48jV0WVFmtzK5qWkh1JKVORpVceizeEY0foVIKfCcHnGdNKL9qzLn7SZG7A7-hWPGYSPFruAU9sxHxA", "<(*_*)>CfDJ8MEam1FDHgxEvfUJJtSmsUZyykRA2AEdmJQWWgun0RcScpUFM6DDwbY_GS39AU4vV26B9XbDA5F8gYt7fxnWiWDNiKHh-mLRkoTduTc_LxN66AR2zMhofyTBFKtOBNG4QskS9QEccjqDHd15E0RBumI4kbQLgKRsFjit3y-jFJC4vWRTJAd48jV0WVFmtzK5qWkh1JKVORpVceizeEY0foVIKfCcHnGdNKL9qzLn7SZG7A7-hWPGYSPFruAU9sxHxA")]
        [Theory]
        public void AddEncValPrefix(string prefix,string value, string expected)
        {
            ICryptoHelper cryptoHelper = new CryptoHelper(Provider);
            string actual = cryptoHelper.AddEncValPrefix(prefix,value);
            Assert.True(expected==actual);
        }

        [InlineData("prefix_", "prefix_value", "value")]
        [InlineData("<(*_*)>", "<(*_*)>CfDJ8MEam1FDHgxEvfUJJtSmsUZyykRA2AEdmJQWWgun0RcScpUFM6DDwbY_GS39AU4vV26B9XbDA5F8gYt7fxnWiWDNiKHh-mLRkoTduTc_LxN66AR2zMhofyTBFKtOBNG4QskS9QEccjqDHd15E0RBumI4kbQLgKRsFjit3y-jFJC4vWRTJAd48jV0WVFmtzK5qWkh1JKVORpVceizeEY0foVIKfCcHnGdNKL9qzLn7SZG7A7-hWPGYSPFruAU9sxHxA", "CfDJ8MEam1FDHgxEvfUJJtSmsUZyykRA2AEdmJQWWgun0RcScpUFM6DDwbY_GS39AU4vV26B9XbDA5F8gYt7fxnWiWDNiKHh-mLRkoTduTc_LxN66AR2zMhofyTBFKtOBNG4QskS9QEccjqDHd15E0RBumI4kbQLgKRsFjit3y-jFJC4vWRTJAd48jV0WVFmtzK5qWkh1JKVORpVceizeEY0foVIKfCcHnGdNKL9qzLn7SZG7A7-hWPGYSPFruAU9sxHxA")]
        [Theory]
        public void RemoveEncValPrefix(string prefix, string value, string expected)
        {
            ICryptoHelper cryptoHelper = new CryptoHelper(Provider);
            string actual = cryptoHelper.RemoveEncValPrefix( prefix,value);
            Assert.True(expected== actual);
        }
    }


}
