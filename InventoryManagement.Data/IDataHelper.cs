namespace InventoryManagement.Data;

public interface IDataHelper<Table>
{
    Task<List<Table>> GetAllAsync();
    Task<List<Table>> SearchAsync(string item);
    Task<Table> FindAsync(Guid id);

    Task AddAsync(Table table);
    Task EditAsync(Table table);
    Task RemoveAsync(Guid Id);
}
