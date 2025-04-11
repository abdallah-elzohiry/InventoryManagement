using InventoryManagement.Core;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Data.Operations;

public class ItemEntity : IDataHelper<Item>
{
    private readonly IDbContextFactory<DBContext> _dbFactory;


    public ItemEntity(IDbContextFactory<DBContext> dbFactory)
    {
        _dbFactory = dbFactory;

    }

    public async Task AddAsync(Item table)
    {
        using var db = _dbFactory.CreateDbContext();

        await db.Items.AddAsync(table);
        await db.SaveChangesAsync();
    }

    public async Task EditAsync(Item table)
    {
        using var db = _dbFactory.CreateDbContext();

        db.Items.Update(table);
        await db.SaveChangesAsync();
    }

    public async Task<Item> FindAsync(Guid id)
    {
        using var db = _dbFactory.CreateDbContext();

        return await db.Items.FindAsync(id);
    }

    public async Task<List<Item>> GetAllAsync()
    {
        using var db = _dbFactory.CreateDbContext();

        var data = await db.Items.Include(z => z.Category).ToListAsync();
        return data;
    }

    public async Task RemoveAsync(Guid Id)
    {
        using var db = _dbFactory.CreateDbContext();

        var item = await FindAsync(Id);
        db.Items.Remove(item);
        await db.SaveChangesAsync();
    }

    public async Task<List<Item>> SearchAsync(string item)
    {
        using var db = _dbFactory.CreateDbContext();

        return await db.Items.Where(x => x.Name.Contains(item) || x.Code.Contains(item)).ToListAsync();
    }
}
