using Ecommerce.Models;
using Ecommerce.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Ecommerce.Data;
using Ecommerce.Repos;
using Microsoft.AspNetCore.Authorization;

namespace Ecom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly EcomDbContext _context;
        private readonly JwttokenService _jwt;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,
                                 EcomDbContext context,
                                 IConfiguration configuration,
                                 JwttokenService jwt)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
            _jwt = jwt;
        }

        [HttpPost("register/customer")]
        public async Task<IActionResult> RegisterCustomer(RegisterCustomerdto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Fullname = model.FullName,
                Address = model.Address,
                Pincode = model.Pincode,
                PhoneNumber = model.PhoneNumber,
                CreatedDate = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(new { success = false, errors = result.Errors });

            var customer = new Customer
            {
                UserId = user.Id,
                User = user
            };

            _context.Customers.Add(customer);
            var cart = new Cart
            {
                UserId = user.Id,
                CreateDate = DateTime.UtcNow
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Customer registered successfully" });
        }
        [HttpPut("update/customer")]
        public async Task<IActionResult> UpdateCustomer(RegisterCustomerdto update)
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return NotFound("User not found");
            }
            user.Fullname = update.FullName ?? user.Fullname;
            user.Address = update.Address ?? user.Address;
            user.Pincode = update.Pincode ?? user.Pincode;
            user.PhoneNumber = update.PhoneNumber ?? user.PhoneNumber;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok("Customer details updated Successfully");
        }
        [HttpPost("register/employee")]
        public async Task<IActionResult> RegisterEmployee(RegisterEmployee model, EmployeeRole role)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Fullname = model.FullName,
                Address = model.Address,
                Pincode = model.Pincode,
                PhoneNumber = model.PhoneNumber,
                CreatedDate = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var employee = new Employee
            {
                UserId = user.Id,
                User = user,
                Role = role
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return Ok("Employee registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Logindto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return Unauthorized("Invalid credentials");

            string role = await _context.Customers.AnyAsync(c => c.UserId == user.Id) ? "Customer" :
                          await _context.Employees.AnyAsync(e => e.UserId == user.Id) ? "Employee" : "Unknown";

            var token = _jwt.GenerateToken(user, role);

            return Ok(new { success = true, role, userId = user.Id, token });
        }
        [Authorize(Roles="Customer")] 
        [HttpGet("address")]
        public async Task<IActionResult> GetAddressByUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { success = false, message = "Invalid Token" });

            var user = await _userManager.FindByIdAsync(userId);
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
