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
        public ViewModelAddCompanies(string Title, string Content) 
        {
            WindowTitle = Title;
            ContentButt = Content;
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
            using (var context = new StationeryCompanyContext()) 
            {
                var newCompany = new CustomerCompany
                {
                    CompanyName = CompanyName,
                    PhoneNumber = PhoneNumber,
                    City = City
                };

                try
                {
                    await context.CustomerCompanies.AddAsync(newCompany); 
                    await context.SaveChangesAsync(); 
                    MessageBox.Show("Компания успешно добавлена.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении компании: {ex.Message}");
                }

                CompanyName = "";
                PhoneNumber = "";
                City = "";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
