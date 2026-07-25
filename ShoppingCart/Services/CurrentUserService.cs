using ShoppingCart.Interfaces;
using ShoppingCart.Models;
using System.Security.Claims;

namespace ShoppingCart.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal? Principal => _httpContextAccessor.HttpContext?.User;

        public string? UserId
        {
            get
            {
                var id = Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                return string.IsNullOrWhiteSpace(id) ? null : id;
            }
        }

        public string? Username
        {
            get
            {
                var name = Principal?.FindFirstValue(ClaimTypes.Name) ?? Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                return string.IsNullOrWhiteSpace(name) ? null : name;
            }
        }

        public string? Role
        {
            get
            {
                var role = Principal?.FindFirstValue(ClaimTypes.Role);
                return string.IsNullOrWhiteSpace(role) ? null : role;
            }
        }

        public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated == true;

        public UserModel? GetCurrentUserModel()
        {
            if (Principal == null)
                return null;

            return new UserModel
            {
                Username = Username,
                Role = Role,
                // Do not populate password here
                Password = null
            };
        }
    }
}
