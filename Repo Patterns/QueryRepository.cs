using AGECorporate.Models;
using AGECorporate.Repo_Patterns;
using Microsoft.EntityFrameworkCore;

public class QueryRepository : IQueryRepository
{
    private readonly DatabaseContext _databaseContext;

    public QueryRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<int> CountAllAsync()
    {
        return await _databaseContext.Queries.CountAsync();
    }

    public async Task<IEnumerable<Query>> GetAllAsync()
    {
        return await _databaseContext.Queries
            .OrderBy(query => query.Reply == null)
            .ToListAsync();
    }

    public async Task InsertAsync(Query query)
    {
        await _databaseContext.Queries.AddAsync(query);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _databaseContext.SaveChangesAsync();
    }
}
