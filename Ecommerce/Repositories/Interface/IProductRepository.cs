using Ecommerce.Dto;
using Ecommerce.Models;

namespace Ecommerce.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<IEnumerable<Product>> GetProductByCategoryId(int id);
        Task<bool> AddProductAsync(Product product); 
        Task<bool> UpdateProductAsync(Productdto product, int id);
        Task<bool> DeleteProductAsync(int id);
    }
}
