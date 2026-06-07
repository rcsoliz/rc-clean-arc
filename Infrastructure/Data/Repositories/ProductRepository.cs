using Application.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            var products = await _context.Products.
                Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price
                }).ToListAsync(cancellationToken);

            return products; 
        }

        public async Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { id }, cancellationToken);

            return new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
        }
        public async Task AddAsync(Product entity, CancellationToken cancellationToken)
        {
            await _context.Products.AddAsync(entity,cancellationToken);
            await _context.SaveChangesAsync();  
        }

        public async Task UpdateAsync(Product entity, CancellationToken cancellationToken)
        {
            var existingProduct = await _context.Products.FindAsync(new object[] { entity.Id }, cancellationToken);
            if(existingProduct != null) 
            {
                _context.Products.Update(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(id,cancellationToken);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
