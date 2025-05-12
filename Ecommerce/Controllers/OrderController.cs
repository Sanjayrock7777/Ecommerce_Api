using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly EcomDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderController(EcomDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize]
        [HttpPost("createorder")]
        public async Task<IActionResult> CreateOrder([FromBody] Orderdto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { success = false, message = "Invalid order data" });
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound(new { success = false, message = "User not found" });
            }

            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                return BadRequest(new { success = false, message = "Cart not found for user." });
            }
            var order = new Order
            {
                UserId = userId,
                Address = dto.Address,
                Pincode = dto.Pincode,
                TotalPrice = dto.TotalPrice,
                PaymentMethod = dto.PaymentMethod,
                OrderDate = DateTime.Now,
                Status = "Pending",
                OrderItems = dto.Orderitems.Select(oi => new OrderItem
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            foreach (var item in order.OrderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    product.stock -= item.Quantity;
                }
            }

            var cartItems = _context.CartItems.Where(c => c.CartId == cart.Id).ToList();

            
            var cartItemsToRemove = cartItems.Where(c => dto.Orderitems.Any(o => o.ProductId == c.ProductId)).ToList();

            if (cartItemsToRemove.Any())
            {
                _context.CartItems.RemoveRange(cartItemsToRemove);
            }

            await _context.SaveChangesAsync();


            return Ok(new { success = true, message = "Order placed successfully!", orderId = order.Id });
        }
        [HttpGet("userorders")]
        public async Task<IActionResult> GetuserOrder()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { success = false, message = "Invalid Token" });
            }
            var orders = await _context.Orders.Where(o => o.UserId == userId).Include(o =>o.OrderItems).OrderByDescending(o=> o.OrderDate).Select(
                o=> new
                {
                    o.OrderDate,
                    o.PaymentMethod,
                    o.Status,
                    o.Address,
                    OrderItems = o.OrderItems.Select(oi => new
                    {
                        ProductName = oi.Product.Name,
                        oi.Quantity,
                        oi.Price
                    }).ToList()
                }).ToListAsync();
            if (orders == null) 
            {
                return NotFound(new { success = false, message = "No orders found for the user." });
            }
            return Ok(new { success = true, orders });
        }

    }
}
