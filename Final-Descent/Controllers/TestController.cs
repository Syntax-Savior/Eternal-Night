using Final_Descent.Models;
using Final_Descent.Services;
using Microsoft.AspNetCore.Authorization;
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

        /*[Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("add-product")]
        public IActionResult AddProduct(string title, string imageUrl, decimal price, decimal originalPrice,
            string currency)
        {
            var result = _productService.AddProduct(title, imageUrl, price, originalPrice, currency);
            return result ? Ok("Product added successfully.") : StatusCode(500, "Failed to add product.");
        }*/

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("add-product")]
        public IActionResult AddProduct([FromBody] ProductModel product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest(new { Message = "Invalid product data." });
                }

                var result = _productService.AddProduct(product.Title, product.ImageUrl, product.Price,
                    product.OriginalPrice, product.Currency);

                return result
                    ? Ok(new { Message = "Product added successfully." })
                    : StatusCode(500, new { Message = "Failed to add product." });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { Message = "Token is missing or invalid. Access denied." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("update-product/{id}")]
        public IActionResult UpdateProduct(int id, string title, string imageUrl, decimal price, decimal originalPrice,
            string currency)
        {
            var result = _productService.UpdateProduct(id, title, imageUrl, price, originalPrice, currency);
            return result ? Ok("Product updated successfully.") : StatusCode(500, "Failed to update product.");
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("delete-product/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var result = _productService.DeleteProduct(id);
            return result ? Ok("Product deleted successfully.") : StatusCode(500, "Failed to delete product.");
        }
    }
}
