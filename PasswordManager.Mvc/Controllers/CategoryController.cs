using PasswordManager.Entity.Concrete;
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
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [CustomAuthorize]
        public async Task<ActionResult> Index()
        {
            var list = await _categoryService.GetAll();
            return View(list);
        }

        [CustomAuthorize]
        public async Task<ActionResult> Create(CategoryDto categoryDto)
        {
            var result = await _categoryService.Create(categoryDto);
            return Json(result);
        }

        [CustomAuthorize]

        public async Task<ActionResult> Update(CategoryDto categoryDto)
        {
            var result = await _categoryService.Update(categoryDto);
            return Json(result);
        }

        [CustomAuthorize]

        public async Task<ActionResult> Delete(MCategory categoryDto)
        {
            var result = await _categoryService.Delete(categoryDto);
            return Json(result);
        }

        [CustomAuthorize]

        public async Task<ActionResult> HardDelete(MCategory categoryDto)
        {
            var result = await _categoryService.HardDelete(categoryDto);
            return Json(result);
        }

        [CustomAuthorize]

        public async Task<ActionResult> GetAll()
        {
            var result = await _categoryService.GetAll();
            return Json(result);
        }
    }
}