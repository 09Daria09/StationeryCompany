using StationeryCompany.ViewModel;
using System.Configuration;
using System.Data;
using System.Net.NetworkInformation;
using System.Windows;

namespace StationeryCompany
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
        public partial class App : Application
        {
            private void OnStartup(object sender, StartupEventArgs e)
            {
                MainWindow view = new MainWindow();
                ViewModelStationery viewModel = new ViewModelStationery();
                view.DataContext = viewModel;
                view.Show();
            }
        }

}
