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
                using (var db = new StationeryCompanyContext())
                {
                    var types = db.ProductTypes.ToList();
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
            ShowLatestSaleInfoCommand = new DelegateCommand(ShowLatestSaleInfo, (object parameter) => true);

            DeleteCommand = new DelegateCommand(DeleteAsync, (object parameter) => true);
            EditCommand = new DelegateCommand(Edit, (object parameter) => true);

            AddManagerCommand = new DelegateCommand(AddManager, (object parameter) => true);
            AddCompanyCommand = new DelegateCommand(AddCompany, (object parameter) => true);
            AddProductsCommand = new DelegateCommand(AddProducts, (object parameter) => true);

            AddProductCommand = new DelegateCommand(AddProduct, (object parameter) => true);

        }

        private void AddProduct(object obj)
        {
            EditProducts edit = new EditProducts("Добавление продукта", "Добавить");
            edit.ShowDialog();
            ShowAllProducts("ShowAllProducts");
        }

        private void AddProducts(object obj)
        {
            EditOrAddProductType edit = new EditOrAddProductType("Добавление типа продукта", "Добавить");
            edit.ShowDialog();
            ShowAllProductTypes("ShowAllProductTypes");
            InitializeMenuItems();

        }

        private void AddCompany(object obj)
        {
            EditCompanies editManager = new EditCompanies("Добавление компании", "Добавить");
            editManager.ShowDialog();
            ShowAllCompanies("ShowAllCompaniesWithOrderCount");
            InitializeMenuItems2();
        }

        private void AddManager(object obj)
        {
            EditManagers editManager = new EditManagers("Добавление менеджера", "Добавить");
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
                        EditProducts edit = new EditProducts("Изменение продукта", "Изменить", entityID);
                        edit.ShowDialog();
                        ShowAllProducts("ShowAllProducts");
                    }
                    if (table.DataView.Table.TableName == "ProductTypes")
                    {
                        EditOrAddProductType edit = new EditOrAddProductType("Изменение типа продукта", "Изменить", entityID);
                        edit.ShowDialog();
                        ShowAllProductTypes("ShowAllProductTypes");
                        InitializeMenuItems();
                    }
                    if (table.DataView.Table.TableName == "SalesManagers")
                    {
                        EditManagers editManager = new EditManagers("Изменение менеджера", "Изменить", entityID);
                        editManager.ShowDialog();
                        ShowAllSalesManagers("ShowAllSalesManagers");
                        InitializeMenuItems1();
                    }
                    if (table.DataView.Table.TableName == "CompaniesWithOrderCount")
                    {
                        EditCompanies editManager = new EditCompanies("Изменение компании", "Изменить", entityID);
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
                if (obj is DataRowView table)
                {
                    string storedProcedureName = null;
                    string parameterName = null;
                    object parameterValue = null;

                    switch (table.DataView.Table.TableName)
                    {
                        case "Products":
                            storedProcedureName = "DeleteProduct";
                            parameterName = "@ProductID";
                            parameterValue = table["Id"];
                            break;
                        case "ProductTypes":
                            storedProcedureName = "DeleteProductTypeAndRelatedProducts";
                            parameterName = "@TypeID";
                            parameterValue = table["Id"];
                            break;
                        case "SalesManagers":
                            storedProcedureName = "DeleteSalesManagerAndRelatedSales";
                            parameterName = "@ManagerID";
                            parameterValue = table["Id"];
                            break;
                        case "CompaniesWithOrderCount":
                            storedProcedureName = "DeleteSalesCompaniesAndRelatedSales";
                            parameterName = "@CompaniesID";
                            parameterValue = table["Id"];
                            break;
                    }

                    if (storedProcedureName != null)
                    {
                        using (var db = new StationeryCompanyContext())
                        {
                            await db.Database.ExecuteSqlRawAsync($"EXEC {storedProcedureName} {parameterName} = {parameterValue}");
                        }
                        RefreshUI(table.DataView.Table.TableName);
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
                using (var db = new StationeryCompanyContext())
                {
                    var companies = db.CustomerCompanies
                                      .FromSqlRaw("EXEC ShowAllCompaniesWithOrderCount")
                                      .ToList();

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("Id", typeof(int));
                    dataTable.Columns.Add("Название Компании", typeof(string));
                    dataTable.Columns.Add("Номер Телефона", typeof(string));
                    dataTable.Columns.Add("Город", typeof(string));
                    dataTable.Columns.Add("Количество Заказов", typeof(int));

                    foreach (var company in companies)
                    {
                        dataTable.Rows.Add(company.CompanyId, company.CompanyName, company.PhoneNumber, company.City);
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

        private void ShowLatestSaleInfo(object obj)
        {
            try
            {
                using (var db = new StationeryCompanyContext())
                {
                    var salesInfo = db.Sales
                                      .FromSqlRaw("EXEC ShowLatestSale")
                                      .ToList();

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("ProductName", typeof(string));
                    dataTable.Columns.Add("ManagerName", typeof(string));
                    dataTable.Columns.Add("CompanyName", typeof(string));

                    foreach (var info in salesInfo)
                    {
                        var productName = ProductTypes.FirstOrDefault(pt => pt.TypeId == info.ProductId)?.TypeName ?? "Unknown";

                        dataTable.Rows.Add(
                            productName,
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




        private async void ShowProductsBoughtByCompanyAsync(object companyName)
        {
            try
            {
                using (var db = new StationeryCompanyContext())
                {
                    var companyNameParameter = new Microsoft.Data.SqlClient.SqlParameter("@CompanyName", companyName);

                    var commandText = "SELECT * FROM ProductsBoughtByCompany(@CompanyName)";
                    var products = await db.Products.FromSqlRaw(commandText, companyNameParameter).ToListAsync();

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("ProductId", typeof(int));
                    dataTable.Columns.Add("ProductName", typeof(string));
                    dataTable.Columns.Add("QuantitySold", typeof(int));
                    dataTable.Columns.Add("TotalSaleAmount", typeof(decimal));

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

                using (var db = new StationeryCompanyContext())
                {
                    var managerNameParameter = new Microsoft.Data.SqlClient.SqlParameter("@ManagerName", managerName);
                    var sales = await db.Sales
                                        .FromSqlRaw("EXEC GetSalesByManager @ManagerName", managerNameParameter)
                                        .ToListAsync();

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("ID Продажи", typeof(int));
                    dataTable.Columns.Add("Тип Продукта", typeof(string));
                    dataTable.Columns.Add("Менеджер", typeof(string));
                    dataTable.Columns.Add("Количество проданного", typeof(int));
                    dataTable.Columns.Add("Цена за единицу", typeof(decimal));
                    dataTable.Columns.Add("Общая сумма продаж", typeof(decimal)); 

                    foreach (var sale in sales)
                    {
                        var typeName = ProductTypes.FirstOrDefault(pt => pt.TypeId == sale.ProductId)?.TypeName ?? "Unknown";

                        dataTable.Rows.Add(
                            sale.SaleId,
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

                using (var db = new StationeryCompanyContext())
                {
                    var typeNameParameter = new Microsoft.Data.SqlClient.SqlParameter("@TypeName", typeName);

                    var products = await db.Products
                                           .FromSqlRaw("EXEC ShowProductsByType @TypeName", typeNameParameter)
                                           .ToListAsync();

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
                            product.Quantity,
                            product.Cost
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
                using (var db = new StationeryCompanyContext())
                {
                    var products = db.Products
                                     .FromSqlRaw("EXEC ShowProductsWithMaxCost")
                                     .ToList();

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("ID Продукта", typeof(int));
                    dataTable.Columns.Add("Название Продукта", typeof(string));
                    dataTable.Columns.Add("Тип Продукта", typeof(string));
                    dataTable.Columns.Add("Количество", typeof(int));
                    dataTable.Columns.Add("Цена за единицу", typeof(decimal));

                    foreach (var product in products)
                    {
                        var typeName = ProductTypes.FirstOrDefault(pt => pt.TypeId == product.TypeId)?.TypeName ?? "Unknown";

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
                System.Windows.MessageBox.Show(ex.Message);
            }
            IsShowAllProductsCommandExecuted = false;
        }

        private void ShowProductsWithMinCost(object obj)
        {
            try
            {
                using (var db = new StationeryCompanyContext())
                {
                    var products = db.Products
                                     .FromSqlRaw("EXEC ShowProductsWithMinCost")
                                     .ToList();

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("ID Продукта", typeof(int));
                    dataTable.Columns.Add("Название Продукта", typeof(string));
                    dataTable.Columns.Add("Тип Продукта", typeof(string));
                    dataTable.Columns.Add("Количество", typeof(int));
                    dataTable.Columns.Add("Цена за единицу", typeof(decimal));

                    foreach (var product in products)
                    {
                        var typeName = ProductTypes.FirstOrDefault(pt => pt.TypeId == product.TypeId)?.TypeName ?? "Unknown";

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
                System.Windows.MessageBox.Show(ex.Message);
            }
            IsShowAllProductsCommandExecuted = false;

        }

        private void ShowProductsWithMinQuantity(object obj)
        {
            try
            {
                using (var db = new StationeryCompanyContext())
                {
                    var products = db.Products
                                     .FromSqlRaw("EXEC ShowProductsWithMinQuantity")
                                     .ToList();

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("ID Продукта", typeof(int));
                    dataTable.Columns.Add("Название Продукта", typeof(string));
                    dataTable.Columns.Add("Тип Продукта", typeof(string));
                    dataTable.Columns.Add("Количество", typeof(int));
                    dataTable.Columns.Add("Цена за единицу", typeof(decimal));

                    foreach (var product in products)
                    {
                        var typeName = ProductTypes.FirstOrDefault(pt => pt.TypeId == product.TypeId)?.TypeName ?? "Unknown";

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
                System.Windows.MessageBox.Show(ex.Message);
            }
            IsShowAllProductsCommandExecuted = true;

        }

        private void ShowProductsWithMaxQuantity(object obj)
        {
            try
            {
                using (var db = new StationeryCompanyContext())
                {
                    var products = db.Products
                                     .FromSqlRaw("EXEC ShowProductsWithMaxQuantity")
                                     .ToList();

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("ID Продукта", typeof(int));
                    dataTable.Columns.Add("Название Продукта", typeof(string));
                    dataTable.Columns.Add("Тип Продукта", typeof(string));
                    dataTable.Columns.Add("Количество", typeof(int));
                    dataTable.Columns.Add("Цена за единицу", typeof(decimal));

                    foreach (var product in products)
                    {
                        var typeName = ProductTypes.FirstOrDefault(pt => pt.TypeId == product.TypeId)?.TypeName ?? "Unknown";

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
                System.Windows.MessageBox.Show(ex.Message);
            }
            IsShowAllProductsCommandExecuted = true;
        }




        private void ShowAllSalesManagers(object obj)
        {
            try
            {
                using (var db = new StationeryCompanyContext())
                {
                    var managers = db.SalesManagers.FromSqlRaw("EXEC ShowAllSalesManagers").ToList();

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
                using (var db = new StationeryCompanyContext())
                {
                    var productTypes = db.ProductTypes.FromSqlRaw("EXEC ShowAllProductTypes").ToList();

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
                using (var db = new StationeryCompanyContext())
                {
                    var products = db.Products.FromSqlRaw("EXEC ShowAllProducts").ToList();

                    DataTable dataTable = new DataTable();
                    dataTable.TableName = "Products";
                    dataTable.Columns.Add("Id", typeof(int));
                    dataTable.Columns.Add("Продукт", typeof(string));
                    dataTable.Columns.Add("Тип продукта", typeof(string));
                    dataTable.Columns.Add("Количество", typeof(int));
                    dataTable.Columns.Add("Стоимость", typeof(decimal));

                    foreach (var product in products)
                    {
                        var typeName = ProductTypes.FirstOrDefault(pt => pt.TypeId == product.TypeId)?.TypeName ?? "Unknown";

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
                System.Windows.MessageBox.Show(ex.Message);
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
                using (var db = new StationeryCompanyContext())
                {
                    var companyNames = await db.CustomerCompanies
                                               .Select(cc => cc.CompanyName)
                                               .Distinct()
                                               .OrderBy(cc => cc)
                                               .ToListAsync();

                    return companyNames;
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
                using (var db = new StationeryCompanyContext())
                {
                    var managerNames = await db.SalesManagers
                                               .Select(sm => sm.ManagerName)
                                               .Distinct()
                                               .OrderBy(sm => sm)
                                               .ToListAsync();

                    return managerNames;
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
                using (var db = new StationeryCompanyContext())
                {
                    var productTypes = await db.ProductTypes
                                               .Select(pt => pt.TypeName)
                                               .Distinct()
                                               .ToListAsync();

                    return productTypes;
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
