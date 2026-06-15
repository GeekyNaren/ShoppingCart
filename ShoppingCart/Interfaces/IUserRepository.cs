using ShoppingCart.Models;

namespace ShoppingCart.Interfaces
{
    public interface IUserRepository : IBaseMongoRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
    }
}
