namespace WebApplication1.Models
{
    public class Offer
    {
        public int ID { get; set; }
        public float Precentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Code { get; set; }
        public float MinOrderAmount { get; set; }

    }
}
