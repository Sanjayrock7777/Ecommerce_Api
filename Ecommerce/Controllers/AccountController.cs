using Ecommerce.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Services.Interface;
using System.Linq.Expressions;

namespace Ecom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register/customer")]
        public async Task<IActionResult> RegisterCustomer(Registerdto model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Invalid data");
                }
                var success = await _userService.RegisterUserAsync(model, "Customer", model.Password);
                return success ? Ok(new { message = "Customer registered successfully" }) : BadRequest("Registration failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500,new {success = false,Message=ex.Message});    
            }
        }
        [Authorize(Roles ="Admin")]
        [HttpPost("register/Admin")]
        public async Task<IActionResult> RegisterAdmin(Registerdto model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Invalid admin data");
                }
                var success = await _userService.RegisterUserAsync(model, "Admin", model.Password);
                return success ? Ok(new { message = "Admin Registered Successfully" }) : BadRequest("Registration Failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, Message = ex.Message });
            }
        }
        [Authorize]
        [HttpPut("update/customer")]
        public async Task<IActionResult> UpdateCustomer(Registerdto update)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _userService.UpdateCustomerAsync(userid, update);
            return Ok( new {Message = "Customer details updated Successfully" });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(Logindto model)
        {
            var (success, role, userId, token) = await _userService.LoginAsync(model);
            return success ? Ok(new { success, role, userId, token }) : Unauthorized("Invalid credentials");
        }
        [Authorize(Roles ="Customer")] 
        [HttpGet("address")]
        public async Task<IActionResult> GetAddressByUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { success = false, message = "Invalid Token" });

            var user = await _userService.GetCustomerAddressAsync(userId);
            if (user == null)
                return NotFound(new { success = false, message = "User not found" });

            return Ok(new
            {
                success = true,
                name = user.Fullname,
                email = user.Email,
                address = user.Address,
                pincode = user.Pincode,
                mobile = user.PhoneNumber
            });
        }


    }
}
