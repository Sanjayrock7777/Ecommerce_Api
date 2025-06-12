using Ecommerce.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IEcommercedetailsRepository _detailrepository;
        public AdminController(IEcommercedetailsRepository detailrepository)
        {
            _detailrepository = detailrepository;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllEcommerceDetails")]
        public async Task<IActionResult> GetAllEcommerceDetails()
        {
            var revenue = await _detailrepository.Revenue();
            var totalproduct = await _detailrepository.TotalProduct();
            var totalcategory = await _detailrepository.TotalCategory();
            var totalcustomer = await _detailrepository.CustomerCount();
            var (pending, shipping, delivered) = await _detailrepository.OrdersCount();
            return Ok(new { revenue, totalproduct, totalcategory, totalcustomer, pending, shipping, delivered});
        }
    }
}
