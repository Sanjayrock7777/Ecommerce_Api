using Ecommerce.Models;
namespace Ecommerce.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<bool> AddUserAsync(ApplicationUser user, string role);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<bool> UpdateUserAsync(ApplicationUser user);
    }
}
