namespace ShoppingCart.Models.Dtos
{
    public class ProductResponseDto
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public string ProductCategory { get; set; } = string.Empty;
        public int ProductStockQuantity { get; set; }
        public string ProductCategoryId { get; set; } = string.Empty;
        public string ProductBrand { get; set; } = string.Empty;
        public decimal? ProductDiscountPrice { get; set; }
        public List<string> ProductImageUrls { get; set; } = [];
        public double ProductAverageRating { get; set; }
        public int ProductReviewCount { get; set; }
        public bool IsActive { get; set; }
    }
}
