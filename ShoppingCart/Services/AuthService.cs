using Microsoft.IdentityModel.Tokens;
using ShoppingCart.ExtensionService;
using ShoppingCart.Interfaces;
using ShoppingCart.Models;
using ShoppingCart.Services.Helper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoppingCart.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public AuthService(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }

        public async Task<ServiceResponse<string?>> LoginAsync(UserLogin userLogin)
        {
            if (string.IsNullOrEmpty(userLogin.Username) || string.IsNullOrEmpty(userLogin.Password))
            {
                return ServiceResponse<string?>.Fail("Username and password are required.");
            }
            var user = await AuthenticateAsync(userLogin);
            if (user != null)
            {
                var token = GenerateToken(user);
                return ServiceResponse<string?>.Ok(token);
            }
            return ServiceResponse<string?>.Fail("Invalid username or password.");
        }

        public string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserModel?> AuthenticateAsync(UserLogin userLogin)
        {
            var dbUser = await _userRepository.GetByUsernameAsync(userLogin.Username);
            if (dbUser == null)
            {
                return null;
            }

            if (!PasswordHelper.VerifyPassword(dbUser.PasswordHash, userLogin.Password))
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
    }
}
