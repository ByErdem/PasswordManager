using Newtonsoft.Json;
using PasswordManager.Entity.Dtos;
using PasswordManager.Services.Abstract;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PasswordManager.Mvc.Models.Attributes
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        private readonly ITokenService _tokenService;
        private readonly IRedisCacheService _redisCacheService;

        public CustomAuthorize()
        {
            _tokenService = DependencyResolver.Current.GetService<ITokenService>();
            _redisCacheService = DependencyResolver.Current.GetService<IRedisCacheService>();
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
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}