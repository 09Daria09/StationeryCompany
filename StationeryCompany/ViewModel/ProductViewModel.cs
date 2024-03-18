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
    public class ProductViewModel : INotifyPropertyChanged
    {
        private Product _product;

        public ProductViewModel(Product product)
        {
            _product = product;
        }

        public int ProductId => _product.ProductId;

        public string ProductName
        {
            get => _product.ProductName;
            set
            {
                if (_product.ProductName != value)
                {
                    _product.ProductName = value;
                    OnPropertyChanged(nameof(ProductName));
                }
            }
        }

        public int? TypeId => _product.TypeId;

        public string TypeName => _product.Type?.TypeName;

        public int? Quantity
        {
            get => _product.Quantity;
            set
            {
                if (_product.Quantity != value)
                {
                    _product.Quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public decimal? Cost
        {
            get => _product.Cost;
            set
            {
                if (_product.Cost != value)
                {
                    _product.Cost = value;
                    OnPropertyChanged(nameof(Cost));
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

