using InventoryManagement.Core;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Data.Operations;

public class CategoryEntity : IDataHelper<Category>
{
    private readonly IDbContextFactory<DBContext> _dbFactory;

    public CategoryEntity(IDbContextFactory<DBContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public Task AddAsync(Category table)
    {
        throw new NotImplementedException();
    }

    public Task EditAsync(Category table)
    {
        throw new NotImplementedException();
    }

    public Task<Category> FindAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Category>> GetAllAsync()
    {
        using var db = _dbFactory.CreateDbContext();
        var data = await db.Categories.ToListAsync();
        return data;
    }

    public Task RemoveAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Category>> SearchAsync(string item)
    {
        throw new NotImplementedException();
    }
}
