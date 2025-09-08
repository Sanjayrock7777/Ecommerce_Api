using Ecommerce.Dto;

namespace Ecommerce.Services.Interface
{
    public interface ICartService
    {
        Task<bool> AddToCartAsync(string userId, Cartdto dto);
        Task<object> GetCartAsync(string userId);
        Task<bool> UpdateCartItemQuantityAsync(int cartItemId, int quantity);
        Task<bool> DeleteCartItemAsync(int cartItemId);
    }
}
