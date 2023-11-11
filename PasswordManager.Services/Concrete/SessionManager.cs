using Newtonsoft.Json;
using PasswordManager.Entity.Dtos;
using PasswordManager.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PasswordManager.Services.Concrete
{
    public class SessionManager : ISessionService
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ITokenService _tokenService;

        public SessionManager(IRedisCacheService redisCacheService, ITokenService tokenService)
        {
            _redisCacheService = redisCacheService;
            _tokenService = tokenService;

        }


        public void SetSessionValue(string key, string value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public string GetSessionValue(string key)
        {
            return HttpContext.Current.Session[key].ToString();
        }

        public bool Validate()
        {
            var guidKey = GetSessionValue("GuidKey");
            var parameters = Task.Run(async () => await _redisCacheService.GetAsync(guidKey)).Result;
            var userParameter = JsonConvert.DeserializeObject<UserParameter>(parameters);
            return _tokenService.ValidateToken(userParameter.Token, userParameter.SecretKey);
        }
    }
}
