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
    class ProductTypeViewModel : INotifyPropertyChanged
    {
        private ProductType _productType;

        public ProductTypeViewModel(ProductType productType)
        {
            _productType = productType;
            Products = new ObservableCollection<ProductViewModel>(
                productType.Products.Select(p => new ProductViewModel(p)));
        }

        public int TypeId
        {
            get => _productType.TypeId;
            set
            {
                if (_productType.TypeId != value)
                {
                    _productType.TypeId = value;
                    OnPropertyChanged(nameof(TypeId));
                }
            }
        }

        public string TypeName
        {
            get => _productType.TypeName;
            set
            {
                if (_productType.TypeName != value)
                {
                    _productType.TypeName = value;
                    OnPropertyChanged(nameof(TypeName));
                }
            }
        }

        public ObservableCollection<ProductViewModel> Products { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    }
