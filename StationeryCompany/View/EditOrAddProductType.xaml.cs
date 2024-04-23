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
    /// Interaction logic for EditOrAddProductType.xaml
    /// </summary>
    public partial class EditOrAddProductType : Window
    {
        public EditOrAddProductType(string title, string Content, int? ID, string connection)
        {
            InitializeComponent();
            this.DataContext = new ViewModelEdit(title, Content, ID, connection);
        }
        public EditOrAddProductType(string title, string Content, string connection)
        {
            InitializeComponent();
            this.DataContext = new ViewModelAddTypeProduct(title, Content, connection);
        }
    }
}
