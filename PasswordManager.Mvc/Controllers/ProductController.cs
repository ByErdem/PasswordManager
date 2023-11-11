using PasswordManager.Entity.Dtos;
using PasswordManager.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PasswordManager.Mvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ActionResult> Index()
        {
            var list = await _productService.GetAll();
            return View(list);
        }

        public async Task<ActionResult> Create(ProductDto productDto)
        {
            HttpContext currentContext = System.Web.HttpContext.Current;
            var result = await _productService.Create(productDto, currentContext);
            return Json(result);
        }

        public async Task<ActionResult> Update(ProductDto productDto)
        {
            var result = await _productService.Update(productDto);
            return Json(result);
        }

        public async Task<ActionResult> Delete(ProductDto productDto)
        {
            var result = await _productService.Delete(productDto);
            return Json(result);
        }

        public async Task<ActionResult> GetAll()
        {
            var result = await _productService.GetAll();
            return Json(result);
        }
    }
}