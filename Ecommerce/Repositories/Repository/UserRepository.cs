using Ecommerce.Models;
using Ecommerce.Repositories.Interface;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Repositories.Repository
{
    public class UserRepository : IUserRepository
    {
        
        private readonly UserManager<ApplicationUser> _userManager;
        public UserRepository(UserManager<ApplicationUser> userManager)
        {
           _userManager = userManager; 
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<bool> AddUserAsync(ApplicationUser user, string role)
        {
            var result = await _userManager.CreateAsync(user); 
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
                return true;
            }
            return false;
        }
        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
