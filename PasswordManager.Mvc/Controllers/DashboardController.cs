using PasswordManager.Mvc.Models.Attributes;
using PasswordManager.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PasswordManager.Mvc.Controllers
{
    public class DashboardController : Controller
    {

        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public ActionResult Index()
        {
            return View();
        }


        [CustomAuthorize]

        public async Task<ActionResult> GetCounts()
        {
            var result = await _dashboardService.GetCounts();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}