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
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _repository.GetProductByIdAsync(id);
        }
        public async Task<bool> AddProductAsync(Productdto dto)
        {
            string imagepath=null;
            if(dto.ImageFile != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(dto.ImageFile.FileName);
                var path = Path.Combine("wwwroot/images", fileName);
                using(var stream = new FileStream(path, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }
                imagepath = "/images" + fileName;
            }
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                stock = dto.stock,
                Brand = dto.Brand,
                CategoryId = dto.CategoryId,
                ImageUrl = imagepath
            };
            return await _repository.AddProductAsync(product);
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int id)
        {
            return await _repository.GetProductByCategoryId(id);
        }
        public Task<bool> UpdateProductAsync(int id,Productdto product)
        {

            return _repository.UpdateProductAsync(product,id);
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _repository.DeleteProductAsync(id);
        }


    }
}
