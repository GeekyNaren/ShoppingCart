using ShoppingCart.ExtensionService;
using ShoppingCart.Interfaces;
using ShoppingCart.Models;
using ShoppingCart.Models.Dtos;
using ShoppingCart.Repositories;

namespace ShoppingCart.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UserService> _logger;

        public ProductService(IProductRepository productRepository, ICurrentUserService currentUserService, ILogger<UserService> logger)
        {
            _productRepository = productRepository;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> AddProduct(AddProductDto request)
        {
            if (_currentUserService.Role != Constants.Roles.Admin)
            {
                _logger.LogWarning("Unauthorized user {UserId} with role {Role} attempted to add a product", _currentUserService.UserId, _currentUserService.Role);
                return ServiceResponse<bool>.Fail("Unauthorized access.");
            }
            var product = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Category = request.Category,
                StockQuantity = request.StockQuantity,
                CategoryId = request.CategoryId,
                Brand = request.Brand,
                DiscountPrice = request.DiscountPrice,
                ImageUrls = request.ImageUrls ?? [],
                Specifications = request.Specifications ?? [],
                AverageRating = request.AverageRating ?? 0.0,
                ReviewCount = request.ReviewCount ?? 0,
                IsActive = request.IsActive ?? true
            };
            await _productRepository.AddAsync(product);
            _logger.LogInformation("Product added successfully with name {ProductName}", request.Name);
            return ServiceResponse<bool>.Ok(true);
        }
        public async Task<ServiceResponse<List<ProductResponseDto>>> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();
            var productDtos = products.Select(p => new ProductResponseDto
            {
                ProductId = p.Id,
                ProductName = p.Name,
                ProductDescription = p.Description,
                ProductPrice = p.Price,
                ProductCategory = p.Category,
                ProductStockQuantity = p.StockQuantity,
                ProductCategoryId = p.CategoryId,
                ProductBrand = p.Brand,
                ProductDiscountPrice = p.DiscountPrice,
                ProductImageUrls = p.ImageUrls,
                ProductAverageRating = p.AverageRating,
                ProductReviewCount = p.ReviewCount,
                IsActive = p.IsActive
            }).ToList();
            return ServiceResponse<List<ProductResponseDto>>.Ok(productDtos);
        }
        public async Task<ServiceResponse<ProductResponseDto>> GetProductById(string id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with id {ProductId} not found", id);
                return ServiceResponse<ProductResponseDto>.Fail("Product not found.");
            }
            var productDto = new ProductResponseDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductPrice = product.Price,
                ProductCategory = product.Category,
                ProductStockQuantity = product.StockQuantity,
                ProductCategoryId = product.CategoryId,
                ProductBrand = product.Brand,
                ProductDiscountPrice = product.DiscountPrice,
                ProductImageUrls = product.ImageUrls,
                ProductAverageRating = product.AverageRating,
                ProductReviewCount = product.ReviewCount,
                IsActive = product.IsActive
            };
            return ServiceResponse<ProductResponseDto>.Ok(productDto);
        }
        public async Task<ServiceResponse<bool>> UpdateProduct(UpdateProductDto request)
        {
            return default;
        }
        public async Task<ServiceResponse<bool>> DeleteProduct(string productId)
        {
            if (_currentUserService.Role != Constants.Roles.Admin)
            {
                _logger.LogWarning("Unauthorized user {UserId} with role {Role} attempted to delete a product", _currentUserService.UserId, _currentUserService.Role, productId);
                return ServiceResponse<bool>.Fail("Unauthorized access.");
            }
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                _logger.LogWarning("Product with id {ProductId} not found for deletion", productId);
                return ServiceResponse<bool>.Fail("Product not found.");
            }
            await _productRepository.DeleteAsync(productId);
            _logger.LogInformation("Product with id {ProductId} deleted successfully", productId);
            return ServiceResponse<bool>.Ok(true);
        }
    }
}
