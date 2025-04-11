using InventoryManagement.Core;

namespace InventoryManagement.Data
{
    public static class DbInitializer
    {
        public static void SeedCategories(DBContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
            {
                new Category
                {
                    Id = Guid.NewGuid(),
                    Code = "01",
                    Name = "Category One",
                    CreateAt = DateTime.UtcNow,
                    UpdateAt = DateTime.UtcNow
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Code = "02",
                    Name = "Category Two",
                    CreateAt = DateTime.UtcNow,
                    UpdateAt = DateTime.UtcNow
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Code = "03",
                    Name = "Category Three",
                    CreateAt = DateTime.UtcNow,
                    UpdateAt = DateTime.UtcNow
                }
            };

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
        }
    }

}
