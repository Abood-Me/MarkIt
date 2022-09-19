using MarkItDesktop.Models;
using MarkItDesktop.Services;
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
        private readonly ApplicationViewModel _application;
        private readonly IAuthService _authService;

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
        public LoginViewModel(ApplicationViewModel application, IAuthService authService)
        {
            this._application = application;
            this._authService = authService;
            LoginCommand = new RelayComamnd(async () => await Login());
        }

        public async Task Login()
        {
            bool isValid = await _authService.LoginAsync(Username, Password);
            if(isValid)    
                _application.CurrentPage = ApplicationPage.MainPage;
            
        }
    }
}
