using Ecommerce.Dto;
using Ecommerce.Models;

namespace Ecommerce.Repositories.Interface
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<Category>> GetCategoriesAsync(); 
        Task<Category> GetCategoryByIdAsync(int id);
        Task<bool> AddCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Categorydto category, int id);
        Task<bool> DeleteCategoryAsync(int id);
        
    }
}
