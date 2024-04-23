using Dapper;
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
        public int? IDproductsType;
        public ICommand ChangeOrEditCommand {  get; set; }
        public string originalTypeName = "";
        public ViewModelEdit(string Title, string Content, int? ID, string connection)
        {
            WindowTitle = Title;
            ContentButt = Content;
            IDproductsType = ID;
            connectionString = connection;
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
                    try
                    {
                        using (var connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            var parameters = new DynamicParameters();
                            parameters.Add("@TypeID", IDproductsType);
                            parameters.Add("@NewTypeName", TextProductType);

                            connection.Execute("UpdateProductTypeName", parameters, commandType: CommandType.StoredProcedure);

                            MessageBox.Show("Информация о типе продукта успешно обновлена.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при обновлении информации о типе продукта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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



        public void LoadProductType()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var typeName = connection.QueryFirstOrDefault<string>(
                        "SELECT TypeName FROM ProductTypes WHERE TypeID = @TypeID",
                        new { TypeID = IDproductsType });

                    TextProductType = string.IsNullOrEmpty(typeName) ? "Неизвестный тип" : typeName;
                    originalTypeName = TextProductType;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке типа продукта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
