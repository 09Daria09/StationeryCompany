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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using StationeryCompany.Model;
using Dapper;

namespace StationeryCompany.ViewModel
{
    class ViewModelAddCompanies : INotifyPropertyChanged
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
        public int? IDproductsType;
        public ICommand ChangeOrEditCommand { get; set; }
        public ViewModelAddCompanies(string Title, string Content, string connection) 
        {
            WindowTitle = Title;
            ContentButt = Content;
            connectionString = connection;
            ChangeOrEditCommand = new DelegateCommand(async (object parameter) =>
            {
                await AddCompanyAsync(parameter);
            }, CanAdd);
        }

        private bool CanAdd(object obj)
        {
            return !string.IsNullOrWhiteSpace(CompanyName) && !string.IsNullOrWhiteSpace(PhoneNumber) && !string.IsNullOrWhiteSpace(City);
        }

        private async Task AddCompanyAsync(object param)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new
                    {
                        CompanyName = CompanyName,
                        PhoneNumber = PhoneNumber,
                        City = City
                    };

                    var query = @"INSERT INTO CustomerCompanies (CompanyName, PhoneNumber, City) VALUES (@CompanyName, @PhoneNumber, @City)";

                    await connection.ExecuteAsync(query, parameters);

                    MessageBox.Show("Компания успешно добавлена.");
                }

                CompanyName = "";
                PhoneNumber = "";
                City = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении компании: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
