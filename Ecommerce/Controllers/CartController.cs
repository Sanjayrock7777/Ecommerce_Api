using Ecommerce.Models;
using Ecommerce.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ecommerce.Data;
using Microsoft.AspNetCore.Authorization;
using System;
namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly EcomDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CartController(EcomDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize] 
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] Cartdto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; //  Correct extraction


            if (userId == null)
                return Unauthorized("Invalid Token");

            var cart = await _context.Carts.Include(c => c.CartItem).FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId, 
                    CartItem = new List<CartItem>
            {
                new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                }
            }
                };

                _context.Carts.Add(cart);
            }
            else
            {
                var existingItem = cart.CartItem.FirstOrDefault(ci => ci.ProductId == dto.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity += dto.Quantity;
                }
                else
                {
                    cart.CartItem.Add(new CartItem
                    {
                        ProductId = dto.ProductId,
                        Quantity = dto.Quantity
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Item added to cart successfully!" });
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { success = false, message = "Invalid Token" });

            var cart = await _context.Carts.Include(c => c.CartItem)
                                           .ThenInclude(ci => ci.Product)
                                           .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return NotFound("Cart not found.");

            return Ok(cart.CartItem.Select(ci => new
            {
                ci.Id,
                ci.ProductId,
                ci.Product.Name,
                ci.Quantity,
                ci.Product.Price,
                Total = ci.Product.Price * ci.Quantity
            }));
        }

        [HttpPut("{cartItemId}/quantity")]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, int quantity)
        {
            var cartitem = await _context.CartItems.FindAsync(cartItemId);
            if (cartitem == null)
            {
                return NotFound(new { success = false, message = "CartItem not found" });
            }
            cartitem.Quantity = quantity;
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Quantity updated successfully!", cartitem });
        }
        [HttpDelete("{cartItemId}")]
        public async Task<IActionResult> DeletecartItem(int cartItemId)
        {
            var cartitem = await _context.CartItems.FindAsync(cartItemId);
            if (cartitem == null)
            {
                return NotFound(new { success = false, message = "CartItem not found" });
            }
            _context.CartItems.Remove(cartitem);
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Cartitem Deleted successfully!", cartitem });
        }


    }

}
