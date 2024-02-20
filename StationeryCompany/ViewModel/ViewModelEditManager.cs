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
        public object? ID;
        public ICommand ChangeOrEditCommand { get; set; }
        public string originalTypeName = "";
        public string originalPhone = ""; 
        public ViewModelEditManager(string Title, string Content, object? ID, string connection)
        {
            WindowTitle = Title;
            ContentButt = Content;
            this.ID = ID;
            this.connection = connection;
            LoadProductType();
            ChangeOrEditCommand = new DelegateCommand(Edit, (object parameter) => true);
        }

        private void Edit(object obj)
        {
            if (NameManader != originalTypeName || Phone != originalPhone)
            {
                var result = MessageBox.Show("Текст был изменен. Вы уверены, что хотите сохранить изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var parameters = new Dictionary<string, object>
            {
                { "@ManagerID", ID },
                { "@ManagerName", NameManader },
                {"@PhoneNumber",  Phone}
            };

                    ExecuteStoredProcedureNonQuery("UpdateSalesManager", parameters);
                    MessageBox.Show("Информация о менеджере успешно обновлена.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
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
                SqlCommand cmd = new SqlCommand("GetManagerById", connect)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ManagerID", ID);

                try
                {
                    connect.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            NameManader = reader.IsDBNull(reader.GetOrdinal("ManagerName"))
                                ? "Неизвестный тип"
                                : reader.GetString(reader.GetOrdinal("ManagerName"));
                            originalTypeName = NameManader;

                            Phone = reader.IsDBNull(reader.GetOrdinal("PhoneNumber"))
                               ? "Неизвестный тип"
                               : reader.GetString(reader.GetOrdinal("PhoneNumber"));
                            originalPhone = Phone;
                        }
                        else
                        {
                            NameManader = "Менеджер не найден.";
                            NameManader = "???";
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
