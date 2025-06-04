using Ecommerce.Dto;
using Ecommerce.Models;

namespace Ecommerce.Services.Interface
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string userId, Orderdto dto);
        Task<object> GetUserOrdersAsync(string userId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
        Task<IEnumerable<Object>> GetAllOrdersAsync();   
    }
}
