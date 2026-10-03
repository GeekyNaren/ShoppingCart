using ShoppingCart.Models.Entities;

namespace ShoppingCart.Interfaces
{
    public interface IProductRepository : IBaseMongoRepository<Product>
    {
    }
}
