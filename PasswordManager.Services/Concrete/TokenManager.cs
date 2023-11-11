using PasswordManager.Services.Abstract;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;

namespace PasswordManager.Services.Concrete
{
    public class TokenManager : ITokenService
    {
        public Tuple<string, string> GenerateToken(string username)
        {
            var secretKey = GenerateSecureKey(32);

            // Header
            var header = new { alg = "HS256", typ = "JWT" };
            string headerJson = new JavaScriptSerializer().Serialize(header);
            string headerBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(headerJson));

            // Payload
            var payload = new { name = username };
            string payloadJson = new JavaScriptSerializer().Serialize(payload);
            string payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));

            // Signature
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(headerBase64 + "." + payloadBase64));
            string signatureBase64 = Convert.ToBase64String(signatureBytes);

            // Token
            string jwtToken = headerBase64 + "." + payloadBase64 + "." + signatureBase64;
            return new Tuple<string, string>(jwtToken, secretKey);
        }

        public static string GenerateSecureKey(int length = 32)
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[length];
                randomNumberGenerator.GetBytes(randomBytes);

                return Convert.ToBase64String(randomBytes);
            }
        }

        public bool ValidateToken(string token, string secretKey)
        {
            var parts = token.Split('.');
            if (parts.Length != 3) // Bir JWT token 3 bölümden oluşmalıdır: header, payload ve signature
            {
                return false;
            }

            string header = parts[0];
            string payload = parts[1];
            string tokenSignature = parts[2];

            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            var computedSignatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(header + "." + payload));
            string computedSignature = Convert.ToBase64String(computedSignatureBytes);

            return computedSignature == tokenSignature; // Eğer hesaplanan imza, token'daki imza ile aynıysa doğrulama başarılıdır
        }
    }
}
