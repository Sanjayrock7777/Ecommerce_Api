using Ecommerce.Data;
using Ecommerce.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Repositories.Repository
{
    public class EcommercedetailsRepository:IEcommercedetailsRepository
    {
        private readonly EcomDbContext _context;
        public EcommercedetailsRepository(EcomDbContext context)
        {
            _context = context;
        }
        public async Task<int> TotalProduct()
        {
            return _context.Products.Count();
        }
        public async Task<int> TotalCategory()
        {
            return _context.Categories.Count();
        }
        public async Task<int> CustomerCount()
        {
            return   _context.Customers.Count();
        }
        public async Task<(int pendingCount, int ShipingCount, int DeliveredCount)> OrdersCount()
        {
            var pending =  _context.Orders.Where(o => o.Status == "Pending").Count();
            var shipping = _context.Orders.Where(o => o.Status == "Shipping").Count();
            var delivered = _context.Orders.Where(o => o.Status == "Delivered").Count();
            return (pending, shipping, delivered);
        }
        public async Task<object> Revenue()
        {
            return _context.Orders.GroupBy(o => o.OrderDate.Date) .Select(g => new
            {
            OrderDate = g.Key,
            TotalRevenue = g.Sum(o => o.TotalPrice) 
            }).OrderByDescending(o => o.OrderDate).ToList();
        }

    }
}
