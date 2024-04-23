using StationeryCompany.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using StationeryCompany.Model;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace StationeryCompany.ViewModel
{
    class ViewModelEditProducts : INotifyPropertyChanged
    {
        public string connectionString; 
        public string _windowTitle;
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


        private string _productName;
        public string ProductName
        {
            get => _productName;
            set
            {
                if (_productName != value)
                {
                    _productName = value;
                    OnPropertyChanged(nameof(ProductName));
                }
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        private int _cost;
        public int Cost
        {
            get => _cost;
            set
            {
                if (_cost != value)
                {
                    _cost = value;
                    OnPropertyChanged(nameof(Cost));
                }
            }
        }
        private int _typeID;
        public int TypeID
        {
            get => _typeID;
            set
            {
                if (_typeID != value)
                {
                    _typeID = value;
                    OnPropertyChanged(nameof(TypeID));
                }
            }
        }

        public string _contentButt;
        public string ContentButt
        {
            get => _contentButt;
            set
            {
                if (_contentButt != value)
                {
                    _contentButt = value;
                    OnPropertyChanged(nameof(ContentButt));
                }
            }
        }

        private ObservableCollection<ProductTypeViewModel> _productType = new ObservableCollection<ProductTypeViewModel>(); 
        public ObservableCollection<ProductTypeViewModel> ProductType
        {
            get => _productType;
            set
            {
                if (_productType != value)
                {
                    _productType = value;
                    OnPropertyChanged(nameof(ProductType));
                }
            }
        }

        private ProductTypeViewModel _selectedProductType;
        public ProductTypeViewModel SelectedProductType
        {
            get => _selectedProductType;
            set
            {
                if (_selectedProductType != value)
                {
                    _selectedProductType = value;
                    OnPropertyChanged(nameof(SelectedProductType));
                }
            }
        }

        public int? ID;
        public ICommand ChangeOrEditCommand { get; set; }

        public string originalProductName = ""; 
        public int originalQuantity;
        public decimal originalCost;
        public int originalTypeID = 0;
        public ViewModelEditProducts(string Title, string Content, int? ID, string connection) 
        {
            connectionString = connection;
            WindowTitle = Title;
            ContentButt = Content;
            this.ID = ID;
            ProductType = new ObservableCollection<ProductTypeViewModel>();
            LoadProductInfoAsync();
            LoadProductTypesAsync();
            ChangeOrEditCommand = new DelegateCommand(
    async (object parameter) =>
    {
        await EditProductAsync(parameter);
    },
    (object parameter) => true);

        }

        private async Task EditProductAsync(object obj)
        {
            if (ProductName != originalProductName || TypeID != originalTypeID ||
                Quantity != originalQuantity || Cost != originalCost)
            {
                var result = MessageBox.Show("Данные продукта были изменены. Вы уверены, что хотите сохранить изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (var connection = new SqlConnection(connectionString))
                        {
                            await connection.OpenAsync();

                            var affectedRows = await connection.ExecuteAsync(
                                @"UPDATE Products
                          SET ProductName = @ProductName,
                              TypeID = @TypeID,
                              Quantity = @Quantity,
                              Cost = @Cost
                          WHERE ProductID = @ProductId",
                                new
                                {
                                    ProductName,
                                    TypeID,
                                    Quantity,
                                    Cost,
                                    ProductId = ID
                                });

                            if (affectedRows > 0)
                            {
                                MessageBox.Show("Информация о продукте успешно обновлена.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                                originalProductName = ProductName;
                                originalTypeID = TypeID;
                                originalQuantity = Quantity;
                                originalCost = Cost;
                            }
                            else
                            {
                                MessageBox.Show("Продукт не найден или ни одна строка не была обновлена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при обновлении информации о продукте: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Изменения отменены.", "Отмена", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Изменений не обнаружено.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }




        public async Task LoadProductTypesAsync()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var productTypes = await connection.QueryAsync<ProductType>("SELECT * FROM ProductTypes");

                    ProductType.Clear();

                    ProductTypeViewModel selectedTypeViewModel = null;
                    foreach (var type in productTypes)
                    {
                        var typeViewModel = new ProductTypeViewModel(type);
                        ProductType.Add(typeViewModel);

                        var products = await connection.QueryAsync<Product>(
                            "SELECT * FROM Products WHERE TypeID = @TypeId",
                            new { TypeId = type.TypeId });

                        if (products.Any(p => p.ProductId == ID))
                        {
                            selectedTypeViewModel = typeViewModel;
                        }
                    }

                    if (selectedTypeViewModel != null)
                    {
                        SelectedProductType = selectedTypeViewModel;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки типов продуктов: {ex.Message}");
            }
        }






        public async Task LoadProductInfoAsync()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var product = await connection.QueryFirstOrDefaultAsync<Product>(
                        @"SELECT p.ProductName,
                         p.Quantity,
                         p.Cost,
                         p.TypeID
                  FROM Products p
                  WHERE p.ProductID = @ProductId",
                        new { ProductId = ID });

                    if (product != null)
                    {
                        ProductName = product.ProductName ?? "Неизвестный продукт";
                        Quantity = product.Quantity ?? 0;
                        Cost = (int)(product.Cost ?? 0);
                        TypeID = product.TypeId ?? 0;
                    }
                    else
                    {
                        ProductName = "Продукт не найден.";
                        Quantity = 0;
                        Cost = 0;
                        TypeID = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных продукта: {ex.Message}");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
