using PasswordManager.Entity.Dtos;
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
    public class MyPasswordsController : Controller
    {
        private readonly IMyPasswordsService _myPasswordsService;

        public MyPasswordsController(IMyPasswordsService myPasswordsService)
        {
            _myPasswordsService = myPasswordsService;
        }

        [CustomAuthorize]
        public async Task<ActionResult> Index()
        {
            var list = await _myPasswordsService.GetAll();
            return View(list);
        }

        public async Task<ActionResult> Create(MyPasswordDto myPasswordDto)
        {
            var result = await _myPasswordsService.Create(myPasswordDto);
            return Json(result);
        }

        public async Task<ActionResult> Update(MyPasswordDto myPasswordDto)
        {
            var result = await _myPasswordsService.Update(myPasswordDto);
            return Json(result);
        }

        public async Task<ActionResult> Delete(MyPasswordDto myPasswordDto)
        {
            var result = await _myPasswordsService.Delete(myPasswordDto);
            return Json(result);
        }
    }
}