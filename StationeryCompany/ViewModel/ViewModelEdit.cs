using StationeryCompany.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StationeryCompany.ViewModel
{
    class ViewModelEdit : INotifyPropertyChanged
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
        public string _textProductType;
        public string TextProductType
        {
            get => _textProductType;
            set
            {
                if (_textProductType != value)
                {
                    _textProductType = value;
                    OnPropertyChanged(nameof(TextProductType));
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
        public ICommand ChangeOrEditCommand {  get; set; }
        public string originalTypeName = "";
        public ViewModelEdit(string Title, string Content, object? ID, string connection)
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
            if (TextProductType != originalTypeName)
            {
                var result = MessageBox.Show("Текст был изменен. Вы уверены, что хотите сохранить изменения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var parameters = new Dictionary<string, object>
            {
                { "@TypeID", IDproductsType },
                { "@NewTypeName", TextProductType }
            };

                    ExecuteStoredProcedureNonQuery("UpdateProductTypeName", parameters);
                    MessageBox.Show("Информация о типе продукта успешно обновлена.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
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
                SqlCommand cmd = new SqlCommand("GetProductTypeById", connect)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@TypeID", IDproductsType);

                try
                {
                    connect.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) 
                        {
                            TextProductType = reader.IsDBNull(reader.GetOrdinal("TypeName"))
                                ? "Неизвестный тип"
                                : reader.GetString(reader.GetOrdinal("TypeName"));
                            originalTypeName = TextProductType;
                        }
                        else
                        {
                            TextProductType = "Тип продукта не найден.";
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
