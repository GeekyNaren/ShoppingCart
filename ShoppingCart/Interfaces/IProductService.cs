using ShoppingCart.ExtensionService;
using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResponse<bool>> AddProduct(AddProductDto request);
        Task<ServiceResponse<List<ProductResponseDto>>> GetProducts();
        Task<ServiceResponse<ProductResponseDto>> GetProductById(string id);
        Task<ServiceResponse<bool>> UpdateProduct(UpdateProductDto request);
        Task<ServiceResponse<bool>> DeleteProduct(string id);
    }
}
