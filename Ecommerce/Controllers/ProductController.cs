using Ecommerce.Data;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
using Ecommerce.Dto;
using Ecommerce.Services.Interface;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("GetAllProductsforcustomer")]
        public async Task<ActionResult<IEnumerable<Product>>> GetallproductforCustomer()
        {
            var product = await _productService.GetProductAsync();
            return Ok(product);
        }
        [HttpGet("GetAllProductsforAdmin")]
        public async Task<ActionResult<IEnumerable<Product>>> GetallproductforAdmin()
        {
            var product = await _productService.GetProductforAdminAsync();
            return Ok(product);
        }

        [HttpGet("GetProductByProductId/{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) 
                return NotFound(new { Message = "Product not found" });

            return Ok(product);
        }
        [HttpGet("GetProductByCategoryId/{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetproductbyCategoryid(int id)
        {
            var productByCategoryId = await _productService.GetProductsByCategoryIdAsync(id);   
            return Ok(productByCategoryId); 
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Updateproduct(int id, Productdto product)
        {
            var result = await _productService.UpdateProductAsync(id,product);
            return result!=null ? Ok(new {Message="Updated Successfully"}) : BadRequest("Failed to Delete");  
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Product>> Addproduct(Productdto dto)
        {
            var result = await _productService.AddProductAsync(dto);
            return result != null ? Ok(new { Message = "Category Added" }) : BadRequest("Failed to Category");

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Deleteproduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            return result != null ? Ok(new {Message = "Deleted Successfully"}):BadRequest("Failed to Category");
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query cannot be empty");

            var products = await _productService.SearchProductsAsync(query);
            if (products.Count != 0)
            {
                return Ok(products);
            }
            else
            {
                return Ok(new { productfoundstatus = false });
            }
        }

    }
}
