using Ecommerce.Data;
using Ecommerce.Dto;
using Ecommerce.Models;
using Ecommerce.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repositories.Repository
{
    public class ProductRepository: IProductRepository
    {
        private readonly EcomDbContext _context;
        public ProductRepository(EcomDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductByCategoryId(int id)
        {
            return await _context.Products.Where(p =>  p.CategoryId == id).ToListAsync();
        }
        public async Task<bool> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            return await _context.SaveChangesAsync() >0;
        }
        public async Task<bool> UpdateProductAsync(Productdto dto, int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.stock = dto.stock;
            product.CategoryId = dto.CategoryId;

           // _context.Entry(product).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);  
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
