using ShoppingCart.Models;

namespace ShoppingCart.Interfaces
{
    public interface IProductRepository : IBaseMongoRepository<Product>
    {
    }
}
