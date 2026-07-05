using ShoppingCart.Models.Dtos;
using ShoppingCart.Models.Responses;

namespace ShoppingCart.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<bool>> RegisterUser(RegisterRequest request);
        Task<ServiceResponse<List<UserDto>>> GetUsers();
        Task<ServiceResponse<UserDto>> GetUserById(string userId);
    }
}
