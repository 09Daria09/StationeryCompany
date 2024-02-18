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
            ShowProductsWithMaxQuantityCommand = new DelegateCommand(ShowProductsWithMaxQuantity, (object parameter) => true);
            ShowProductsWithMinQuantityCommand = new DelegateCommand(ShowProductsWithMinQuantity, (object parameter) => true);
            ShowProductsWithMinCostCommand = new DelegateCommand(ShowProductsWithMinCost, (object parameter) => true);
            ShowProductsWithMaxCostCommand = new DelegateCommand(ShowProductsWithMaxCost, (object parameter) => true);
            ShowProductsByTypeCommand = new DelegateCommand(ShowProductsByType, (object parameter) => true);
            ShowProductsSoldByManagerCommand = new DelegateCommand(ShowProductsSoldByManager, (object parameter) => true);
            ShowProductsBoughtByCompanyCommand = new DelegateCommand(ShowProductsBoughtByCompany, (object parameter) => true);
            ShowLatestSaleInfoCommand = new DelegateCommand(ShowLatestSaleInfo, (object parameter) => true);
            ShowAverageQuantityByTypeCommand = new DelegateCommand(ShowAverageQuantityByType, (object parameter) => true);
        }

        private void ShowAverageQuantityByType(object obj)
        {
            ExecuteStoredProcedure("ShowAverageQuantityByProductType");
        }

        private void ShowLatestSaleInfo(object obj)
        {
            ExecuteStoredProcedure("ShowLatestSale");
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
        }

        private void ShowProductsWithMaxCost(object obj)
        {
            ExecuteStoredProcedure("ShowProductsWithMaxCost");
        }

        private void ShowProductsWithMinCost(object obj)
        {
            ExecuteStoredProcedure("ShowProductsWithMinCost");
        }

        private void ShowProductsWithMinQuantity(object obj)
        {
            ExecuteStoredProcedure("ShowProductsWithMinQuantity");
        }

        private void ShowProductsWithMaxQuantity(object obj)
        {
            ExecuteStoredProcedure("ShowProductsWithMaxQuantity");
        }

        private void ShowAllSalesManagers(object obj)
        {

            ExecuteStoredProcedure("ShowAllSalesManagers");
        }

        private void ShowAllProductTypes(object obj)
        {
            ExecuteStoredProcedure("ShowAllProductTypes");
        }

        private void ShowAllProducts(object obj)
        {
            ExecuteStoredProcedure("ShowAllProducts");
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


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
