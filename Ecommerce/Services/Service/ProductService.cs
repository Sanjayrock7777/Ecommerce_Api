using Ecommerce.Dto;
using Ecommerce.Models;
using Ecommerce.Repositories.Interface;
using Ecommerce.Services.Interface;

namespace Ecommerce.Services.Service
{
    public class ProductService: IProductService
    {
        private readonly IProductRepository _repository;
        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }
        public Task<IEnumerable<Product>> GetProductAsync()
        {
            return _repository.GetProductsAsync();
        }
        public Task<bool> AddProductAsync(Productdto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                stock = dto.stock,
                CategoryId = dto.CategoryId
            };
            return _repository.AddProductAsync(product);
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int id)
        {
            return await _repository.GetProductByCategoryId(id);
        }
        public Task<bool> UpdateProductAsync(int id,Productdto product)
        {

            return _repository.UpdateProductAsync(product,id);
        }
        public Task<bool> DeleteProductAsync(int id)
        {
            return _repository.DeleteProductAsync(id);
        }


    }
}
