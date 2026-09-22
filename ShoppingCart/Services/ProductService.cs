using ShoppingCart.ExtensionService;
using ShoppingCart.Interfaces;
using ShoppingCart.Models;
using ShoppingCart.Models.Dtos;

namespace ShoppingCart.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UserService> _logger;

        public ProductService(IProductRepository productRepository, IUserRepository userRepository, ICurrentUserService currentUserService, ILogger<UserService> logger)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
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
    }
}
