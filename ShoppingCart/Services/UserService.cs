using System.Security.Cryptography;
using System.Text;
using ShoppingCart.Interfaces;
using ShoppingCart.Models;

namespace ShoppingCart.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> RegisterAsync(string username, string email, string password, string role)
        {
            // basic validation
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("username is required");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("email is required");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("password is required");

            var existingByEmail = await _userRepository.GetByEmailAsync(email);
            if (existingByEmail != null) throw new InvalidOperationException("Email already in use");

            var existingByUsername = await _userRepository.GetByUsernameAsync(username);
            if (existingByUsername != null) throw new InvalidOperationException("Username already in use");

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = username,
                Email = email,
                PasswordHash = HashPassword(password),
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            return user;
        }

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
    }
}
