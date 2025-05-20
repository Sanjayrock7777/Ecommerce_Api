using Ecommerce.Models;
using Ecommerce.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Services.Interface;
namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ICartService cartService, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] Cartdto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized("Invalid Token");

            var success = await _cartService.AddToCartAsync(userId, dto);
            return success ? Ok(new { success = true, message = "Item added to cart successfully" }) : BadRequest("Failed to add item.");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized("Invalid Token");

            var cart = await _cartService.GetCartAsync(userId);
            return cart != null ? Ok(cart) : NotFound("Cart not found.");
        }
        [HttpPut("{cartItemId}/quantity")]
        public async Task<IActionResult> UpdateCartItemQuantity(int cartItemId, int quantity)
        {
            var success = await _cartService.UpdateCartItemQuantityAsync(cartItemId, quantity);
            if (!success)
            {
                return NotFound(new { success = false, message = "Cart item not found" });
            }
            return Ok(new { success = true, message = "Quantity updated successfully!", cartItemId, quantity });
        }
        [HttpDelete("{cartItemId}")]
        public async Task<IActionResult> DeleteCartItem(int cartItemId)
        {
            var success = await _cartService.DeleteCartItemAsync(cartItemId);
            if (!success)
            {
                return NotFound(new { success = false, message = "Cart item not found" });
            }

            return Ok(new { success = true, message = "Item removed from cart successfully" });
        }
    }

}
