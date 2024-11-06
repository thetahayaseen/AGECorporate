using AGECorporate.Models;

namespace AGECorporate.Repo_Patterns
{
    public interface IQueryRepository
    {
        Task<int> CountAllAsync();
        Task<IEnumerable<Query>> GetAllAsync();
        Task InsertAsync(Query query);
        Task SaveAsync();
    }
}
