using Ecommerce.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
using Ecommerce.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly EcomDbContext context;
        public ProductController(EcomDbContext _context)
        {
            context = _context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Getallproduct()
        {
            return await context.Products.ToListAsync();

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> Getproductbyid(int id)
        {
            return await context.Products.Where(p => p.CategoryId == id).ToListAsync(); 
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Updateproduct(int id, Product products)
        {
             if(id != products.Id)
            {
                return BadRequest();
            }
             context.Entry(products).State = EntityState.Modified;
            try
            {
                await context.Products.AddAsync(products);
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Product>> Addproduct(Productdto dto)
        {
            var category = await context.Categories.FindAsync(dto.CategoryId);
            if(category == null)
            {
                return BadRequest("Invalid Category Id");
            }
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                stock = dto.stock,
                CategoryId = dto.CategoryId,
            };
             context.Products.Add(product);
             await context.SaveChangesAsync();
            return Ok(product);
            
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Deleteproduct(int id)
        {
            var products = await context.Products.FindAsync(id);
            if(products == null)
            {
                return NotFound();
            }
            context.Products.Remove(products);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
