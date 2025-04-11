using InventoryManagement.WPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace InventoryManagement.WPF.Views;
public partial class ItemView : UserControl
{
    private ItemViewModel itemView;
    public ItemView()
    {
        InitializeComponent();
        //itemView = new ViewModel.ItemViewModel();
        itemView = App.ServiceProvider.GetRequiredService<ItemViewModel>();
        DataContext = itemView;

    }
}
