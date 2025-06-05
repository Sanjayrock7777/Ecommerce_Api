using Ecommerce.Dto;
using Ecommerce.Models;

namespace Ecommerce.Services.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductAsync();
        Task<IEnumerable<Product>> GetProductforAdminAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int id);
        Task<bool> AddProductAsync(Productdto product);
        Task<bool> UpdateProductAsync(int id,Productdto product);
        Task<bool> DeleteProductAsync(int id);
        Task<List<Product>> SearchProductsAsync(string query);
    }
}
