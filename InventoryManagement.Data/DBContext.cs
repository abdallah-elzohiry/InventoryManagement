using InventoryManagement.Core;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Data;

public class DBContext : DbContext
{


    public DBContext(DbContextOptions options) : base(options)
    {
    }



    public DbSet<Item> Items { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>()
            .HasKey(i => i.Id);

        modelBuilder.Entity<Item>()
            .HasOne(i => i.Category)
            .WithMany() // or .WithMany(c => c.Items) if Category has a collection
            .HasForeignKey(i => i.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        base.OnModelCreating(modelBuilder);
    }

}
