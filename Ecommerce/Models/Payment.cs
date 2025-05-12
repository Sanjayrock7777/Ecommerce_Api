namespace Ecommerce.Models
{
    public class Payment
    {
        public int Id { get; set; } 
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string PaymentMethod { get; set; }
        public decimal? Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}
