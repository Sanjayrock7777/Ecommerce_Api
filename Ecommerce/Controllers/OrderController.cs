using Microsoft.AspNetCore.Mvc;
using Ecommerce.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Ecommerce.Services.Interface;
using Ecommerce.Models;
namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("createorder")]
        public async Task<IActionResult> CreateOrder([FromBody] Orderdto dto)
        {
            if (dto == null) return BadRequest(new { success = false, message = "Invalid order data" });

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized(new { success = false, message = "Invalid Token" });

            try
            {
                var order = await _orderService.CreateOrderAsync(userId, dto);
                if (order == null) return NotFound();
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

        [Authorize(Roles = "Admin")]
        [HttpPatch("UpdateOrderStatus/{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, string orderStatus)
        {
            var orderstatusupdate = await _orderService.UpdateOrderStatusAsync(id, orderStatus);
            return orderstatusupdate != null ? Ok(new { success = true, Message = "Order Status Updated Successfully" }) : NotFound(new { success = false });
        }

       // [Authorize(Roles = "Admin")]
        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<IEnumerable<IActionResult>>> GetAllOrders()
        {
            var order = await _orderService.GetAllOrdersAsync();
            if (order == null) 
                return NotFound(new { Message = "Orders not found" });
            return Ok(order);   
        }
    }
}
