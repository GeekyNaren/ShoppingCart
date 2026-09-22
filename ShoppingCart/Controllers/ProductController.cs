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

    }
}
