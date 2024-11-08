using DotNetAssignment.Server.Models;
using DotNetAssignment.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAssignment.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("SaveProduct")]
        public async Task<IActionResult> SaveProduct([FromBody] List<ProductDetail> products)
        {
            if (products == null || !products.Any())
            {
                return BadRequest("Invalid product data.");
            }

            try
            {
                // Assuming _productService.SaveProduct takes a list of products.
                _productService.SaveProducts(products);
                return StatusCode(200, new { message = "Product saved"});
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                // Call the service method to get all products
                var products = _productService.GetAllProducts();

                if (products == null || !products.Any())
                {
                    return NotFound("No products found.");
                }

                return Ok(products); // Return the products as JSON
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
