namespace InventoryManagement.Core;

public class Item
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal StockQuantity { get; set; }
    public Guid CategoryId { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }

    public virtual Category Category { get; set; }


}
