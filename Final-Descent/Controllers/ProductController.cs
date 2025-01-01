using Final_Descent.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Final_Descent.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            var products = _productService.GetProducts();
            return View("~/Views/Home/Index.cshtml", products);
        }

        public IActionResult ProductDescriptionPage(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
