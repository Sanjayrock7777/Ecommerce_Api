using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Dto
{
    public class Registerdto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Pincode must be exactly 6 digits.")]
        public string Pincode { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits")]
        public string PhoneNumber { get; set; }
    }
}
