using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.Repositories.Repository
{
    public class OrderRepository:IOrderRepository
    {
        private readonly EcomDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderRepository(EcomDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetUserOrdersAsync(string userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId).Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate).ToListAsync();
        }

        public async Task<Cart> GetCartByUserIdAsync(string userId) =>
            await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);

        public async Task<ApplicationUser> GetUserByIdAsync(string userId) =>
            await _userManager.FindByIdAsync(userId);

        public async Task<List<CartItem>> GetCartItemsAsync(int cartId) =>
            await _context.CartItems.Where(c => c.CartId == cartId).ToListAsync();

        public async Task<List<Product>> GetProductsByIdsAsync(List<int> productIds) =>
            await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

        public async Task RemoveCartItemsAsync(List<CartItem> cartItems)
        {
            _context.CartItems.RemoveRange(cartItems);
            await SaveChangesAsync();
        }
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
