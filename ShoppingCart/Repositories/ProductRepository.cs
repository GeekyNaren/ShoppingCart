using MongoDB.Driver;
using ShoppingCart.Interfaces;
using ShoppingCart.Models;

namespace ShoppingCart.Repositories
{
    public class ProductRepository : BaseMongoRepository<Product>, IProductRepository
    {
        public ProductRepository(IMongoDatabase database) : base(database, "products")
        {
        }
    }
}
