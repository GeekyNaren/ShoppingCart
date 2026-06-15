using MongoDB.Driver;
using ShoppingCart.Interfaces;
using ShoppingCart.Models;

namespace ShoppingCart.Repositories
{
    public class UserRepository : BaseMongoRepository<User>, IUserRepository
    {
        public UserRepository(IMongoDatabase database) : base(database, "users")
        {
        }

        public async Task<User?> GetByEmailAsync(string email) =>
            await _collection.Find(u => u.Email == email).FirstOrDefaultAsync();

        public async Task<User?> GetByUsernameAsync(string username) =>
            await _collection.Find(u => u.Username == username).FirstOrDefaultAsync();
    }
}
