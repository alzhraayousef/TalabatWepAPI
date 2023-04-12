namespace WebApplication1.Models
{
    public class Review
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        public int ResturantID { get; set; }
        public Resturant Resturant { get; set; }

    }
}
