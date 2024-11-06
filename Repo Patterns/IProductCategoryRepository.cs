using AGECorporate.Models;

namespace AGECorporate.Repo_Patterns
{
    public interface IProductCategoryRepository
    {
        Task<int> CountAllAsync();
        Task<IEnumerable<ProductCategory>> GetAllAsync();
        Task<ProductCategory> GetByIdAsync(int id);
        Task InsertAsync(ProductCategory productCategory);
        Task UpdateAsync(ProductCategory productCategory);
        Task DeleteAsync(int id);
        Task SaveAsync();

    }
}
