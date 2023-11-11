using Newtonsoft.Json;
using PasswordManager.Entity.Dtos;
using PasswordManager.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PasswordManager.Mvc.Models.Filter
{
    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ITokenService _tokenService;
        private readonly ISessionService _sessionService;

        public CustomAuthorizeFilter(IRedisCacheService redisCacheService, ITokenService tokenService, ISessionService sessionService)
        {
            _redisCacheService = redisCacheService;
            _tokenService = tokenService;
            _sessionService = sessionService;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {

        }
    }
}