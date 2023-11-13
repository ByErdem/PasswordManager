using PasswordManager.Entity.Dtos;
using PasswordManager.Mvc.Models.Attributes;
using PasswordManager.Services.Abstract;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PasswordManager.Mvc.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;


        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegisterPage()
        {
            return View();
        }

        public async Task<ActionResult> SignIn(UserLoginDto user)
        {
            ResponseDto<UserParameter> result = null;
            result = await _userService.SignIn(user);
            return Json(result);
        }

        public async Task<ActionResult> Register(UserRegisterDto user)
        {
            var result = await _userService.Register(user);
            return Json(result);
        }

        [CustomAuthorize]

        public async Task<ActionResult> GetUserInformations()
        {
            var result = await _userService.GetUserInformations();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize]
        public async Task<ActionResult> Logout()
        {
            var result = await _userService.Logout();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}