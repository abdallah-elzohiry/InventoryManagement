using CommunityToolkit.Mvvm.Input;
using InventoryManagement.Core;
using InventoryManagement.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace InventoryManagement.WPF.ViewModel;


public class ItemViewModel : INotifyPropertyChanged
{
    #region Fields

    private readonly IDataHelper<Item> _itemHelper;
    private readonly IDataHelper<Category> _categoryHelper;
    private Item _item = new();
    private Item _selectedItem;
    private ObservableCollection<Item> _items = new();
    private ObservableCollection<Category> _categories = new();
    public ObservableCollection<string> StockStatusOptions { get; set; }

    private string _windowTitle;
    private string _searchTerm;
    private string _stockStatusFilter;


    #endregion

    #region Properties

    public Guid ItemId
    {
        get => _item.Id;
        set
        {
            if (_item.Id != value)
            {
                _item.Id = value;
                OnPropertyChanged(nameof(ItemId));
            }
        }
    }
    public Guid CategoryId
    {
        get => _item.CategoryId;
        set
        {
            if (_item.CategoryId != value)
            {
                _item.CategoryId = value;
                OnPropertyChanged(nameof(CategoryId));
                OnPropertyChanged(nameof(CanSave));
            }
        }
    }
    public string CategoryName
    {
        get => _item.Category?.Name;
        set
        {
            if (_item.Category?.Name != value)
            {
                _item.Category.Name = value;
                OnPropertyChanged(nameof(CategoryName));
            }
        }
    }
    public string Code
    {
        get => _item.Code;
        set
        {
            if (_item.Code != value)
            {
                _item.Code = value;
                OnPropertyChanged(nameof(Code));
                OnPropertyChanged(nameof(CanSave));
            }
        }
    }
    public string Name
    {
        get => _item.Name;
        set
        {
            if (_item.Name != value)
            {
                _item.Name = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(CanSave));
            }
        }
    }
    public decimal StockQuantity
    {
        get => _item.StockQuantity;
        set
        {
            if (_item.StockQuantity != value)
            {
                _item.StockQuantity = value;
                OnPropertyChanged(nameof(StockQuantity));
                OnPropertyChanged(nameof(CanSave));
            }
        }
    }
    public DateTime CreateAt
    {
        get => _item.CreateAt;
        set
        {
            if (_item.CreateAt != value)
            {
                _item.CreateAt = value;
                OnPropertyChanged(nameof(CreateAt));
            }
        }
    }
    public DateTime UpdateAt
    {
        get => _item.UpdateAt;
        set
        {
            if (_item.UpdateAt != value)
            {
                _item.UpdateAt = value;
                OnPropertyChanged(nameof(UpdateAt));
            }
        }
    }
    public Item SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (_selectedItem != value)
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
    }
    // Property for search term
    public string SearchTerm
    {
        get => _searchTerm;
        set
        {
            if (_searchTerm != value)
            {
                _searchTerm = value;
                OnPropertyChanged(nameof(SearchTerm));
                ApplyFilters();
            }
        }
    }

    // Property for stock status filter
    public string StockStatusFilter
    {
        get => _stockStatusFilter;
        set
        {
            if (_stockStatusFilter != value)
            {
                _stockStatusFilter = value;
                OnPropertyChanged(nameof(StockStatusFilter));
                ApplyFilters();
            }
        }
    }
    private ObservableCollection<Item> _allItems;
    private IDataHelper<Item> object1;
    private IDataHelper<Category> object2;

    public ObservableCollection<Item> AllItems
    {
        get => _allItems;
        set
        {
            _allItems = value;
            OnPropertyChanged(nameof(AllItems));
        }
    }

    public ObservableCollection<Item> Items
    {
        get => _items;
        set
        {
            if (_items != value)
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
    }

    public ObservableCollection<Category> Categories
    {
        get => _categories;
        set
        {
            if (_categories != value)
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }
    }
    public string WindowTitle
    {
        get => _windowTitle;
        set
        {
            if (_windowTitle != value)
            {
                _windowTitle = value;
                OnPropertyChanged(nameof(WindowTitle));
            }
        }
    }
    public bool CanSave
    {
        get
        {
            return !string.IsNullOrEmpty(Name) &&
                   !string.IsNullOrEmpty(Code) &&
                   //!string.IsNullOrEmpty(StockQuantity) &&
                   CategoryId != Guid.Empty; // Ensure a category is selected
        }
    }

    #endregion

    #region Commands

    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand SaveCommand { get; }

    #endregion

    #region Constructor

    public ItemViewModel(IDataHelper<Item> itemHelper, IDataHelper<Category> categoryHelper)

    {
        _itemHelper = itemHelper;
        _categoryHelper = categoryHelper;

        AddCommand = new RelayCommand(AddData);
        EditCommand = new RelayCommand(EditData);
        SaveCommand = new RelayCommand(SaveData);
        DeleteCommand = new RelayCommand(DeleteData);
        // Initialize stock status options
        StockStatusOptions = new ObservableCollection<string>
        {
            "All", // Option for showing all items
            "Low Stock",
            "In Stock"
        };
        LoadData();
    }



    #endregion

    #region Methods
    public void ApplyFilters()
    {
        if (AllItems == null) return;

        IEnumerable<Item> result = AllItems;

        // Apply search
        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            result = result.Where(i =>
                i.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        // Apply stock filter
        if (!string.IsNullOrWhiteSpace(StockStatusFilter) && StockStatusFilter != "All")
        {
            result = result.Where(i =>
                StockStatusFilter == "Low Stock" ? i.StockQuantity < 10 :
                StockStatusFilter == "In Stock" ? i.StockQuantity >= 10 : true);
        }

        Items = new ObservableCollection<Item>(result);
    }



    public async void SaveData()
    {
        var newItem = new Item
        {
            Name = Name,
            Code = Code,
            StockQuantity = StockQuantity,
            CategoryId = CategoryId,
            CreateAt = SelectedItem?.CreateAt ?? DateTime.Now,
            UpdateAt = SelectedItem?.UpdateAt ?? DateTime.Now,
            Id = SelectedItem?.Id ?? Guid.Empty
        };

        if (SelectedItem != null)
        {
            await _itemHelper.EditAsync(newItem);
        }
        else
        {
            await _itemHelper.AddAsync(newItem);
        }
        LoadData();
        OnRequestClose();
    }

    private void AddData()
    {
        ClearForm();
        new Views.AddItem(this, false).Show();
    }

    private void EditData()
    {

        if (SelectedItem != null)
        {
            FillFormFromSelectedUser();
            new Views.AddItem(this, true).Show();
        }
        else
        {
            MessageBox.Show("Please select an item to edit.");
        }
    }

    private async void DeleteData()
    {
        if (SelectedItem != null)
        {
            // Show confirmation dialog before deleting
            var result = MessageBox.Show(
                "Are you sure you want to delete this item?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                await _itemHelper.RemoveAsync(SelectedItem.Id);
                LoadData();
            }
        }
        else
        {
            MessageBox.Show("Select item to Delete");
        }
    }


    public async void LoadData()
    {
        //var data = new ObservableCollection<Item>(await _itemHelper.GetAllAsync());
        var categ = new ObservableCollection<Category>(await _categoryHelper.GetAllAsync());
        // Items = data;
        Categories = categ;

        var itemsFromDataSource = await _itemHelper.GetAllAsync();
        AllItems = new ObservableCollection<Item>(itemsFromDataSource);
        Items = new ObservableCollection<Item>(itemsFromDataSource);
    }

    private void FillFormFromSelectedUser()
    {
        if (SelectedItem == null) return;

        Code = SelectedItem.Name;
        Name = SelectedItem.Code;
        StockQuantity = SelectedItem.StockQuantity;
        CategoryId = SelectedItem.CategoryId;
    }

    private void ClearForm()
    {
        Name = string.Empty;
        Code = string.Empty;
        StockQuantity = 1;
        CategoryId = Guid.Empty;
    }
    public void SetWindowTitle(bool isEditMode)
    {
        WindowTitle = isEditMode ? "Edit Item" : "Add New Item";
    }
    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler? RequestClose;

    private void OnRequestClose() => RequestClose?.Invoke(this, EventArgs.Empty);

    protected virtual void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    #endregion
}
