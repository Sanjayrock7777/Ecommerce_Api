using Ecommerce.Data;
using Ecommerce.Dto;
using Ecommerce.Models;
using Ecommerce.Repositories.Interface;
using Ecommerce.Services.Interface;
namespace Ecommerce.Services.Service
{
    public class CategoriesService: ICategoriesService
    {
        private readonly ICategoriesRepository _categoryRepository;

        public CategoriesService(ICategoriesRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetCategoriesAsync();
        }
        public async Task<Category> GetCategoriesByIdAsync(int id)
        {
            return await _categoryRepository.GetCategoryByIdAsync(id);
        }
        public async Task<bool> AddCategoriesByIdAsync(Categorydto model)
        {
            string imagepath = null;
            if (model.ImageFile != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
                var path = Path.Combine("wwwroot/images", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
                imagepath = "/images" + fileName;
            }
            var category = new Category
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = imagepath
            };
           return await _categoryRepository.AddCategoryAsync(category);

        }
        public async Task<bool> UpdateCategoriesAsync(int id, Categorydto category)
        {
            return await _categoryRepository.UpdateCategoryAsync(category, id);
        }
        public async Task<bool> DeleteCategoriesAsync(int id)
        {
            return await _categoryRepository.DeleteCategoryAsync(id);   
        }
    }
}
