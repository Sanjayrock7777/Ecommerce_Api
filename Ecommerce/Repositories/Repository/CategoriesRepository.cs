using Ecommerce.Data;
using Ecommerce.Dto;
using Ecommerce.Models;
using Ecommerce.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.Repositories.Repository
{
    public class CategoriesRepository: ICategoriesRepository
    {
        private readonly EcomDbContext _context;
        public CategoriesRepository(EcomDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<bool> AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            return await _context.SaveChangesAsync() > 0;

        }
        public async Task<bool> UpdateCategoryAsync(Categorydto dto, int id)
        {
            var category = await _context.Categories.FindAsync(id); 
            if (category == null) return false;

            category.Name = dto.Name;
            category.Description = dto.Description;
            if (dto.ImageFile != null)
            {
                var filePath = Path.Combine("wwwroot/images", dto.ImageFile.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }

                category.ImageUrl = "/images/" + dto.ImageFile.FileName;
            }
            _context.Entry(category).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
