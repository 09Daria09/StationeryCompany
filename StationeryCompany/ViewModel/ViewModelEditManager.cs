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
    class ViewModelEditManager : INotifyPropertyChanged
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
        public string _nameManader;
        public string NameManader
        {
            get => _nameManader;
            set
            {
                if (_nameManader != value)
                {
                    _nameManader = value;
                    OnPropertyChanged(nameof(NameManader));
                }
            }
        }

        public string _phone;
        public string Phone 
        {
            get => _phone;
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChanged(nameof(Phone));
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
        public int? ID;
        public ICommand ChangeOrEditCommand { get; set; }
        public string originalTypeName = "";
        public string originalPhone = ""; 
        public ViewModelEditManager(string Title, string Content, int? ID, string connection)
        {
            WindowTitle = Title;
            ContentButt = Content;
            this.ID = ID;
            connectionString = connection;
            LoadManagerByIdAsync();
            ChangeOrEditCommand = new DelegateCommand(async (object parameter) =>
            {
                await EditAsync(parameter);
            }, (object parameter) => true);
        }

        private async Task EditAsync(object parameter)
        {
            if (NameManader != originalTypeName || Phone != originalPhone)
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
                                "UPDATE SalesManagers SET ManagerName = @ManagerName, PhoneNumber = @PhoneNumber WHERE ManagerId = @ManagerId",
                                new { ManagerName = NameManader, PhoneNumber = Phone, ManagerId = ID });

                            if (affectedRows > 0)
                            {
                                MessageBox.Show("Информация о менеджере успешно обновлена.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                                originalTypeName = NameManader;
                                originalPhone = Phone;
                            }
                            else
                            {
                                MessageBox.Show("Не удалось обновить информацию о менеджере.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при обновлении менеджера: {ex.Message}");
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




        public async Task LoadManagerByIdAsync()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var manager = await connection.QueryFirstOrDefaultAsync(
                        "SELECT ManagerName, PhoneNumber FROM SalesManagers WHERE ManagerId = @ManagerId",
                        new { ManagerId = ID });

                    if (manager != null)
                    {
                        NameManader = manager.ManagerName ?? "Неизвестный менеджер";
                        Phone = manager.PhoneNumber ?? "Неизвестный номер";
                        originalTypeName = NameManader;
                        originalPhone = Phone;
                    }
                    else
                    {
                        NameManader = "Менеджер не найден.";
                        Phone = "???";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных менеджера: {ex.Message}");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
