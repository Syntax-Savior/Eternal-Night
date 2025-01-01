using Final_Descent.Services;
using Microsoft.AspNetCore.Mvc;

namespace Final_Descent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ProductService _productService;

        public TestController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("test-db-connection")]
        public IActionResult TestDbConnection()
        {
            _productService.TestDatabaseConnection();
            return Ok("Database connection tested. Check the console logs.");
        }

        [HttpGet("get-products")]
        public IActionResult GetProducts()
        {
            var products = _productService.GetProducts();
            return Ok(products);
        }

        [HttpPost("add-product")]
        public IActionResult AddProduct(string title, string imageUrl, decimal price, decimal originalPrice,
            string currency)
        {
            var result = _productService.AddProduct(title, imageUrl, price, originalPrice, currency);
            return result ? Ok("Product added successfully.") : StatusCode(500, "Failed to add product.");
        }

        [HttpPut("update-product/{id}")]
        public IActionResult UpdateProduct(int id, string title, string imageUrl, decimal price, decimal originalPrice,
            string currency)
        {
            var result = _productService.UpdateProduct(id, title, imageUrl, price, originalPrice, currency);
            return result ? Ok("Product updated successfully.") : StatusCode(500, "Failed to update product.");
        }

        [HttpDelete("delete-product/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var result = _productService.DeleteProduct(id);
            return result ? Ok("Product deleted successfully.") : StatusCode(500, "Failed to delete product.");
        }
    }
}
