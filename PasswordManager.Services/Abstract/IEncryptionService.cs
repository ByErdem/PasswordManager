using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services.Abstract
{
    public interface IEncryptionService
    {
        string GenerateRandomIV(int size);
        string AESEncrypt(string plainText);
        string AESDecrypt(string plainText);
        string Base64Encode(string plainText);
        string Base64Decode(string plainText);
        string MD5Encrypt(string plainText);
    }
}
