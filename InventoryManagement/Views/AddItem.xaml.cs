using InventoryManagement.WPF.ViewModel;
using System.Windows;

namespace InventoryManagement.WPF.Views;

/// <summary>
/// Interaction logic for AddUser.xaml
/// </summary>
public partial class AddItem : Window
{
    public AddItem(ItemViewModel userView, bool isEditMode = false)
    {
        InitializeComponent();
        DataContext = userView;
        this.Title = isEditMode ? "Edit Item" : "Add Item";
        userView.SetWindowTitle(isEditMode);
        userView.RequestClose += (s, e) => Close();
    }
}
