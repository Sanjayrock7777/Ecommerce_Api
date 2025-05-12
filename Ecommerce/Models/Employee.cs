namespace Ecommerce.Models
{
    public enum EmployeeRole
    {
        Maintenance, Delivery
    }
    public class Employee
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        
        public ApplicationUser User { get; set; }
        public EmployeeRole Role { get; set; }

    }
}
