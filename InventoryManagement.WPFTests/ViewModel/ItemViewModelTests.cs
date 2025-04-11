using InventoryManagement.Core;
using InventoryManagement.Data;
using InventoryManagement.WPF.ViewModel;
using Moq;
using System.Collections.ObjectModel;

namespace InventoryManagement.Tests.ViewModel;

[TestClass]
public class ItemViewModelTests
{
    private readonly Mock<IDataHelper<Item>> _mockItemHelper;
    private readonly Mock<IDataHelper<Category>> _mockCategoryHelper;
    private readonly ItemViewModel _viewModel;
    public ItemViewModelTests()
    {
        _mockItemHelper = new Mock<IDataHelper<Item>>();
        _mockCategoryHelper = new Mock<IDataHelper<Category>>();
        _viewModel = new ItemViewModel(_mockItemHelper.Object, _mockCategoryHelper.Object);
    }

    [TestMethod]
    public async Task SaveData_ShouldAddNewItem_WhenSelectedItemIsNull()
    {
        // Arrange
        _viewModel.Name = "Test Item";
        _viewModel.Code = "CODE123";
        _viewModel.StockQuantity = 5;
        _viewModel.CategoryId = Guid.NewGuid();
        _viewModel.SelectedItem = null;

        // Act
        _viewModel.SaveData();

        // Assert: Ensure that AddAsync is called
        _mockItemHelper.Verify(m => m.AddAsync(It.IsAny<Item>()), Times.Once);
    }
    [TestMethod]
    public async Task SaveData_ShouldEditItem_WhenSelectedItemIsNotNull()
    {
        // Arrange
        var itemToEdit = new Item { Id = Guid.NewGuid(), Name = "Old Item", Code = "OLDCODE" };
        _viewModel.SelectedItem = itemToEdit;
        _viewModel.Name = "Updated Item";
        _viewModel.Code = "UPDATED123";
        _viewModel.StockQuantity = 10;
        _viewModel.CategoryId = Guid.NewGuid();

        // Act
        _viewModel.SaveData();

        // Assert: Ensure that EditAsync is called
        _mockItemHelper.Verify(m => m.EditAsync(It.IsAny<Item>()), Times.Once);
    }
    // Test for ApplyFilters method
    [TestMethod]
    public void ApplyFilters_ShouldFilterItemsBySearchTerm()
    {
        // Arrange: Setup some test items
        _viewModel.AllItems = new ObservableCollection<Item>
        {
            new Item { Name = "Test Item 1", StockQuantity = 5 },
            new Item { Name = "Another Item", StockQuantity = 3 },
            new Item { Name = "Test Item 2", StockQuantity = 7 }
        };

        _viewModel.SearchTerm = "Test";  // Search for items containing "Test"

        // Act
        _viewModel.ApplyFilters();

        // Assert: Only "Test Item 1" and "Test Item 2" should remain
        Xunit.Assert.Equal(2, _viewModel.Items.Count);
        Xunit.Assert.Contains(_viewModel.Items, item => item.Name == "Test Item 1");
        Xunit.Assert.Contains(_viewModel.Items, item => item.Name == "Test Item 2");
        Xunit.Assert.DoesNotContain(_viewModel.Items, item => item.Name == "Another Item");
    }


    [TestMethod]
    public void ApplyFilters_ShouldFilterItemsByStockStatus()
    {
        // Arrange: Setup test items
        _viewModel.AllItems = new ObservableCollection<Item>
        {
            new Item { Name = "Item 1", StockQuantity = 5 },
            new Item { Name = "Item 2", StockQuantity = 12 }
        };

        _viewModel.StockStatusFilter = "Low Stock";  // Filter for items with less than 10 in stock

        // Act
        _viewModel.ApplyFilters();

        // Assert: Only "Item 1" should remain (StockQuantity < 10)
        Xunit.Assert.Single(_viewModel.Items);
    }

    // Test for CanSave property
    [TestMethod]
    public void CanSave_ShouldReturnFalse_WhenFieldsAreEmpty()
    {
        // Arrange
        _viewModel.Name = string.Empty;
        _viewModel.Code = string.Empty;
        _viewModel.CategoryId = Guid.Empty;

        // Act
        var result = _viewModel.CanSave;

        // Assert
        Xunit.Assert.False(result);
    }

    [TestMethod]
    public void CanSave_ShouldReturnTrue_WhenFieldsAreValid()
    {
        // Arrange
        _viewModel.Name = "Test Item";
        _viewModel.Code = "CODE123";
        _viewModel.CategoryId = Guid.NewGuid();

        // Act
        var result = _viewModel.CanSave;

        // Assert
        Xunit.Assert.True(result);
    }

    // Test for LoadData method (mocking the async call)
    [TestMethod]
    public async Task LoadData_ShouldLoadItemsAndCategories()
    {
        // Arrange
        var itemList = new List<Item>
        {
            new Item { Id = Guid.NewGuid(), Name = "Item 1", StockQuantity = 10 },
            new Item { Id = Guid.NewGuid(), Name = "Item 2", StockQuantity = 5 }
        };

        var categoryList = new List<Category>
        {
            new Category { Id = Guid.NewGuid(), Name = "Category 1" },
            new Category { Id = Guid.NewGuid(), Name = "Category 2" }
        };

        _mockItemHelper.Setup(m => m.GetAllAsync()).ReturnsAsync(itemList);
        _mockCategoryHelper.Setup(m => m.GetAllAsync()).ReturnsAsync(categoryList);

        // Act
        _viewModel.LoadData();

        // Assert: Verify that the items and categories were loaded
        Xunit.Assert.Equal(2, _viewModel.Items.Count);
        Xunit.Assert.Equal(2, _viewModel.Categories.Count);
    }
}
