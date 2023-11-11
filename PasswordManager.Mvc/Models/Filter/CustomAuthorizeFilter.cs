using PasswordManager.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PasswordManager.Mvc.Models.Filter
{
    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        private readonly IRedisCacheService _redisCacheService;

        public CustomAuthorizeFilter(IRedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var exists = _redisCacheService.Any();

            if (!filterContext.RouteData.Values.ContainsValue("Login"))
            {
                if (!exists)
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }


        }
    }
}