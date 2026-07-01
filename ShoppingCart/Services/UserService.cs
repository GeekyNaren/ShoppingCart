using ShoppingCart.Interfaces;
using ShoppingCart.Models;
using ShoppingCart.Models.Dtos;
using System.Security.Cryptography;
using System.Text;

namespace ShoppingCart.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #region Public Methods  
        public async Task<User> RegisterUser(RegisterRequest request)
        {
            var duplicateRecord = await GetUsers();
            if (duplicateRecord.Any(x => x.Username == request.Username || x.Email == request.Email))
            {
                throw new Exception("Username or Email already exists.");
            }
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                Role = request.Role,
                CreatedAt = DateTime.UtcNow
            };
            await _userRepository.AddAsync(user);
            return user;
        }
        public async Task<List<UserDto>> GetUsers()
        {
            var userList = await _userRepository.GetAllAsync();
            var dtos = userList.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role,
                CreatedAt = u.CreatedAt
            }).ToList();

            return dtos;
        }
        #endregion

        #region Private Methods
        private static string HashPassword(string password)
        {
            // PBKDF2
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[16];
            rng.GetBytes(salt);

            var hashed = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, 100_000, HashAlgorithmName.SHA256, 32);

            var result = new byte[salt.Length + hashed.Length];
            Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
            Buffer.BlockCopy(hashed, 0, result, salt.Length, hashed.Length);

            return Convert.ToBase64String(result);
        }
        #endregion
    }
}
