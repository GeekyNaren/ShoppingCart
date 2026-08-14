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
        private readonly ILogger<UserService> _logger;
        public UserService(IUserRepository userRepository, ICurrentUserService currentUserService, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        #region Public Methods  
        public async Task<ServiceResponse<bool>> RegisterUser(UserRegisterRequestDto request)
        {
            // Check for duplicate username or email without requiring Admin role
            var userList = await _userRepository.GetAllAsync();
            if (userList.Any(x => x.Username == request.Username || x.Email == request.Email))
            {
                _logger.LogWarning("Attempt to register duplicate user with username {Username} and email {Email}", request.Username, request.Email);
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
                _logger.LogWarning("Unauthorized access attempt by user {UserId} with role {Role}", _currentUserService.UserId, _currentUserService.Role);
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

        public async Task<ServiceResponse<UserResponseDto>> UpdateUser(UpdateUserRequestDto request)
        {
            // Check if user is authenticated
            if (!_currentUserService.IsAuthenticated)
            {
                return ServiceResponse<UserResponseDto>.Fail("User is not authenticated.");
            }

            var currentUserId = _currentUserService.UserId;
            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
            {
                return ServiceResponse<UserResponseDto>.Fail("User not found.");
            }

            // Allow users to update their own profile, or allow admins to update any user
            if (currentUserId != request.Id && _currentUserService.Role != "Admin")
            {
                return ServiceResponse<UserResponseDto>.Fail("You can only update your own profile.");
            }

            // Check if new username or email already exists (exclude current user)
            var existingUsers = await _userRepository.GetAllAsync();
            if (existingUsers.Any(x => (x.Username == request.Username || x.Email == request.Email) && x.Id != request.Id))
            {
                return ServiceResponse<UserResponseDto>.Fail("Username or Email already exists.");
            }

            // Update user details
            user.Username = request.Username;
            user.Email = request.Email;
            if (!string.IsNullOrEmpty(request.Password))
            {
                user.PasswordHash = HashPassword(request.Password);
            }

            await _userRepository.UpdateAsync(request.Id, user);

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
