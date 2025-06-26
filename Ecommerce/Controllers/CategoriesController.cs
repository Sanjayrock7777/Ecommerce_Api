using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
using Ecommerce.Dto;
using Ecommerce.Services.Interface;
using Microsoft.AspNetCore.Authorization;
namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }
   
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _categoriesService.GetCategoriesAsync();

            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoriesService.GetCategoriesByIdAsync(id);
            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Categorydto category)
        {
            var result = await _categoriesService.UpdateCategoriesAsync(id, category);
            return result != null ? Ok(result) : NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> PostCategory(Categorydto dto)
        {
            var result = await _categoriesService.AddCategoriesByIdAsync(dto);
            return result != null ? Ok( new { Message = "Category Added"}) : BadRequest("Failed to Category");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
             var result = await _categoriesService.DeleteCategoriesAsync(id);
            return result!=null ? NoContent() : NotFound();
        }
    }
}
