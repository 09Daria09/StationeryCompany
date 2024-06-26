﻿using StationeryCompany.Commands;
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
using StationeryCompany.Model;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using Dapper;

namespace StationeryCompany.ViewModel
{
    class ViewModelAddProducts : INotifyPropertyChanged
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

        private decimal _cost;
        public decimal Cost
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
        public ICommand ChangeOrEditCommand { get; set; }

        public string originalProductName = "";
        public int originalQuantity;
        public decimal originalCost;
        public int originalTypeID = 0;
        public ViewModelAddProducts(string Title, string Content, string connection)
        {
            WindowTitle = Title;
            ContentButt = Content;
            connectionString = connection;
            LoadProductTypesAsync();
            ChangeOrEditCommand = new DelegateCommand(async (object parameter) =>
            {
                await AddAsync(parameter);
            }, (object parameter) => CanAdd()
);

        }

        private async Task AddAsync(object obj)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new
                    {
                        ProductName = this.ProductName,
                        TypeId = SelectedProductType.TypeId,
                        Quantity = this.Quantity,
                        Cost = this.Cost
                    };

                    var query = @"INSERT INTO Products (ProductName, TypeID, Quantity, Cost)
                          VALUES (@ProductName, @TypeId, @Quantity, @Cost)";

                    await connection.ExecuteAsync(query, parameters);

                    MessageBox.Show("Продукт успешно добавлен.");

                    ProductName = string.Empty;
                    SelectedProductType = null;
                    Quantity = 0;
                    Cost = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении продукта: {ex.Message}");
            }
        }



        private bool CanAdd()
        {
            return !string.IsNullOrWhiteSpace(ProductName) && Quantity > 0 && Cost > 0;
        }

        public async Task LoadProductTypesAsync()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"SELECT pt.*, p.* FROM ProductTypes pt
                          LEFT JOIN Products p ON pt.TypeId = p.TypeId";

                    var productTypesDictionary = new Dictionary<int, ProductType>();

                    await connection.QueryAsync<ProductType, Product, ProductType>(
                        query,
                        (type, product) =>
                        {
                            if (!productTypesDictionary.TryGetValue(type.TypeId, out var productType))
                            {
                                productType = type;
                                productType.Products = new List<Product>();
                                productTypesDictionary.Add(productType.TypeId, productType);
                            }

                            if (product != null)
                                productType.Products.Add(product);

                            return productType;
                        },
                        splitOn: "ProductId");

                    ProductType.Clear();
                    foreach (var productType in productTypesDictionary.Values)
                    {
                        ProductType.Add(new ProductTypeViewModel(productType));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки типов продуктов: {ex.Message}");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
