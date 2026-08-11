using ShoppingCart.ExtensionService;
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
        private readonly ICurrentUserService _currentUserService;

        public UserService(IUserRepository userRepository, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
        }

        #region Public Methods  
        public async Task<ServiceResponse<bool>> RegisterUser(UserRegisterRequestDto request)
        {
            // Check for duplicate username or email without requiring Admin role
            var userList = await _userRepository.GetAllAsync();
            if (userList.Any(x => x.Username == request.Username || x.Email == request.Email))
            {
                return ServiceResponse<bool>.Fail("Username already exists with same credentials.");
            }

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                Role = request.Role ?? "Customer",
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            return ServiceResponse<bool>.Ok(true);
        }
        public async Task<ServiceResponse<List<UserResponseDto>>> GetUsers()
        {
            if (_currentUserService.Role != "Admin")
            {
                return ServiceResponse<List<UserResponseDto>>.Fail("Unauthorized access.");
            }
            var userList = await _userRepository.GetAllAsync();
            var dtos = userList.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role,
                CreatedAt = u.CreatedAt
            }).ToList();
            return ServiceResponse<List<UserResponseDto>>.Ok(dtos);
        }
        public async Task<ServiceResponse<UserResponseDto>> GetUserById(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ServiceResponse<UserResponseDto>.Fail("User not found.");
            }
            var userDto = new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
            return ServiceResponse<UserResponseDto>.Ok(userDto);
        }
        public async Task<ServiceResponse<bool>> DeleteUser(string userId)
        {
            if(_currentUserService.Role != "Admin")
            {
                return ServiceResponse<bool>.Fail("Unauthorized access.");
            }
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ServiceResponse<bool>.Fail("User not found.");
            }
            await _userRepository.DeleteAsync(userId);
            return ServiceResponse<bool>.Ok(true);
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
