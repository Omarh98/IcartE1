using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Services
{
    public class CipherService : ICipherService
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private const string Key = "A-very-secure-encryption-key";

        public CipherService(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }

        public string Encrypt(string input)
        {
            var protector = _dataProtectionProvider.CreateProtector(Key);
            return protector.Protect(input);
        }

        public string Decrypt(string cipherText)
        {
            var protector = _dataProtectionProvider.CreateProtector(Key);
            return protector.Unprotect(cipherText);
        }
    }
}
