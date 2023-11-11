using Newtonsoft.Json;
using PasswordManager.Entity.Dtos;
using PasswordManager.Services.Abstract;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PasswordManager.Mvc.Models.Attributes
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        private readonly ITokenService _tokenService;
        private readonly IRedisCacheService _redisCacheService;
        private readonly ISessionService _sessionService;

        public CustomAuthorize()
        {
            _tokenService = DependencyResolver.Current.GetService<ITokenService>();
            _redisCacheService = DependencyResolver.Current.GetService<IRedisCacheService>();
            _sessionService = DependencyResolver.Current.GetService<ISessionService>();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authHeader = httpContext.Request.Headers["Authorization"];

            if (authHeader != null)
            {
                var guid = authHeader.Replace("Bearer ", "");
                var parameters = Task.Run(async () => await _redisCacheService.GetAsync(guid)).Result;
                var userParameter = JsonConvert.DeserializeObject<UserParameter>(parameters);
                var validated = _tokenService.ValidateToken(userParameter.Token, userParameter.SecretKey);
                if (validated)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //Eğer tıklanan menülerden Bearer token olmadan istek gelirse session'da oturumumuz olup olmadığın kontrol edelim.
            var status = _sessionService.Validate();
            if (!status)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}