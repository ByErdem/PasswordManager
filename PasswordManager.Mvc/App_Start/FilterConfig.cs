using PasswordManager.Mvc.Models.Filter;
using PasswordManager.Services.Abstract;
using System.Web;
using System.Web.Mvc;

namespace PasswordManager.Mvc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters, IRedisCacheService redisCacheService, ITokenService tokenService, ISessionService sessionService)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomAuthorizeFilter(redisCacheService, tokenService, sessionService));
        }
    }
}
