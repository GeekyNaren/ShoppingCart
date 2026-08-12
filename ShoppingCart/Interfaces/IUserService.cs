using ShoppingCart.ExtensionService;
using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<bool>> RegisterUser(UserRegisterRequestDto request);
        Task<ServiceResponse<List<UserResponseDto>>> GetUsers();
        Task<ServiceResponse<UserResponseDto>> GetUserById(string userId);
        Task<ServiceResponse<UserResponseDto>> UpdateUser(UpdateUserRequestDto request);
        Task<ServiceResponse<bool>> DeleteUser(string userId);
    }
}
