using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Interfaces;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Services;

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
            if (response == null || !response.Success)
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

        [HttpGet("userById")]
        public async Task<ActionResult> GetUserById(string userId)
        {
            var response = await _userService.GetUserById(userId);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("deleteUser")]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            var response = await _userService.DeleteUser(userId);
            return Ok(response);
        }
    }
}
