using StationeryCompany.Commands;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Microsoft.VisualBasic;
using StationeryCompany.View;
using System.Windows.Forms;
using StationeryCompany.Model;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using System.Configuration;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.IO;
namespace StationeryCompany.ViewModel
{
    internal class ViewModelStationery : INotifyPropertyChanged
    {
        public string connection; 
        private DataTable _productsData;
        public DataTable ProductsData
        {
            get { return _productsData; }
            set
            {
                if (_productsData != value)
                {
                    _productsData = value;
                    OnPropertyChanged(nameof(ProductsData));
                }
            }
        }
        private ObservableCollection<ProductViewModel> _products = new ObservableCollection<ProductViewModel>();

        public ObservableCollection<ProductViewModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public ObservableCollection<ProductTypeViewModel> ProductTypes { get; set; } = new ObservableCollection<ProductTypeViewModel>();

        private void LoadProductTypes()
        {
            try
            {
                using (var connections = new System.Data.SqlClient.SqlConnection(connection))
                {
                    connections.Open();

                    var query = "SELECT * FROM ProductTypes";

                    var types = connections.Query<ProductType>(query).ToList();
                    foreach (var type in types)
                    {
                        ProductTypes.Add(new ProductTypeViewModel(type));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public ObservableCollection<MenuItem> MenuItems { get; set; }
        public ObservableCollection<MenuItem> MenuItems1 { get; set; }
        public ObservableCollection<MenuItem> MenuItems2 { get; set; }
        public ICommand ShowAllProductsCommand { get; private set; }
        public ICommand ShowAllProductTypesCommand { get; private set; }
        public ICommand ShowAllSalesManagersCommand { get; private set; }
        public ICommand ShowProductsWithMaxQuantityCommand { get; private set; }
        public ICommand ShowProductsWithMinQuantityCommand { get; private set; }
        public ICommand ShowProductsWithMinCostCommand { get; private set; }
        public ICommand ShowProductsWithMaxCostCommand { get; private set; }
        public ICommand ShowProductsByTypeCommand { get; private set; }
        public ICommand ShowProductsSoldByManagerCommand { get; private set; }
        public ICommand ShowProductsBoughtByCompanyCommand { get; private set; }
        public ICommand ShowLatestSaleInfoCommand { get; private set; }
        public ICommand ShowAverageQuantityByTypeCommand { get; private set; }
        public ICommand ShowTopSellingManagerByUnitsCommand { get; private set; }
        public ICommand ShowTopSellingManagerByProfitCommand { get; private set; }
        public ICommand ShowTopCustomerCompanyBySpendingCommand { get; private set; }
        public ICommand ShowTopProductTypeByUnitsSoldCommand { get; private set; }
        public ICommand ShowTopProfitableProductTypeCommand { get; private set; }
        public ICommand ShowMostPopularProductsCommand { get; private set; }
        public ICommand ShowTopSellingManagerByProfitInPeriodCommand { get; private set; }
        public ICommand ShowUnsoldProductsCommand { get; private set; }
        public ICommand ShowAllCompaniesCommand { get; private set; }

        public ICommand ShowAllSalesCommand { get; private set; }

        public ICommand DeleteCommand { get; }

        public ICommand EditCommand { get; }

        public ICommand AddManagerCommand { get; private set; }
        public ICommand AddCompanyCommand { get; private set; }
        public ICommand AddProductsCommand { get; private set; }
        public ICommand AddProductCommand { get; private set; }

        private bool _isShowAllProductsCommandExecuted;
        public bool IsShowAllProductsCommandExecuted
        {
            get { return _isShowAllProductsCommandExecuted; }
            set
            {
                _isShowAllProductsCommandExecuted = value;
                OnPropertyChanged(nameof(IsShowAllProductsCommandExecuted));
            }
        }

        public ViewModelStationery()
        {
            var builder = new ConfigurationBuilder();
            string path = Directory.GetCurrentDirectory();
            builder.SetBasePath(path);
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            connection = config.GetConnectionString("DefaultConnection");

            LoadProductTypes();
            MenuItems = new ObservableCollection<MenuItem>();
            InitializeMenuItems();
            MenuItems1 = new ObservableCollection<MenuItem>();
            InitializeMenuItems1();
            MenuItems2 = new ObservableCollection<MenuItem>();
            InitializeMenuItems2();

            ShowAllProductsCommand = new DelegateCommand(ShowAllProducts, (object parameter) => true);
            ShowAllProductTypesCommand = new DelegateCommand(ShowAllProductTypes, (object parameter) => true);
            ShowAllSalesManagersCommand = new DelegateCommand(ShowAllSalesManagers, (object parameter) => true);
            ShowAllCompaniesCommand = new DelegateCommand(ShowAllCompanies, (object parameter) => true);

            ShowProductsWithMaxQuantityCommand = new DelegateCommand(ShowProductsWithMaxQuantity, (object parameter) => true);
            ShowProductsWithMinQuantityCommand = new DelegateCommand(ShowProductsWithMinQuantity, (object parameter) => true);
            ShowProductsWithMinCostCommand = new DelegateCommand(ShowProductsWithMinCost, (object parameter) => true);
            ShowProductsWithMaxCostCommand = new DelegateCommand(ShowProductsWithMaxCost, (object parameter) => true);
            ShowProductsByTypeCommand = new DelegateCommand(ShowProductsByType, (object parameter) => true);
            ShowProductsSoldByManagerCommand = new DelegateCommand(ShowProductsSoldByManager, (object parameter) => true);
            ShowProductsBoughtByCompanyCommand = new DelegateCommand(ShowProductsBoughtByCompanyAsync, (object parameter) => true);
            ShowLatestSaleInfoCommand = new DelegateCommand(ShowLatestSaleInfoAsync, (object parameter) => true);

            DeleteCommand = new DelegateCommand(DeleteAsync, (object parameter) => true);
            EditCommand = new DelegateCommand(Edit, (object parameter) => true);

            AddManagerCommand = new DelegateCommand(AddManager, (object parameter) => true);
            AddCompanyCommand = new DelegateCommand(AddCompany, (object parameter) => true);
            AddProductsCommand = new DelegateCommand(AddProducts, (object parameter) => true);

            AddProductCommand = new DelegateCommand(AddProduct, (object parameter) => true);

        }

        private void AddProduct(object obj)
        {
            EditProducts edit = new EditProducts("Добавление продукта", "Добавить", connection);
            edit.ShowDialog();
            ShowAllProducts("ShowAllProducts");
        }

        private void AddProducts(object obj)
        {
            EditOrAddProductType edit = new EditOrAddProductType("Добавление типа продукта", "Добавить", connection);
            edit.ShowDialog();
            ShowAllProductTypes("ShowAllProductTypes");
            InitializeMenuItems();

        }

        private void AddCompany(object obj)
        {
            EditCompanies editManager = new EditCompanies("Добавление компании", "Добавить", connection);
            editManager.ShowDialog();
            ShowAllCompanies("ShowAllCompaniesWithOrderCount");
            InitializeMenuItems2();
        }

        private void AddManager(object obj)
        {
            EditManagers editManager = new EditManagers("Добавление менеджера", "Добавить", connection);
            editManager.ShowDialog();
            ShowAllSalesManagers("ShowAllSalesManagers");
            InitializeMenuItems1();
        }

        private void Edit(object obj)
        {
            if (obj is DataRowView dataRowView)
            {
                DataRow selectedRow = dataRowView.Row;
                int entityID = (int)selectedRow.ItemArray[0];

                if (obj is DataRowView table)
                {
                    if (table.DataView.Table.TableName == "Products")
                    {
                        EditProducts edit = new EditProducts("Изменение продукта", "Изменить", entityID, connection);
                        edit.ShowDialog();
                        ShowAllProducts("ShowAllProducts");
                    }
                    if (table.DataView.Table.TableName == "ProductTypes")
                    {
                        EditOrAddProductType edit = new EditOrAddProductType("Изменение типа продукта", "Изменить", entityID, connection);
                        edit.ShowDialog();
                        ShowAllProductTypes("ShowAllProductTypes");
                        InitializeMenuItems();
                    }
                    if (table.DataView.Table.TableName == "SalesManagers")
                    {
                        EditManagers editManager = new EditManagers("Изменение менеджера", "Изменить", entityID, connection);
                        editManager.ShowDialog();
                        ShowAllSalesManagers("ShowAllSalesManagers");
                        InitializeMenuItems1();
                    }
                    if (table.DataView.Table.TableName == "CompaniesWithOrderCount")
                    {
                        EditCompanies editManager = new EditCompanies("Изменение компании", "Изменить", entityID, connection);
                        editManager.ShowDialog();
                        ShowAllCompanies("ShowAllCompaniesWithOrderCount");
                        InitializeMenuItems2();
                    }
                }
            }

        }


        private async void DeleteAsync(object obj)
        {
            var result = System.Windows.MessageBox.Show("Вы уверены, что хотите удалить этот компонент?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (obj is DataRowView rowView)
                {
                    string storedProcedureName = null;
                    string parameterName = null;
                    object parameterValue = null;

                    switch (rowView.Row.Table.TableName)
                    {
                        case "Products":
                            storedProcedureName = "DeleteProduct";
                            parameterName = "@ProductID";
                            parameterValue = rowView[0];
                            break;
                        case "ProductTypes":
                            storedProcedureName = "DeleteProductTypeAndRelatedProducts";
                            parameterName = "@TypeID";
                            parameterValue = rowView[0];
                            break;
                        case "SalesManagers":
                            storedProcedureName = "DeleteSalesManagerAndRelatedSales";
                            parameterName = "@ManagerID";
                            parameterValue = rowView[0];
                            break;
                        case "CompaniesWithOrderCount":
                            storedProcedureName = "DeleteSalesCompaniesAndRelatedSales";
                            parameterName = "@CompaniesID";
                            parameterValue = rowView[0];
                            break;
                    }

                    if (storedProcedureName != null)
                    {

                        try
                        {
                            using (var connections = new System.Data.SqlClient.SqlConnection(connection))
                            {
                                await connections.OpenAsync();

                                var parameters = new DynamicParameters();
                                parameters.Add(parameterName, parameterValue);

                                await connections.ExecuteAsync(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);
                            }

                            RefreshUI(rowView.Row.Table.TableName);
                        }
                        catch (Exception ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }


        private void RefreshUI(string tableName)
        {
            switch (tableName)
            {
                case "Products":
                    ShowAllProducts("ShowAllProducts");
                    break;
                case "ProductTypes":
                    ShowAllProductTypes("ShowAllProductTypes");
                    InitializeMenuItems();
                    break;
                case "SalesManagers":
                    ShowAllSalesManagers("ShowAllSalesManagers");
                    InitializeMenuItems1();
                    break;
                case "CompaniesWithOrderCount":
                    ShowAllCompanies("ShowAllCompaniesWithOrderCount");
                    InitializeMenuItems2();
                    break;
            }
        }

        private void ShowAllCompanies(object obj)
        {
            try
            {
                using (var connections = new System.Data.SqlClient.SqlConnection(connection))
                {
                    connections.Open();
                    var companies = connections.Query<CustomerCompany>("EXEC ShowAllCompaniesWithOrderCount").ToList();

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Id", typeof(int));
                    dataTable.Columns.Add("Название Компании", typeof(string));
                    dataTable.Columns.Add("Номер Телефона", typeof(string));
                    dataTable.Columns.Add("Город", typeof(string));
                    dataTable.Columns.Add("Количество Заказов", typeof(int));

                    foreach (var company in companies)
                    {
                        dataTable.Rows.Add(company.CompanyId, company.CompanyName, company.PhoneNumber, company.City, company.Sales.Count);
                    }

                    ProductsData = dataTable;
                    ProductsData.TableName = "CompaniesWithOrderCount";
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            IsShowAllProductsCommandExecuted = true;
        }


        private async void ShowLatestSaleInfoAsync(object obj)
        {
            try
            {
                using (var connections = new System.Data.SqlClient.SqlConnection(connection))
                {
                    await connections.OpenAsync();
                    var salesInfo = await connections.QueryAsync<Sale>(
                        "EXEC ShowLatestSale");

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Название продукта", typeof(string));
                    dataTable.Columns.Add("Имя менеджера", typeof(string));
                    dataTable.Columns.Add("Название компании", typeof(string));
                    dataTable.Columns.Add("Количество проданного", typeof(int));
                    dataTable.Columns.Add("Цена за единицу", typeof(decimal));

                    foreach (var info in salesInfo)
                    {
                        var productName = await connections.QueryFirstOrDefaultAsync<string>(
                            "SELECT ProductName FROM Products WHERE ProductID = @ProductId",
                            new { ProductId = info.ProductId });
                        productName = productName ?? "Неизвестный продукт";

                        var managerName = await connections.QueryFirstOrDefaultAsync<string>(
                        "SELECT ManagerName FROM SalesManagers WHERE ManagerID = @ManagerId",
                         new { ManagerId = info.ManagerId });
                        managerName = managerName ?? "Неизвестный менеджер";

                        var companyName = await connections.QueryFirstOrDefaultAsync<string>(
                            "SELECT CompanyName FROM CustomerCompanies WHERE CompanyID = @CompanyID",
                            new { CompanyId = info.CompanyId }); 
                        companyName = companyName ?? "Неизвестная компания";

                        dataTable.Rows.Add(
                            productName,
                            managerName,
                            companyName,
                            info.QuantitySold,
                            info.PricePerUnit
                        );
                    }
                    ProductsData = dataTable;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            IsShowAllProductsCommandExecuted = false;
        }





        private async void ShowProductsBoughtByCompanyAsync(object companyNameObj)
        {
            try
            {
                var companyName = companyNameObj as string;
                if (companyName == null) throw new ArgumentNullException(nameof(companyNameObj), "Company name must not be null.");

                using (var connections = new System.Data.SqlClient.SqlConnection(connection))
                {
                    await connections.OpenAsync();
                    var products = await connections.QueryAsync<Product>(
                        "SELECT * FROM ProductsBoughtByCompany(@CompanyName)",
                        new { CompanyName = companyName });

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Id", typeof(int));
                    dataTable.Columns.Add("Тип Продукта", typeof(string));
                    dataTable.Columns.Add("Количество проданного", typeof(int));
                    dataTable.Columns.Add("Общая сумма продаж", typeof(decimal));

                    foreach (var product in products)
                    {

                        dataTable.Rows.Add(product.ProductId, product.ProductName, product.Quantity, product.Cost);
                    }

                    ProductsData = dataTable;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }






        private async void ShowProductsSoldByManager(object obj)
        {
            try
            {
                var managerName = obj as string;
                if (managerName == null) return;

                using (var connections = new System.Data.SqlClient.SqlConnection(connection))
                {
                    await connections.OpenAsync();
                    var parameters = new { ManagerName = managerName };
                    var sales = await connections.QueryAsync<Sale>(
                        "EXEC ShowProductsSoldByManager @ManagerName", parameters);

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Тип Продукта", typeof(string));
                    dataTable.Columns.Add("Менеджер", typeof(string));
                    dataTable.Columns.Add("Количество проданного", typeof(int));
                    dataTable.Columns.Add("Цена за единицу", typeof(decimal));
                    dataTable.Columns.Add("Общая сумма продаж", typeof(decimal));

                    foreach (var sale in sales)
                    {
                        var typeName = await connections.QueryFirstOrDefaultAsync<string>(
                            "SELECT TypeName FROM ProductTypes WHERE TypeID = (SELECT TypeID FROM Products WHERE ProductID = @ProductId)",
                            new { ProductId = sale.ProductId });

                        typeName = typeName ?? "Unknown";

                        dataTable.Rows.Add(
                            typeName,
                            managerName,
                            sale.QuantitySold,
                            sale.PricePerUnit,
                            sale.QuantitySold * sale.PricePerUnit
                        );
                    }
                    ProductsData = dataTable;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }



        private async void ShowProductsByType(object obj)
        {
            try
            {
                var typeName = obj as string;
                if (typeName == null) return;

                using (var connections = new System.Data.SqlClient.SqlConnection(connection))
                {
                    await connections.OpenAsync();
                    var parameters = new { TypeName = typeName };
                    var products = await connections.QueryAsync<Product>("EXEC ShowProductsByType @TypeName", parameters);
                    var productTypes = connections.Query<ProductType>("SELECT * FROM ProductTypes").ToDictionary(pt => pt.TypeId, pt => pt);

                    foreach (var product in products)
                    {
                        if (product.TypeId.HasValue && productTypes.ContainsKey(product.TypeId.Value))
                        {
                            product.Type = productTypes[product.TypeId.Value];
                        }
                        else
                        {
                            product.Type = new ProductType { TypeName = "Unknown" };
                        }
                    }

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("ID Продукта", typeof(int));
                    dataTable.Columns.Add("Название", typeof(string));
                    dataTable.Columns.Add("Тип", typeof(string));
                    dataTable.Columns.Add("Количество", typeof(int));
                    dataTable.Columns.Add("Стоимость", typeof(decimal));

                    foreach (var product in products)
                    {
                        dataTable.Rows.Add(
                            product.ProductId,
                            product.ProductName,
                            typeName,
                            product.Quantity ?? 0,
                            product.Cost ?? 0.0m
                        );
                    }
                    ProductsData = dataTable;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            IsShowAllProductsCommandExecuted = false;
        }



        private void ShowProductsWithMaxCost(object obj)
        {
            try
            {
                using (var connections = new System.Data.SqlClient.SqlConnection(connection))
                {
                    connections.Open();
                    var products = connections.Query<Product>("EXEC ShowProductsWithMaxCost").ToList();
                    var productTypes = connections.Query<ProductType>("SELECT * FROM ProductTypes").ToDictionary(pt => pt.TypeId, pt => pt);

                    foreach (var product in products)
                    {
                        if (product.TypeId.HasValue && productTypes.ContainsKey(product.TypeId.Value))
                        {
                            product.Type = productTypes[product.TypeId.Value];
                        }
                        else
                        {
                            product.Type = new ProductType { TypeName = "Unknown" }; 
                        }
                    }

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("ID Продукта", typeof(int));
                    dataTable.Columns.Add("Название Продукта", typeof(string));
                    dataTable.Columns.Add("Тип Продукта", typeof(string));
                    dataTable.Columns.Add("Количество", typeof(int));
                    dataTable.Columns.Add("Цена за единицу", typeof(decimal));

                    foreach (var product in products)
                    {
                        var typeName = product.Type?.TypeName ?? "Unknown";

                        dataTable.Rows.Add(
                            product.ProductId,
                            product.ProductName,
                            typeName,
                            product.Quantity ?? 0,
                            product.Cost ?? 0.0m
                        );
                    }

                    ProductsData = dataTable;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            IsShowAllProductsCommandExecuted = true;
        }



        private void ShowProductsWithMinCost(object obj)
        {
            try
            {
                using (var connections = new System.Data.SqlClient.SqlConnection(connection))
                {
                    connections.Open();
                    var products = connections.Query<Product>("EXEC ShowProductsWithMinCost").ToList();
                    var productTypes = connections.Query<ProductType>("SELECT * FROM ProductTypes").ToDictionary(pt => pt.TypeId, pt => pt);

                    foreach (var product in products)
                    {
                        if (product.TypeId.HasValue && productTypes.ContainsKey(product.TypeId.Value))
                        {
                            product.Type = productTypes[product.TypeId.Value];
                        }
                        else
                        {
                            product.Type = new ProductType { TypeName = "Unknown" };
                        }
                    }

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("ID Продукта", typeof(int));
                    dataTable.Columns.Add("Название Продукта", typeof(string));
                    dataTable.Columns.Add("Тип Продукта", typeof(string));
                    dataTable.Columns.Add("Количество", typeof(int));
                    dataTable.Columns.Add("Цена за единицу", typeof(decimal));

                    foreach (var product in products)
                    {
                        var typeName = product.Type?.TypeName ?? "Unknown";

                        dataTable.Rows.Add(
                            product.ProductId,
                            product.ProductName,
                            typeName,
                            product.Quantity ?? 0,
                            product.Cost ?? 0.0m
                        );
                    }

                    ProductsData = dataTable;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            IsShowAllProductsCommandExecuted = true;
        }

        private void ShowProductsWithMinQuantity(object obj)
        {
            try
            {
                using (var connections = new System.Data.SqlClient.SqlConnection(connection))
                {
                    connections.Open();
                    var products = connections.Query<Product>("EXEC ShowProductsWithMinQuantity").ToList();
                    var productTypes = connections.Query<ProductType>("SELECT * FROM ProductTypes").ToDictionary(pt => pt.TypeId, pt => pt);

                    foreach (var product in products)
                    {
                        if (product.TypeId.HasValue && productTypes.ContainsKey(product.TypeId.Value))
                        {
                            product.Type = productTypes[product.TypeId.Value];
                        }
                        else
                        {
                            product.Type = new ProductType { TypeName = "Unknown" };
                        }
                    }

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("ID Продукта", typeof(int));
                    dataTable.Columns.Add("Название Продукта", typeof(string));
                    dataTable.Columns.Add("Тип Продукта", typeof(string));
                    dataTable.Columns.Add("Количество", typeof(int));
                    dataTable.Columns.Add("Цена за единицу", typeof(decimal));

                    foreach (var product in products)
                    {
                        var typeName = product.Type?.TypeName ?? "Unknown";

                        dataTable.Rows.Add(
                            product.ProductId,
                            product.ProductName,
                            typeName,
                            product.Quantity ?? 0,
                            product.Cost ?? 0.0m
                        );
                    }

                    ProductsData = dataTable;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            IsShowAllProductsCommandExecuted = true;

        }

        private void ShowProductsWithMaxQuantity(object obj)
        {
            try
            {
                using (var connections = new System.Data.SqlClient.SqlConnection(connection))
                {
                    connections.Open();
                    var products = connections.Query<Product>("EXEC ShowProductsWithMaxQuantity").ToList();
                    var productTypes = connections.Query<ProductType>("SELECT * FROM ProductTypes").ToDictionary(pt => pt.TypeId, pt => pt);

                    foreach (var product in products)
                    {
                        if (product.TypeId.HasValue && productTypes.ContainsKey(product.TypeId.Value))
                        {
                            product.Type = productTypes[product.TypeId.Value];
                        }
                        else
                        {
                            product.Type = new ProductType { TypeName = "Unknown" };
                        }
                    }

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("ID Продукта", typeof(int));
                    dataTable.Columns.Add("Название Продукта", typeof(string));
                    dataTable.Columns.Add("Тип Продукта", typeof(string));
                    dataTable.Columns.Add("Количество", typeof(int));
                    dataTable.Columns.Add("Цена за единицу", typeof(decimal));

                    foreach (var product in products)
                    {
                        var typeName = product.Type?.TypeName ?? "Unknown";

                        dataTable.Rows.Add(
                            product.ProductId,
                            product.ProductName,
                            typeName,
                            product.Quantity ?? 0,
                            product.Cost ?? 0.0m
                        );
                    }

                    ProductsData = dataTable;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            IsShowAllProductsCommandExecuted = true;
        }




        private void ShowAllSalesManagers(object obj)
        {
            try
            {
                using (var connections = new System.Data.SqlClient.SqlConnection(connection))
                {
                    connections.Open();
                    var managers = connections.Query<SalesManager>("EXEC ShowAllSalesManagers").ToList();

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Id", typeof(int));
                    dataTable.Columns.Add("Имя Менеджера", typeof(string));
                    dataTable.Columns.Add("Номер Телефона", typeof(string));

                    foreach (var manager in managers)
                    {
                        dataTable.Rows.Add(manager.ManagerId, manager.ManagerName, manager.PhoneNumber);
                    }

                    ProductsData = dataTable;
                    ProductsData.TableName = "SalesManagers";
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            IsShowAllProductsCommandExecuted = true;
        }



        private void ShowAllProductTypes(object obj)
        {
            try
            {
                using (var connections = new System.Data.SqlClient.SqlConnection(connection))
                {
                    connections.Open();
                    var productTypes = connections.Query<ProductType>("EXEC ShowAllProductTypes").ToList();

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Id", typeof(int));
                    dataTable.Columns.Add("Тип продукта", typeof(string));

                    foreach (var type in productTypes)
                    {
                        dataTable.Rows.Add(type.TypeId, type.TypeName);
                    }

                    ProductsData = dataTable;
                    ProductsData.TableName = "ProductTypes"; 
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            IsShowAllProductsCommandExecuted = true; 
        }


        private void ShowAllProducts(object obj)
        {
            LoadProductTypes();
            try
            {
                using (var connections = new System.Data.SqlClient.SqlConnection(connection))
                {
                    connections.Open();
                    var products = connections.Query<Product>("ShowAllProducts", commandType: CommandType.StoredProcedure).ToList();
                    foreach (var product in products)
                    {
                        if (product.TypeId.HasValue)
                        {
                            product.Type = connections.Query<ProductType>(
                                "SELECT * FROM ProductTypes WHERE TypeId = @TypeId",
                                new { TypeId = product.TypeId }
                            ).FirstOrDefault();
                        }
                    }
                    DataTable dataTable = new DataTable();
                    dataTable.TableName = "Products";
                    dataTable.Columns.Add("ID продукта", typeof(int));
                    dataTable.Columns.Add("Название продукта", typeof(string));
                    dataTable.Columns.Add("Тип продукта", typeof(string));
                    dataTable.Columns.Add("Количество", typeof(int));
                    dataTable.Columns.Add("Стоимость", typeof(decimal));

                    foreach (var product in products)
                    {
                        string typeName = product.Type?.TypeName ?? "Неизвестно";  
                        dataTable.Rows.Add(
                            product.ProductId,
                            product.ProductName,
                            typeName, 
                            product.Quantity,
                            product.Cost
                        );
                    }

                    ProductsData = dataTable;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка: " + ex.Message);
            }
            IsShowAllProductsCommandExecuted = true;
        }




        public async void InitializeMenuItems()
        {
            var productTypes = await GetCategoriesAsync();

            MenuItems.Clear();

            foreach (var type in productTypes)
            {
                var menuItem = new MenuItem
                {
                    Header = type,
                    Command = ShowProductsByTypeCommand,
                    CommandParameter = type
                };

                MenuItems.Add(menuItem);
            }
        }
        public async void InitializeMenuItems1()
        {
            var productTypes = await GetManagerNamesAsync();

            MenuItems1.Clear();

            foreach (var type in productTypes)
            {
                var menuItem = new MenuItem
                {
                    Header = type,
                    Command = ShowProductsSoldByManagerCommand,
                    CommandParameter = type
                };

                MenuItems1.Add(menuItem);
            }
        }
        public async void InitializeMenuItems2()
        {
            var productTypes = await GetCompanyNamesAsync();

            MenuItems2.Clear();

            foreach (var type in productTypes)
            {
                var menuItem = new MenuItem
                {
                    Header = type,
                    Command = ShowProductsBoughtByCompanyCommand,
                    CommandParameter = type
                };

                MenuItems2.Add(menuItem);
            }
        }
        public async Task<List<string>> GetCompanyNamesAsync()
        {
            try
            {
                using (var connections = new System.Data.SqlClient.SqlConnection(connection))
                {
                    await connections.OpenAsync();

                    var companyNames = await connections.QueryAsync<string>(
                        "SELECT DISTINCT CompanyName FROM CustomerCompanies ORDER BY CompanyName");

                    return companyNames.AsList();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return new List<string>();
            }
        }


        public async Task<List<string>> GetManagerNamesAsync()
        {
            try
            {
                using (var connections = new System.Data.SqlClient.SqlConnection(connection))
                {
                    await connections.OpenAsync();

                    var managerNames = await connections.QueryAsync<string>(
                        "SELECT DISTINCT ManagerName FROM SalesManagers ORDER BY ManagerName");

                    return managerNames.AsList();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return new List<string>();
            }
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            try
            {
                using (var connections = new System.Data.SqlClient.SqlConnection(connection))
                {
                    await connections.OpenAsync();

                    var productTypes = await connections.QueryAsync<string>(
                        "SELECT DISTINCT TypeName FROM ProductTypes");

                    return productTypes.AsList();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return new List<string>();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
