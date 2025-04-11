using System.Windows;

namespace InventoryManagement.WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DPanelContainer.Children.Add(new Views.ItemView());
    }
}