using Ecommerce.Dto;
using Ecommerce.Models;
using Ecommerce.Repositories.Interface;
using Ecommerce.Services.Interface;

namespace Ecommerce.Services.Service
{
    public class CartService: ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> AddToCartAsync(string userId, Cartdto dto)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItem = new List<CartItem>
                {
                    new CartItem { ProductId = dto.ProductId, Quantity = dto.Quantity }
                }
                };
                await _cartRepository.AddCartAsync(cart);
            }
            else
            {
                var existingItem = cart.CartItem.FirstOrDefault(ci => ci.ProductId == dto.ProductId);
                if (existingItem != null)
                    existingItem.Quantity += dto.Quantity;
                else
                    cart.CartItem.Add(new CartItem { ProductId = dto.ProductId, Quantity = dto.Quantity });

                await _cartRepository.UpdateCartAsync(cart);
            }
            return true;
        }

        public async Task<object> GetCartAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) return null;

            return cart.CartItem.Select(ci => new
            {
                ci.Id,
                ci.ProductId,
                ci.Product.Name,
                ci.Quantity,
                ci.Product.Price,
                Total = ci.Product.Price * ci.Quantity
            });
        }

        public async Task<bool> UpdateCartItemQuantityAsync(int cartItemId, int quantity)
        {
            var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);
            if (cartItem == null) return false;
            cartItem.Quantity = quantity;
            await _cartRepository.SaveChangesAsync();
            return true;
            
            return false;
        }

        public async Task<bool> DeleteCartItemAsync(int cartItemId)
        {
            var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);
            if (cartItem == null) return false;

            await _cartRepository.DeleteCartItemAsync(cartItem);
            return true;
        }
    }
}
