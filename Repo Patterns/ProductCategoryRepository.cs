using AGECorporate.Models;
using Microsoft.EntityFrameworkCore;

namespace AGECorporate.Repo_Patterns
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly DatabaseContext _databaseContext;

        public ProductCategoryRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<int> CountAllAsync()
        {
            return await _databaseContext.ProductCategories.CountAsync();
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            return await _databaseContext.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory?> GetByIdAsync(int id)
        {
            return await _databaseContext.ProductCategories.FindAsync(id);
        }

        public async Task InsertAsync(ProductCategory productCategory)
        {
            await _databaseContext.ProductCategories.AddAsync(productCategory);
            await SaveAsync();
        }

        public async Task UpdateAsync(ProductCategory productCategory)
        {
            _databaseContext.ProductCategories.Update(productCategory);
            await SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var productCategory = await GetByIdAsync(id);
            if (productCategory != null)
            {
                _databaseContext.ProductCategories.Remove(productCategory);
                await SaveAsync();
            }
        }

        public async Task SaveAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
