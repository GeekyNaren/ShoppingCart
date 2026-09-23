using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Interfaces;
using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("addProduct")]
        public async Task<ActionResult> AddProduct([FromBody] AddProductDto request)
        {
            var response = await _productService.AddProduct(request);
            if (response == null || !response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("getProducts")]
        public async Task<ActionResult> GetProducts()
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }

        [HttpGet("getProductById")]
        public async Task<ActionResult> GetProductById(string productId)
        {
            var product = await _productService.GetProductById(productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPut("updateProduct")]
        public async Task<ActionResult> UpdateProduct([FromBody] UpdateProductDto request)
        {
            var response = await _productService.UpdateProduct(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("deleteProduct")]
        public async Task<ActionResult> DeleteProduct(string productId)
        {
            var response = await _productService.DeleteProduct(productId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
