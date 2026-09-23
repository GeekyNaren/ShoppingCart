namespace ShoppingCart.Models.Dtos
{
    public static class Constants
    {
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Customer = "Customer";
        }
        public enum OrderStatus
        {
            Pending,
            Processing,
            Shipped,
            Delivered,
            Cancelled
        }
        public enum PaymentStatus
        {
            Pending,
            Completed,
            Failed,
            Refunded
        }
        public enum ProductCategory
        {
            Electronics,
            Clothing,
            HomeAppliances,
            Books,
            Sports,
            Beauty,
            Toys,
            Automotive
        }
    }
}
