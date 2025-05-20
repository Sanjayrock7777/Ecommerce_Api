using Ecommerce.Dto;
using Ecommerce.Models;

namespace Ecommerce.Services.Interface
{
    public interface ICategoriesService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category> GetCategoriesByIdAsync(int id);
        Task<bool> AddCategoriesByIdAsync(Categorydto dto);
        Task<bool> UpdateCategoriesAsync(int id, Categorydto category);
        Task<bool> DeleteCategoriesAsync(int id);

    }
}
