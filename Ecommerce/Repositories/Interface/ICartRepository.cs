using Ecommerce.Models;

namespace Ecommerce.Repositories.Interface
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserIdAsync(string userId);
        Task AddCartAsync(Cart cart);
        
        Task UpdateCartAsync(Cart cart);
        Task<CartItem> GetCartItemByIdAsync(int cartItemId);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task DeleteCartItemAsync(CartItem cartItem);
        Task SaveChangesAsync();

    }
}
