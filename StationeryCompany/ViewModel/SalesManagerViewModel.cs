using StationeryCompany.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationeryCompany.ViewModel
{
    class SalesManagerViewModel : INotifyPropertyChanged 
    {
        private SalesManager _salesManager;

        public SalesManagerViewModel(SalesManager salesManager)
        {
            _salesManager = salesManager;
            Sales = new ObservableCollection<SalesViewModel>(salesManager.Sales.Select(s => new SalesViewModel(s)));
        }

        public int ManagerId
        {
            get => _salesManager.ManagerId;
            set
            {
                if (_salesManager.ManagerId != value)
                {
                    _salesManager.ManagerId = value;
                    OnPropertyChanged(nameof(ManagerId));
                }
            }
        }

        public string ManagerName
        {
            get => _salesManager.ManagerName;
            set
            {
                if (_salesManager.ManagerName != value)
                {
                    _salesManager.ManagerName = value;
                    OnPropertyChanged(nameof(ManagerName));
                }
            }
        }

        public string PhoneNumber
        {
            get => _salesManager.PhoneNumber;
            set
            {
                if (_salesManager.PhoneNumber != value)
                {
                    _salesManager.PhoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        public ObservableCollection<SalesViewModel> Sales { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
