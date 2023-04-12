namespace WebApplication1.Models
{
    public class Customer
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public bool IsDeleted { get; set; }
        public List<Cart> Cart { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Order> Orders { get; set; }

    }
}
