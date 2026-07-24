using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Interfaces;
using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserRegisterRequestDto request)
        {
            var response = await _userService.RegisterUser(request);
            if (response == null || !response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("users")]
        [Authorize]
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
        [Authorize]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            var response = await _userService.DeleteUser(userId);
            return Ok(response);
        }

        ////For admin Only
        //[HttpGet]
        //[Route("Admins")]
        //[Authorize(Roles = "Admin")]
        //public IActionResult AdminEndPoint()
        //{
        //    var currentUser = GetCurrentUser();
        //    return Ok($"Hi you are an {currentUser.Role}"); 
        //}
        //private UserModel GetCurrentUser()
        //{
        //    var identity = HttpContext.User.Identity as ClaimsIdentity;
        //    if (identity != null)
        //    {
        //        var userClaims = identity.Claims;
        //        return new UserModel
        //        {
        //            Username = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
        //            Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
        //        };
        //    }
        //    return null;
        //}
    }
}
