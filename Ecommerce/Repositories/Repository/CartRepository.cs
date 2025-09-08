using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repositories.Repository
{
    public class CartRepository: ICartRepository
    {
        private readonly EcomDbContext _context;

        public CartRepository(EcomDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartByUserIdAsync(string userId) =>
           await _context.Carts.Include(c => c.CartItem).ThenInclude(ci => ci.Product) .FirstOrDefaultAsync(c => c.UserId == userId);

        public async Task AddCartAsync(Cart cart)
        {
            _context.Carts.Add(cart);
            await SaveChangesAsync();
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await SaveChangesAsync();
        }
        

        public async Task<CartItem> GetCartItemByIdAsync(int cartItemId) =>
            await _context.CartItems.Include(c=>c.Product).FirstOrDefaultAsync(c => c.Id == cartItemId);

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await SaveChangesAsync();
        }
        public async Task DeleteCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
