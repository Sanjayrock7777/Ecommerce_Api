using Ecommerce.Dto;
using Ecommerce.Models;
namespace Ecommerce.Services.Interface
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(RegisterCustomerdto model, string role, string password);
        Task<(bool success, string role, string userId, string token)> LoginAsync(Logindto model);
        Task<bool> UpdateCustomerAsync(string userId, RegisterCustomerdto update);
        Task<ApplicationUser> GetCustomerAddressAsync(string userId);


    }
}
