using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Core.Utils;
using Modules.ModalWindowModule.ExternalCommands;
using Modules.ModalWindowModule.ViewModel;

namespace Modules.ModalWindowModule.View
{
    public partial class ModalWindowView : Window
    {
        private readonly Regex _regex = new Regex("[^0-9]+");
        public ModalWindowView(ModalWindowViewModel viewModel)
        {
            InitializeComponent();
            InitializeUiLang();
            DataContext = viewModel;
        }

        private void InitializeUiLang()
        {
            Title = LangPackUtil.GetLanguageResources().GetString("tittlePanel", LangPackUtil.GetCultureInfo());
            TextBlock1.Text = LangPackUtil.GetLanguageResources().GetString("textBlock1", LangPackUtil.GetCultureInfo());
        }

        private void ButtonClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            var module = ModalWindowModule.GetInstance();
            module.CloseControls();
        }

        private void DragMoveWindow(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonMinimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _regex.IsMatch(e.Text);
        }

        private void DataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
