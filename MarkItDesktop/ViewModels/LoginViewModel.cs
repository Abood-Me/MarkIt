using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MarkItDesktop.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { private set; get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayComamnd(Login);
        }

        public void Login()
        {
            ((ApplicationViewModel)(Application.Current.MainWindow.DataContext)).CurrentPage = Models.ApplicationPage.MainPage;
        }
    }
}
