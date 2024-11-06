using AGECorporate.Models;
using Microsoft.EntityFrameworkCore;

namespace AGECorporate.Repo_Patterns
{
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseContext _databaseContext;

        public ProductRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<int> CountAllAsync()
        {
            return await _databaseContext.Products.CountAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _databaseContext.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _databaseContext.Products.Where(product => product.ProductCategoryId == categoryId).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _databaseContext.Products.FindAsync(id);
        }

        public async Task InsertAsync(Product product)
        {
            await _databaseContext.Products.AddAsync(product);
            await SaveAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _databaseContext.Products.Update(product);
            await SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                _databaseContext.Products.Remove(product);
                await SaveAsync();
            }
        }

        public async Task SaveAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }

    }
}
