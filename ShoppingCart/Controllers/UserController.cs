using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Interfaces;
using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await _userService.RegisterUser(request);
            if (response == null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("users")]
        public async Task<ActionResult> GetUsers()
        {
            var response = await _userService.GetUsers();
            return Ok(response);
        }
    }
}
