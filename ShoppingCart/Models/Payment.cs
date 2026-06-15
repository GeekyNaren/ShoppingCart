namespace ShoppingCart.Models
{
    public class Payment
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string PaymentMethod { get; set; } // e.g. "CreditCard", "PayPal"
        public string TransactionId { get; set; }
        public string Status { get; set; } // Success, Failed, Pending
    }
}
