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
        public int? IDproductsType;
        public ICommand ChangeOrEditCommand { get; set; }
        public string originalCompanyName = "";
        public string originalPhoneNumber = "";
        public string originalCity = "";
        public ViewModelEditCompanies(string Title, string Content, int? ID) 
        {
            WindowTitle = Title;
            ContentButt = Content;
            IDproductsType = ID;
            this.connection = connection;
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
                    using (var context = new StationeryCompanyContext()) 
                    {
                        var company = await context.CustomerCompanies.FindAsync(IDproductsType);
                        if (company != null)
                        {
                            company.CompanyName = CompanyName;
                            company.PhoneNumber = PhoneNumber;
                            company.City = City;

                            try
                            {
                                await context.SaveChangesAsync();
                                MessageBox.Show("Информация о компании успешно обновлена.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                                originalCompanyName = CompanyName;
                                originalPhoneNumber = PhoneNumber;
                                originalCity = City;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка при обновлении компании: {ex.Message}");
                            }
                        }
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
            using (var context = new StationeryCompanyContext()) 
            {
                try
                {
                    var company = await context.CustomerCompanies
                        .Where(c => c.CompanyId == IDproductsType) 
                        .Select(c => new { c.CompanyName, c.PhoneNumber, c.City }) 
                        .FirstOrDefaultAsync();

                    if (company != null)
                    {
                        CompanyName = company.CompanyName ?? "Неизвестный тип";
                        PhoneNumber = company.PhoneNumber ?? "Неизвестный тип";
                        City = company.City ?? "Неизвестный тип";

                        originalCompanyName = CompanyName;
                        originalPhoneNumber = PhoneNumber;
                        originalCity = City;
                    }
                    else
                    {
                        CompanyName = "Компания не найдена.";
                        PhoneNumber = "Неизвестный тип";
                        City = "Неизвестный тип";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных компании: {ex.Message}");
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
