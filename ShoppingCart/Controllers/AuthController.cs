using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShoppingCart.Interfaces;
using ShoppingCart.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public AuthController(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UserLogin userLogin)
        {
            var user = await Authenticate(userLogin);
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }

            return NotFound("user not found");
        }

        // To generate token
        private string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Username),
                new Claim(ClaimTypes.Role,user.Role)
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        //To authenticate user against database
        private async Task<UserModel?> Authenticate(UserLogin userLogin)
        {
            var dbUser = await _userRepository.GetByUsernameAsync(userLogin.Username);
            if (dbUser == null)
            {
                return null;
            }

            if (!VerifyPassword(dbUser.PasswordHash, userLogin.Password))
            {
                return null;
            }

            return new UserModel
            {
                Username = dbUser.Username,
                Role = dbUser.Role,
                Password = string.Empty
            };
        }

        private static bool VerifyPassword(string storedBase64, string password)
        {
            try
            {
                var fullBytes = Convert.FromBase64String(storedBase64);
                // salt is first 16 bytes, hash is remaining
                var salt = new byte[16];
                Buffer.BlockCopy(fullBytes, 0, salt, 0, salt.Length);
                var storedHash = new byte[fullBytes.Length - salt.Length];
                Buffer.BlockCopy(fullBytes, salt.Length, storedHash, 0, storedHash.Length);

                var computedHash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, 100_000, HashAlgorithmName.SHA256, storedHash.Length);

                return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
            }
            catch
            {
                return false;
            }
        }
    }
}
