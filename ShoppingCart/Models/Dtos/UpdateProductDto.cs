namespace ShoppingCart.Models.Dtos
{
    public class UpdateProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? Category { get; set; }
        public int? StockQuantity { get; set; }
        public string? CategoryId { get; set; }
        public string? Brand { get; set; }
        public decimal? DiscountPrice { get; set; }
        public List<string>? ImageUrls { get; set; }
        public List<string>? Specifications { get; set; }
        public double? AverageRating { get; set; }
        public int? ReviewCount { get; set; }
        public bool? IsActive { get; set; }
    }
}
