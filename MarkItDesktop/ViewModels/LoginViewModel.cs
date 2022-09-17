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

        private string? _username;

        public string? Username
        {
            get => _username;
            set 
            { 
                _username = value;
                OnPropertyChanged();
            }
        }

        private string? _password;

        public string? Password
        {
            get => _password;
            set 
            { 
                _password = value;
                OnPropertyChanged();
            }
        }




        #region Comamnds

        public ICommand LoginCommand { private set; get; }

        #endregion
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
