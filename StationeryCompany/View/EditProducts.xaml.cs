using StationeryCompany.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StationeryCompany.View
{
    /// <summary>
    /// Interaction logic for EditProducts.xaml
    /// </summary>
    public partial class EditProducts : Window
    {
        public EditProducts(string title, string Content, int? ID)
        {
            InitializeComponent();
            this.DataContext = new ViewModelEditProducts(title, Content, ID);
        }
        public EditProducts(string title, string Content)
        {
            InitializeComponent();
            this.DataContext = new ViewModelAddProducts(title, Content);
        }
    }
}
