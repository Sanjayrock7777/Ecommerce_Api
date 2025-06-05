using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Repositories.Interface
{
    public interface IEcommercedetailsRepository
    {
        Task<object> Revenue();
        Task<int> TotalProduct();
        Task<int> TotalCategory();
        Task<(int pendingCount, int ShipingCount, int DeliveredCount)> OrdersCount();
        Task<int> CustomerCount();
        //Task<IActionResult> Rev


    }
}
