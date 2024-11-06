using AGECorporate.Models;

namespace AGECorporate.Repo_Patterns
{
    public interface IProductRepository
    {
        Task<int> CountAllAsync();
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<Product> GetByIdAsync(int id);
        Task InsertAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task SaveAsync();

    }
}
