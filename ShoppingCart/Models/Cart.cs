namespace ShoppingCart.Models
{
    public class Cart
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public DateTime UpdatedAt { get; set; }
    }

    public class CartItem
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
