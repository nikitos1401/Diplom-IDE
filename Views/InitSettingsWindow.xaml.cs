using Diplom.Properties;
using Diplom.ViewModels;
using System.Windows;

namespace Diplom.Views
{
    public partial class InitSettingsWindow : Window
    {
        public InitSettingsWindow()
        {
            if (Settings.Default.AreaId == -1 && Settings.Default.AreaItemId == -1)
            {
                InitializeComponent();
                DataContext = new InitSettingsViewModel();
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
