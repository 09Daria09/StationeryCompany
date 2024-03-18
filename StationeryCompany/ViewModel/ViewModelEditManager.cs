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
    class ViewModelEditManager : INotifyPropertyChanged
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
        public ViewModelEditManager(string Title, string Content, int? ID)
        {
            WindowTitle = Title;
            ContentButt = Content;
            this.ID = ID;
            this.connection = connection;
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
                    using (var context = new StationeryCompanyContext())
                    {
                        var manager = await context.SalesManagers.FindAsync(ID);
                        if (manager != null)
                        {
                            manager.ManagerName = NameManader;
                            manager.PhoneNumber = Phone;

                            try
                            {
                                await context.SaveChangesAsync();
                                MessageBox.Show("Информация о менеджере успешно обновлена.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                                originalTypeName = NameManader;
                                originalPhone = Phone;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка при обновлении менеджера: {ex.Message}");
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



        public async Task LoadManagerByIdAsync()
        {
            using (var context = new StationeryCompanyContext()) 
            {
                try
                {
                    var manager = await context.SalesManagers
                        .Where(m => m.ManagerId == ID) 
                        .Select(m => new { m.ManagerName, m.PhoneNumber }) 
                        .FirstOrDefaultAsync();

                    if (manager != null)
                    {
                        NameManader = manager.ManagerName ?? "Неизвестный тип";
                        Phone = manager.PhoneNumber ?? "Неизвестный тип";
                        originalTypeName = NameManader;
                        originalPhone = Phone;
                    }
                    else
                    {
                        NameManader = "Менеджер не найден.";
                        Phone = "???";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных менеджера: {ex.Message}");
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
