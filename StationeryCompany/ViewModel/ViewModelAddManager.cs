﻿using StationeryCompany.Commands;
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
    class ViewModelAddManager : INotifyPropertyChanged
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
                    OnPropertyChanged(nameof(CanAdd));
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
        public ICommand ChangeOrEditCommand { get; set; }
        public string originalTypeName = "";
        public string originalPhone = "";
        public ViewModelAddManager(string Title, string Content, string connection) 
        {
            WindowTitle = Title;
            ContentButt = Content;
            connectionString = connection;
            ChangeOrEditCommand = new DelegateCommand(Add, CanAdd);
        }

        private bool CanAdd(object obj)
        {
            return !string.IsNullOrWhiteSpace(NameManader) && !string.IsNullOrWhiteSpace(Phone);
        }

        private void Add(object obj)
        {
            string procedureName = "AddSalesManager";
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(procedureName, connect)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@ManagerName", NameManader);
                cmd.Parameters.AddWithValue("@PhoneNumber", Phone);

                try
                {
                    connect.Open();
                    cmd.ExecuteNonQuery(); 
                    MessageBox.Show("Менеджер успешно добавлен.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении менеджера: {ex.Message}");
                }
            }
            NameManader = "";
            Phone = "";

        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}