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

namespace StationeryCompany.ViewModel
{
    class ViewModelEditProducts : INotifyPropertyChanged
    {
        public string connection;
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
        public object? ID;
        public ICommand ChangeOrEditCommand { get; set; }

        public string originalProductName = ""; 
        public int originalQuantity;
        public decimal originalCost;
        public int originalTypeID = 0;
        public ViewModelEditProducts(string Title, string Content, object? ID, string connection) 
        {
            WindowTitle = Title;
            ContentButt = Content;
            this.ID = ID;
            this.connection = connection;
            LoadProductInfo();
            LoadProductTypes();
            ChangeOrEditCommand = new DelegateCommand(EditProduct, (object parameter) => true);
        }

        private void EditProduct(object obj)
        {
            if (ProductName != originalProductName || TypeID != originalTypeID ||
                Quantity != originalQuantity || Cost != originalCost)
            {
                var result = MessageBox.Show("Данные продукта были изменены. Вы уверены, что хотите сохранить изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var parameters = new Dictionary<string, object>
            {
                { "@ProductID", ID }, 
                { "@ProductName", ProductName },
                { "@TypeID", TypeID },
                { "@Quantity", Quantity },
                { "@Cost", Cost }
            };

                    ExecuteStoredProcedureNonQuery("UpdateProduct", parameters);
                    MessageBox.Show("Информация о продукте успешно обновлена.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                    originalProductName = ProductName;
                    originalTypeID = TypeID;
                    originalQuantity = Quantity;
                    originalCost = Cost;
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


        public void LoadProductTypes()
        {
            ProductTypes = new ObservableCollection<ProductType>();
            using (SqlConnection connect = new SqlConnection(connection))
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

        public void LoadProductInfo()
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetProductById", connect)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ProductID", ID);

                try
                {
                    connect.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ProductName = reader.IsDBNull(reader.GetOrdinal("ProductName"))
                                ? "Неизвестный продукт"
                                : reader.GetString(reader.GetOrdinal("ProductName"));

                            Quantity = reader.IsDBNull(reader.GetOrdinal("Quantity"))
                                ? 0
                                : reader.GetInt32(reader.GetOrdinal("Quantity"));

                            Cost = reader.IsDBNull(reader.GetOrdinal("Cost"))
                                ? 0
                                : reader.GetDecimal(reader.GetOrdinal("Cost"));

                             TypeID = reader.IsDBNull(reader.GetOrdinal("TypeID"))
                                 ? 0
                                 : reader.GetInt32(reader.GetOrdinal("TypeID"));
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
