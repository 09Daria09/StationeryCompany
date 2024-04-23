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
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace StationeryCompany.ViewModel
{
    class ViewModelEditCompanies : INotifyPropertyChanged
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
        public string originalCompanyName = "";
        public string originalPhoneNumber = "";
        public string originalCity = "";
        public ViewModelEditCompanies(string Title, string Content, int? ID, string connection) 
        {
            WindowTitle = Title;
            ContentButt = Content;
            IDproductsType = ID;
            connectionString = connection;
            LoadCompanyByIdAsync();
            ChangeOrEditCommand = new DelegateCommand(async (object parameter) =>
            {
                await EditAsync(parameter);
            }, (object parameter) => true);
        }

        private async Task EditAsync(object parameter)
        {
            if (CompanyName != originalCompanyName || PhoneNumber != originalPhoneNumber || City != originalCity)
            {
                var result = MessageBox.Show("Текст был изменен. Вы уверены, что хотите сохранить изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (var connection = new SqlConnection(connectionString))
                        {
                            await connection.OpenAsync();

                            var affectedRows = await connection.ExecuteAsync(
                                "UPDATE CustomerCompanies SET CompanyName = @CompanyName, PhoneNumber = @PhoneNumber, City = @City WHERE CompanyId = @CompanyId",
                                new { CompanyName, PhoneNumber, City, CompanyId = IDproductsType });

                            if (affectedRows > 0)
                            {
                                MessageBox.Show("Информация о компании успешно обновлена.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                                originalCompanyName = CompanyName;
                                originalPhoneNumber = PhoneNumber;
                                originalCity = City;
                            }
                            else
                            {
                                MessageBox.Show("Не удалось обновить информацию о компании.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при обновлении компании: {ex.Message}");
                    }
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



        public async Task LoadCompanyByIdAsync()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var company = await connection.QueryFirstOrDefaultAsync<CustomerCompany>(
                        "SELECT CompanyName, PhoneNumber, City FROM CustomerCompanies WHERE CompanyId = @CompanyId",
                        new { CompanyId = IDproductsType });

                    if (company != null)
                    {
                        CompanyName = company.CompanyName ?? "Неизвестная компания";
                        PhoneNumber = company.PhoneNumber ?? "Неизвестный номер";
                        City = company.City ?? "Неизвестный город";

                        originalCompanyName = CompanyName;
                        originalPhoneNumber = PhoneNumber;
                        originalCity = City;
                    }
                    else
                    {
                        CompanyName = "Компания не найдена.";
                        PhoneNumber = "Неизвестный номер";
                        City = "Неизвестный город";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных компании: {ex.Message}");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
