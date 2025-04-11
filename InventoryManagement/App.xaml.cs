using InventoryManagement.Core;
using InventoryManagement.Data;
using InventoryManagement.Data.Operations;
using InventoryManagement.WPF.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;

namespace InventoryManagement.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; }
    public IConfiguration Configuration { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        Configuration = builder.Build();

        var services = new ServiceCollection();

        // Register DbContext with SQL Server
        services.AddDbContext<DBContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        // Register IDbContextFactory<DBContext> for services needing a factory
        services.AddDbContextFactory<DBContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        // Register data helpers
        services.AddScoped<IDataHelper<Item>, ItemEntity>();
        services.AddScoped<IDataHelper<Category>, CategoryEntity>();

        // Register the ViewModel
        services.AddTransient<ItemViewModel>();

        ServiceProvider = services.BuildServiceProvider();

        // Run migrations and seed data
        using (var scope = ServiceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DBContext>();
            context.Database.Migrate(); // Applies pending migrations

            // Seed categories
            DbInitializer.SeedCategories(context);
        }

        // Resolve and show main window
        var viewModel = ServiceProvider.GetRequiredService<ItemViewModel>();
        //var mainWindow = new MainWindow
        //{
        //    DataContext = viewModel
        //};
    }


}

