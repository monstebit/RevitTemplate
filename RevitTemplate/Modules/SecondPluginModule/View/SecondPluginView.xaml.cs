using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Core.Utils;
using Modules.SecondPluginModule.ExternalCommands;
using Modules.SecondPluginModule.ViewModel;

namespace Modules.SecondPluginModule.View
{
    public partial class SecondPluginView : Window
    {
        private readonly Regex _regex = new Regex("[^0-9]+");

        //private SecondPluginViewModel _viewModel;

        public SecondPluginView(SecondPluginViewModel viewModel)
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
            var module = SecondPluginModule1.GetInstance();
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
    }
}
