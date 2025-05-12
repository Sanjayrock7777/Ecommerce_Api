namespace Ecommerce.Dto
{
    public class Orderdto
    {
        //public string UserId { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentMethod { get; set; }   
        public ICollection<Orderitemdto> Orderitems { get; set; }
    }
    public class Orderitemdto
    {
        public int ProductId { get; set; }
        //public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } 
    }
}
