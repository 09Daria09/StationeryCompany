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

namespace StationeryCompany.ViewModel
{
    class ViewModelEditCompanies : INotifyPropertyChanged
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
        public string _companyName;
        public string CompanyName
        {
            get => _companyName;
            set
            {
                if (_companyName != value)
                {
                    _companyName = value;
                    OnPropertyChanged(nameof(CompanyName));
                }
            }
        }

        public string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        public string _city;
        public string City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged(nameof(City));
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
        public object? IDproductsType;
        public ICommand ChangeOrEditCommand { get; set; }
        public string originalCompanyName = "";
        public string originalPhoneNumber = "";
        public string originalCity = "";
        public ViewModelEditCompanies(string Title, string Content, object? ID, string connection) 
        {
            WindowTitle = Title;
            ContentButt = Content;
            IDproductsType = ID;
            this.connection = connection;
            LoadProductType();
            ChangeOrEditCommand = new DelegateCommand(Edit, (object parameter) => true);
        }

        private void Edit(object obj)
        {
            if (CompanyName != originalCompanyName || PhoneNumber != originalPhoneNumber || City != originalCity)
            {
                var result = MessageBox.Show("Текст был изменен. Вы уверены, что хотите сохранить изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var parameters = new Dictionary<string, object>
            {
                { "@CompanyID", IDproductsType },
                { "@CompanyName", CompanyName },
                { "@PhoneNumber", PhoneNumber },
                { "@City", City }
            };

                    ExecuteStoredProcedureNonQuery("UpdateCustomerCompany", parameters);
                    MessageBox.Show("Информация о компании успешно обновлена.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public void LoadProductType()
        {

            using (SqlConnection connect = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetCompaniesById", connect)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@CompanyID", IDproductsType);

                try
                {
                    connect.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            CompanyName = reader.IsDBNull(reader.GetOrdinal("CompanyName"))
                                ? "Неизвестный тип"
                                : reader.GetString(reader.GetOrdinal("CompanyName"));
                            originalCompanyName = CompanyName;

                            PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber"))
                                ? "Неизвестный тип"
                                : reader.GetString(reader.GetOrdinal("PhoneNumber"));
                            originalPhoneNumber = PhoneNumber;

                            City = reader.IsDBNull(reader.GetOrdinal("City"))
                                ? "Неизвестный тип"
                                : reader.GetString(reader.GetOrdinal("City"));
                            originalCity = City;
                        }
                        else
                        {
                            CompanyName = "Компания не найдена.";
                        }
                    }
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
