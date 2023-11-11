using System;

namespace PasswordManager.Services.Abstract
{
    public interface ITokenService
    {
        Tuple<string, string> GenerateToken(string username);
        bool ValidateToken(string token, string secretKey);
    }
}
