using System.Text.Json.Serialization;

namespace Ecommerce.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentMethod {  get; set; }  
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [JsonIgnore]
        public ICollection<OrderItem> OrderItems { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
