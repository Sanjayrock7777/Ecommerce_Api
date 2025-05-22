using Ecommerce.Data;
using Ecommerce.Dto;
using Ecommerce.Models;
using Ecommerce.Repos;
using Ecommerce.Repositories.Interface;
using Ecommerce.Services.Interface;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Services.Service
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly EcomDbContext _context;
        private readonly JwttokenService _jwtTokenService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserService(IUserRepository userRepository, EcomDbContext context, JwttokenService jwttokenService, SignInManager<ApplicationUser> signInManager)
        {
            _userRepository = userRepository;
            _context = context; 
            _jwtTokenService = jwttokenService;
            _signInManager = signInManager;
        }
        public async Task<bool> RegisterUserAsync(RegisterCustomerdto model, string role, string password)
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
            var success = await _userRepository.AddUserAsync(user, role, password);
            if(!success)
            {
                return false;
            }
            if (string.IsNullOrEmpty(user.Id))
            {
                return false;
            }
            var customer = new Customer
            {
                UserId = user.Id,
                User = user
            };
            _context.Customers.Add(customer);
            var cart = new Cart
            {
                UserId = user.Id,
                User = user
            };
            _context.Carts.Add(cart);   
            return await _context.SaveChangesAsync() > 0;

        }
        public async Task<(bool success, string role, string userId, string token)> LoginAsync(Logindto model)
        {
            var user = await _userRepository.GetUserByEmailAsync(model.Email);
            if (user == null)
            {
                return (false, null, null, null);
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if(!result.Succeeded)
            {
                return (false, null, null, null);
            }
            var roles = await _userRepository.GetUserRolesAsync(user);
            string role = roles.FirstOrDefault() ?? "Unknown";
            var token = _jwtTokenService.GenerateToken(user, role);
            return (true, role, user.Id, token);    
        }
        public async Task<bool> UpdateCustomerAsync(string userId, RegisterCustomerdto update)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            user.Fullname = update.FullName ?? user.Fullname;
            user.Address = update.Address ?? user.Address;
            user.Pincode = update.Pincode ?? user.Pincode;
            user.PhoneNumber = update.PhoneNumber ?? user.PhoneNumber;
            return await _userRepository.UpdateUserAsync(user);
        }
        public async Task<ApplicationUser> GetCustomerAddressAsync(string userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

    }
}
