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
    class ViewModelAddTypeProduct : INotifyPropertyChanged
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
                    OnPropertyChanged(nameof(CanAdd));
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
        public string originalTypeName = "";
        public ViewModelAddTypeProduct(string Title, string Content, string connection)
        { 
            WindowTitle = Title;
            ContentButt = Content;
            connectionString = connection;
            ChangeOrEditCommand = new DelegateCommand(Add, CanAdd);
        }

        private bool CanAdd(object obj)
        {
            return !string.IsNullOrWhiteSpace(TextProductType);
        }

        private void Add(object obj)
        {
            string procedureName = "AddProductType";
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(procedureName, connect)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@TypeName", TextProductType);

                try
                {
                    connect.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Тип товара успешно добавлен.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении типа товара: {ex.Message}");
                }
            }
            TextProductType = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
