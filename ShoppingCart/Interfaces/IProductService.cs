using ShoppingCart.ExtensionService;
using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResponse<bool>> AddProduct(AddProductDto request);
    }
}
