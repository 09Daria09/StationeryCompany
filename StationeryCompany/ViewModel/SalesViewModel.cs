using StationeryCompany.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationeryCompany.ViewModel
{
    class SalesViewModel : INotifyPropertyChanged
    {
        private Sale _sale;

        public SalesViewModel(Sale sale) 
        {
            _sale = sale;
        }

        public int SaleId
        {
            get => _sale.SaleId;
            set
            {
                if (_sale.SaleId != value)
                {
                    _sale.SaleId = value;
                    OnPropertyChanged(nameof(SaleId));
                }
            }
        }

        public int? ProductId
        {
            get => _sale.ProductId;
            set
            {
                if (_sale.ProductId != value)
                {
                    _sale.ProductId = value;
                    OnPropertyChanged(nameof(ProductId));
                }
            }
        }

        public int? ManagerId
        {
            get => _sale.ManagerId;
            set
            {
                if (_sale.ManagerId != value)
                {
                    _sale.ManagerId = value;
                    OnPropertyChanged(nameof(ManagerId));
                }
            }
        }

        public int? CompanyId
        {
            get => _sale.CompanyId;
            set
            {
                if (_sale.CompanyId != value)
                {
                    _sale.CompanyId = value;
                    OnPropertyChanged(nameof(CompanyId));
                }
            }
        }

        public int? QuantitySold
        {
            get => _sale.QuantitySold;
            set
            {
                if (_sale.QuantitySold != value)
                {
                    _sale.QuantitySold = value;
                    OnPropertyChanged(nameof(QuantitySold));
                }
            }
        }

        public decimal? PricePerUnit
        {
            get => _sale.PricePerUnit;
            set
            {
                if (_sale.PricePerUnit != value)
                {
                    _sale.PricePerUnit = value;
                    OnPropertyChanged(nameof(PricePerUnit));
                }
            }
        }

        public DateTime? SaleDate
        {
            get => _sale.SaleDate;
            set
            {
                if (_sale.SaleDate != value)
                {
                    _sale.SaleDate = value;
                    OnPropertyChanged(nameof(SaleDate));
                }
            }
        }

        public string CompanyName => _sale.Company?.CompanyName;
        public string ManagerName => _sale.Manager?.ManagerName;
        public string ProductName => _sale.Product?.ProductName;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
