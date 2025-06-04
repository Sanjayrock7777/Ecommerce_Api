using Ecommerce.Models;

namespace Ecommerce.Repositories.Interface
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<List<Order>> GetUserOrdersAsync(string userId);
        Task<Order> GetOrdersByIDAsync(int orderid);
        Task<List<Order>> GetAllOrdersAsync();
        Task<bool> UpdateOrderStatusAsync(Order order);
        Task<Cart> GetCartByUserIdAsync(string userId);
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<List<CartItem>> GetCartItemsAsync(int cartId);
        Task<List<Product>> GetProductsByIdsAsync(List<int> productIds);
        Task RemoveCartItemsAsync(List<CartItem> cartItems);
        Task SaveChangesAsync();
    }
}
