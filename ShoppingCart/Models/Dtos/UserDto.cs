namespace ShoppingCart.Models.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }  // MongoDB ObjectId as string
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // e.g. "Customer", "Admin"
        public DateTime CreatedAt { get; set; }
    }
}
