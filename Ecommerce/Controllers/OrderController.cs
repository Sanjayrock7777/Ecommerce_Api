using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Ecommerce.Services.Interface;
namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(IOrderService orderService, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost("createorder")]
        public async Task<IActionResult> CreateOrder([FromBody] Orderdto dto)
        {
            if (dto == null) return BadRequest(new { success = false, message = "Invalid order data" });

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized(new { success = false, message = "Invalid Token" });

            try
            {
                var order = await _orderService.CreateOrderAsync(userId, dto);
                return Ok(new { success = true, message = "Order placed successfully!", orderId = order.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("userorders")]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized(new { success = false, message = "Invalid Token" });
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return orders != null? Ok(new { success = true, orders }): NotFound(new { success = false, message = "No orders found for the user." });
        }

    }
}
