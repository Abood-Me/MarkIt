using MarkItDesktop.ViewModels;
using System.Windows.Controls;

namespace MarkItDesktop.Views
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : BasePage<LoginViewModel>
    {
        public LoginPage()
        {
            InitializeComponent();

            Loaded += LoginPage_Loaded;
        }

        private async void LoginPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await ViewModel.OnLoaded();
        }
    }
}
