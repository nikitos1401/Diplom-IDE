using Diplom.ViewModels;
using System.Windows;

namespace Diplom.Views
{
    public partial class AnalyticWindow : Window
    {
        public AnalyticWindow()
        {
            InitializeComponent();
            DataContext = new AnalyticViewModel();
        }
    }
}
