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
using StationeryCompany.Model;
using System.Collections.ObjectModel;

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

        private ObservableCollection<ProductType> _productTypes;
        public ObservableCollection<ProductType> ProductTypes
        {
            get => _productTypes;
            set
            {
                if (_productTypes != value)
                {
                    _productTypes = value;
                    OnPropertyChanged(nameof(ProductTypes));
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
            LoadProductTypes();
            ChangeOrEditCommand = new DelegateCommand(Add, CanAdd);
        }

        private void Add(object obj)
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddProduct", connect)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ProductName", ProductName);
                cmd.Parameters.AddWithValue("@TypeID", TypeID);
                cmd.Parameters.AddWithValue("@Quantity", Quantity);
                cmd.Parameters.AddWithValue("@Cost", Cost);

                try
                {
                    connect.Open();
                    cmd.ExecuteNonQuery(); 
                    MessageBox.Show("Продукт успешно добавлен.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении продукта: {ex.Message}");
                }
            }
        }

        private bool CanAdd(object obj)
        {
            return !string.IsNullOrWhiteSpace(ProductName) && Quantity > 0 && Cost > 0 && TypeID > 0;
        }

        public void LoadProductTypes()
        {
            ProductTypes = new ObservableCollection<ProductType>();
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("ShowAllProductTypes", connect)
                {
                    CommandType = CommandType.StoredProcedure
                };

                try
                {
                    connect.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductTypes.Add(new ProductType
                            {
                                TypeID = reader.GetInt32(reader.GetOrdinal("ID типа")),
                                TypeName = reader.GetString(reader.GetOrdinal("Название типа"))
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки типов продуктов: {ex.Message}");
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
