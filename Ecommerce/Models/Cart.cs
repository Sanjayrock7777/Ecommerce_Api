using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ecommerce.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }  
        public DateTime CreateDate { get; set; }  
        public ApplicationUser User { get; set; }
        public ICollection<CartItem> CartItem { get; set; }

    }
}
