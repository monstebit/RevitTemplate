using System.Windows;

namespace MVVM
{
    public partial class MainWindow : Window
    {
        public MainWindow(ApplicationViewModel viewModel)
        {
            InitializeComponent();
            DataContext = new ApplicationViewModel();
        }
    }
}