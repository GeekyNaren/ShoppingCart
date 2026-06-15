namespace ShoppingCart.Models
{
    public class Order
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // Pending, Paid, Shipped, Delivered
        public DateTime CreatedAt { get; set; }
    }

    public class OrderItem
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }
    }
}
