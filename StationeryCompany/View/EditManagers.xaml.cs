using StationeryCompany.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
    /// Interaction logic for EditManagers.xaml
    /// </summary>
    public partial class EditManagers : Window
    {
        public EditManagers(string title, string content, object? id, string connection)
        {
            InitializeComponent();
            this.DataContext = new ViewModelEditManager(title, content, id, connection);

        }
        public EditManagers(string title, string content, string connection)
        {
            InitializeComponent();  
            this.DataContext = new ViewModelAddManager(title, content, connection);
        }
    }
}
