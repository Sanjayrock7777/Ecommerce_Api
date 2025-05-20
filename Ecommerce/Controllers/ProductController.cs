using Ecommerce.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
using Ecommerce.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Services.Interface;
namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Getallproduct()
        {
            var product = await _productService.GetProductAsync();
            return Ok(product);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> Getproductbyid(int id)
        {
            var productByCategoryId = await _productService.GetProductsByCategoryIdAsync(id);   
            return Ok(productByCategoryId); 
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Updateproduct(int id, Productdto product)
        {
            var result = _productService.UpdateProductAsync(id,product);
            return result!=null ? Ok(new {Message="Updated Successfully"}) : BadRequest("Failed to Delete");  
        }
        [HttpPost]
        public async Task<ActionResult<Product>> Addproduct(Productdto dto)
        {
            var result = await _productService.AddProductAsync(dto);
            return result != null ? Ok(new { Message = "Category Added" }) : BadRequest("Failed to Category");

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Deleteproduct(int id)
        {
            var result = _productService.DeleteProductAsync(id);
            return result != null ? Ok(new {Message = "Deleted Successfully"}):BadRequest("Failed to Category");
        }

    }
}
