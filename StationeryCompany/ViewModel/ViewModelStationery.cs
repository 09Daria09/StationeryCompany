using StationeryCompany.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Microsoft.VisualBasic;
using StationeryCompany.View;
using System.Windows.Media;
using System.Windows.Markup;
using System.Reflection;
using System.Reflection.Metadata;
using static StationeryCompany.View.EditManagers;

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

        public ICommand AddManagerCommand {  get; private set; }
        public ICommand AddCompanyCommand { get; private set; }
        public ICommand AddProductsCommand {  get; private set; }
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

        public ViewModelStationery(string connect)
        {
            connection = connect;
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
            ShowAllSalesCommand = new DelegateCommand(ShowAllSales, (object parameter) => true);

            ShowProductsWithMaxQuantityCommand = new DelegateCommand(ShowProductsWithMaxQuantity, (object parameter) => true);
            ShowProductsWithMinQuantityCommand = new DelegateCommand(ShowProductsWithMinQuantity, (object parameter) => true);
            ShowProductsWithMinCostCommand = new DelegateCommand(ShowProductsWithMinCost, (object parameter) => true);
            ShowProductsWithMaxCostCommand = new DelegateCommand(ShowProductsWithMaxCost, (object parameter) => true);
            ShowProductsByTypeCommand = new DelegateCommand(ShowProductsByType, (object parameter) => true);
            ShowProductsSoldByManagerCommand = new DelegateCommand(ShowProductsSoldByManager, (object parameter) => true);
            ShowProductsBoughtByCompanyCommand = new DelegateCommand(ShowProductsBoughtByCompany, (object parameter) => true);
            ShowLatestSaleInfoCommand = new DelegateCommand(ShowLatestSaleInfo, (object parameter) => true);
            ShowAverageQuantityByTypeCommand = new DelegateCommand(ShowAverageQuantityByType, (object parameter) => true);
            ShowTopSellingManagerByUnitsCommand = new DelegateCommand(ShowTopSellingManagerByUnits, (object parameter) => true);
            ShowTopSellingManagerByProfitCommand = new DelegateCommand(ShowTopSellingManagerByProfit, (object parameter) => true);
            ShowUnsoldProductsCommand = new DelegateCommand(ShowUnsoldProducts, (object parameter) => true);
            ShowTopCustomerCompanyBySpendingCommand = new DelegateCommand(ShowTopCustomerCompanyBySpending, (object parameter) => true);
            ShowTopProductTypeByUnitsSoldCommand = new DelegateCommand(ShowTopProductTypeByUnitsSold, (object parameter) => true);
            ShowTopProfitableProductTypeCommand = new DelegateCommand(ShowTopProfitableProductType, (object parameter) => true);
            ShowMostPopularProductsCommand = new DelegateCommand(ShowMostPopularProducts, (object parameter) => true);
            ShowTopSellingManagerByProfitInPeriodCommand = new DelegateCommand(ShowTopSellingManagerByProfitInPeriod, (object parameter) => true);

            DeleteCommand = new DelegateCommand(Delete, (object parameter) => true);
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
                var entityID = selectedRow.ItemArray[0];

                if (obj is DataRowView table)
                {
                    if (table.DataView.Table.TableName == "ShowAllProducts")
                    {
                        EditProducts edit = new EditProducts("Изменение продукта", "Изменить", entityID, connection);
                        edit.ShowDialog();
                        ShowAllProducts("ShowAllProducts");
                    }
                    if (table.DataView.Table.TableName == "ShowAllProductTypes")
                    {
                        EditOrAddProductType edit = new EditOrAddProductType("Изменение типа продукта", "Изменить", entityID, connection);
                        edit.ShowDialog();
                        ShowAllProductTypes("ShowAllProductTypes");
                        InitializeMenuItems();
                    }
                    if (table.DataView.Table.TableName == "ShowAllSalesManagers")
                    {
                        EditManagers editManager = new EditManagers("Изменение менеджера", "Изменить", entityID, connection);
                        editManager.ShowDialog();
                        ShowAllSalesManagers("ShowAllSalesManagers");
                        InitializeMenuItems1();
                    }
                    if (table.DataView.Table.TableName == "ShowAllCompaniesWithOrderCount")
                    {
                        EditCompanies editManager = new EditCompanies("Изменение компании", "Изменить", entityID, connection);
                        editManager.ShowDialog();
                        ShowAllCompanies("ShowAllCompaniesWithOrderCount");
                        InitializeMenuItems2();
                    }
                }
            }

        }


        private void Delete(object obj)
        {
            var result = MessageBox.Show("Вы уверены, что хотите удалить этот компонент?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (obj is DataRowView table)
                {
                    if (table.DataView.Table.TableName == "ShowAllProducts")
                    {
                        DeleteEntityFromTable(obj, "DeleteProduct", "@ProductID", () => ShowAllProducts("ShowAllProducts"));
                    }
                    if (table.DataView.Table.TableName == "ShowAllProductTypes")
                    {
                        DeleteEntityFromTable(obj, "DeleteProductTypeAndRelatedProducts", "@TypeID", () =>
                        {
                            ShowAllProducts("ShowAllProductTypes");
                            InitializeMenuItems();
                        });
                    }
                    if (table.DataView.Table.TableName == "ShowAllSalesManagers")
                    {
                        DeleteEntityFromTable(obj, "DeleteSalesManagerAndRelatedSales", "@ManagerID", () =>
                        {
                            ShowAllSalesManagers("ShowAllSalesManagers");
                            InitializeMenuItems1();
                        });
                    }
                    if (table.DataView.Table.TableName == "ShowAllCompaniesWithOrderCount")
                    {
                        DeleteEntityFromTable(obj, "DeleteSalesCompaniesAndRelatedSales", "@CompaniesID", () =>
                        {
                            ShowAllCompanies("ShowAllCompaniesWithOrderCount");
                            InitializeMenuItems2();
                        });
                    }
                }
            }
        }
        private void DeleteEntityFromTable(object obj, string storedProcedureName, string parameterName, Action postDeleteAction)
        {
            if (obj is DataRowView dataRowView)
            {
                DataRow selectedRow = dataRowView.Row;
                var entityID = selectedRow.ItemArray[0];
                var parameters = new Dictionary<string, object>
        {
            { parameterName, entityID }
        };
                ExecuteStoredProcedureNonQuery(storedProcedureName, parameters);
                postDeleteAction?.Invoke(); //если есть еще какой-то компонент 
            }
        }

        private void ShowAllSales(object obj)
        {
            ExecuteStoredProcedure("GetAllSalesDetails");
            IsShowAllProductsCommandExecuted = false;
        }
        private void ShowAllCompanies(object obj)
        {
            ExecuteStoredProcedure("ShowAllCompaniesWithOrderCount");
            IsShowAllProductsCommandExecuted = true;
        }

        private void ShowTopSellingManagerByProfitInPeriod(object obj) /*Нужно изменить отображение InputBox*/
        {
            DateTime startDate, endDate;
            bool isStartDateValid = false;
            bool isEndDateValid = false;

            do
            {
                string inputStartDate = Interaction.InputBox("Введите начальную дату периода для анализа (например, 2024-01-01)", "Запрос начальной даты периода", "2024-01-01");
                if (DateTime.TryParse(inputStartDate, out startDate))
                {
                    isStartDateValid = true;
                }
                else
                {
                    MessageBox.Show("Введенная начальная дата не является допустимым форматом даты. Пожалуйста, введите дату корректно.", "Ошибка", MessageBoxButton.OK);
                }
            }
            while (!isStartDateValid);

            do
            {
                string inputEndDate = Interaction.InputBox("Введите конечную дату периода для анализа (например, 2024-01-30)", "Запрос конечной даты периода", "2024-01-30");
                if (DateTime.TryParse(inputEndDate, out endDate) && endDate >= startDate)
                {
                    isEndDateValid = true;
                }
                else
                {
                    MessageBox.Show("Введенная конечная дата не является допустимым форматом даты или меньше начальной даты. Пожалуйста, введите дату корректно.", "Ошибка", MessageBoxButton.OK);
                }
            }
            while (!isEndDateValid);

            var parameters = new Dictionary<string, object>
    {
        { "@StartDate", startDate },
        { "@EndDate", endDate }
    };

            ExecuteStoredProcedure("ShowTopSellingManagerByProfitInPeriod", parameters);
            IsShowAllProductsCommandExecuted = false;
        }


        private void ShowUnsoldProducts(object obj)
        {
            string input = Interaction.InputBox("Введите количество дней", "Запрос количества дней", "30");
            int days = 0;
            if (int.TryParse(input, out days))
            {
                var parameters = new Dictionary<string, object>
        {
            { "@DaysNotSold", days }
        };
                ExecuteStoredProcedure("ShowUnsoldProducts", parameters);
                IsShowAllProductsCommandExecuted = false;
            }
        }

        private void ShowMostPopularProducts(object obj)
        {
            ExecuteStoredProcedure("ShowMostPopularProducts");
            IsShowAllProductsCommandExecuted = false;
        }

        private void ShowTopProfitableProductType(object obj)
        {
            ExecuteStoredProcedure("ShowTopProfitableProductType");
            IsShowAllProductsCommandExecuted = false;
        }

        private void ShowTopProductTypeByUnitsSold(object obj)
        {
            ExecuteStoredProcedure("ShowTopProductType");
            IsShowAllProductsCommandExecuted = false;
        }

        private void ShowTopCustomerCompanyBySpending(object obj)
        {
            ExecuteStoredProcedure("ShowTopCustomer");
            IsShowAllProductsCommandExecuted = false;
        }

        private void ShowTopSellingManagerByProfit(object obj)
        {
            ExecuteStoredProcedure("ShowTopByProfit");
            IsShowAllProductsCommandExecuted = false;
        }

        private void ShowTopSellingManagerByUnits(object obj)
        {
            ExecuteStoredProcedure("ShowTopManager");
            IsShowAllProductsCommandExecuted = false;
        }

        private void ShowAverageQuantityByType(object obj)
        {
            ExecuteStoredProcedure("ShowAverageQuantityByProductType");
            IsShowAllProductsCommandExecuted = false;
        }

        private void ShowLatestSaleInfo(object obj)
        {
            ExecuteStoredProcedure("ShowLatestSale");
            IsShowAllProductsCommandExecuted = false;
        }

        private void ShowProductsBoughtByCompany(object obj)
        {
            var typeName = obj as string;
            if (typeName == null) return;

            var parameters = new Dictionary<string, object>
    {
        { "@CompanyName", typeName }
    };

            ExecuteStoredProcedure("ShowProductsBoughtByCompany", parameters);
            IsShowAllProductsCommandExecuted = false;
        }

        private void ShowProductsSoldByManager(object obj)
        {
            var typeName = obj as string;
            if (typeName == null) return;

            var parameters = new Dictionary<string, object>
    {
        { "@ManagerName", typeName }
    };

            ExecuteStoredProcedure("ShowProductsSoldByManager", parameters);
            IsShowAllProductsCommandExecuted = false;
        }
        private void ShowProductsByType(object obj)
        {
            var typeName = obj as string;
            if (typeName == null) return;

            var parameters = new Dictionary<string, object>
    {
        { "@TypeName", typeName }
    };

            ExecuteStoredProcedure("ShowProductsByType", parameters);
            IsShowAllProductsCommandExecuted = false;
        }

        private void ShowProductsWithMaxCost(object obj)
        {
            ExecuteStoredProcedure("ShowProductsWithMaxCost");
            IsShowAllProductsCommandExecuted = false;
        }

        private void ShowProductsWithMinCost(object obj)
        {
            ExecuteStoredProcedure("ShowProductsWithMinCost");
            IsShowAllProductsCommandExecuted = false;
        }

        private void ShowProductsWithMinQuantity(object obj)
        {
            ExecuteStoredProcedure("ShowProductsWithMinQuantity");
            IsShowAllProductsCommandExecuted = false;
        }

        private void ShowProductsWithMaxQuantity(object obj)
        {
            ExecuteStoredProcedure("ShowProductsWithMaxQuantity");
            IsShowAllProductsCommandExecuted = false;
        }

        private void ShowAllSalesManagers(object obj)
        {

            ExecuteStoredProcedure("ShowAllSalesManagers");
            IsShowAllProductsCommandExecuted = true;
        }

        private void ShowAllProductTypes(object obj)
        {
            ExecuteStoredProcedure("ShowAllProductTypes");
            IsShowAllProductsCommandExecuted = true;
        }

        private void ShowAllProducts(object obj)
        {
            ExecuteStoredProcedure("ShowAllProducts");
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
            var companyNames = new List<string>();
            using (var connect = new SqlConnection(connection))
            {
                await connect.OpenAsync();
                var command = new SqlCommand("SELECT DISTINCT CompanyName FROM CustomerCompanies ORDER BY CompanyName", connect);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        companyNames.Add(reader.GetString(0));
                    }
                }
            }
            return companyNames;
        }

        public async Task<List<string>> GetManagerNamesAsync()
        {
            var managerNames = new List<string>();
            using (var connect = new SqlConnection(connection))
            {
                await connect.OpenAsync();
                var command = new SqlCommand("SELECT DISTINCT ManagerName FROM SalesManagers ORDER BY ManagerName", connect);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        managerNames.Add(reader.GetString(0));
                    }
                }
            }
            return managerNames;
        }
        public async Task<List<string>> GetCategoriesAsync()
        {
            var productTypes = new List<string>();
            using (var connect = new SqlConnection(connection))
            {
                await connect.OpenAsync();
                var command = new SqlCommand("SELECT DISTINCT TypeName FROM ProductTypes", connect);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        productTypes.Add(reader.GetString(0));
                    }
                }
            }
            return productTypes;
        }

        private void ExecuteStoredProcedure(string procedureName)
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand(procedureName, connect)
                {
                    CommandType = CommandType.StoredProcedure
                };

                DataTable dataTable = new DataTable();
                dataTable.TableName = procedureName;
                try
                {
                    connect.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                    ProductsData = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void ExecuteStoredProcedure(string procedureName, Dictionary<string, object> procedureParams = null)
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand(procedureName, connect)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (procedureParams != null)
                {
                    foreach (var param in procedureParams)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

                DataTable dataTable = new DataTable();
                try
                {
                    connect.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                    ProductsData = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void ExecuteStoredProcedureNonQuery(string procedureName, Dictionary<string, object> procedureParams = null)
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand(procedureName, connect)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (procedureParams != null)
                {
                    foreach (var param in procedureParams)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                }

                try
                {
                    connect.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
