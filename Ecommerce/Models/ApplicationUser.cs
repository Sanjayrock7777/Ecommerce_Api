using Microsoft.AspNetCore.Identity;
namespace Ecommerce.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string Fullname { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        public DateTime CreatedDate { get; set; } 
        public Customer Customer { get; set; }
        public Employee Employee { get; set; }
    }
}
