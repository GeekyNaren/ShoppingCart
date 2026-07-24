using ShoppingCart.Models;
using System.Security.Claims;

namespace ShoppingCart.Interfaces
{
    public interface ICurrentUserService
    {
        /// <summary>
        /// Returns the raw ClaimsPrincipal for the current request (may be null).
        /// </summary>
        ClaimsPrincipal? Principal { get; }

        /// <summary>
        /// Gets the user's id from claims (ClaimTypes.NameIdentifier) if present.
        /// </summary>
        string? UserId { get; }

        /// <summary>
        /// Gets the username from claims (ClaimTypes.Name or NameIdentifier) if present.
        /// </summary>
        string? Username { get; }

        /// <summary>
        /// Gets the role claim value (ClaimTypes.Role) if present.
        /// </summary>
        string? Role { get; }

        /// <summary>
        /// True when the current user is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Returns a UserModel composed from available claims (may have null properties).
        /// </summary>
        UserModel? GetCurrentUserModel();
    }
}
