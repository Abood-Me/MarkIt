using MarkItDesktop.Models;
using MarkItDesktop.Services;
using MarkItDesktop.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MarkItDesktop.ViewModels
{
    public class LoginViewModel : BaseValidationViewModel
    {

        private string? _username;
        private string? _password;

        private readonly ApplicationViewModel _application;
        private readonly IAuthService _authService;
        private readonly IClientDataStore _storeService;

        [Required(AllowEmptyStrings = false,ErrorMessage = "Username field is required")]
        [MinLength(5, ErrorMessage = "Username field must have a minimum length of 5")]
        public string? Username
        {
            get => _username;
            set 
            { 
                _username = value;
                OnPropertyChanged();
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password field is required")]
        [MinLength(5, ErrorMessage = "Password field must have a minimum length of 5")]
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
        public ICommand RegisterCommand { private set; get; }

        #endregion

        public LoginViewModel(
            ApplicationViewModel application, 
            IAuthService authService,
            IClientDataStore storeService)
        {
            this._application = application;
            this._authService = authService;
            this._storeService = storeService;
            LoginCommand = new RelayCommand(async () => await Login());
            RegisterCommand = new RelayCommand(GoToRegister);
        }

        public LoginViewModel()
        {

        }

        public async Task Login()
        {
            ValidateProperty(Username);
            ValidateProperty(Password);

            if (HasErrors)
                return;

            bool isValid = await _authService.LoginAsync(Username, Password);
            if (isValid)
                _application.NavigateTo(ApplicationPage.MainPage);
        }

        public void GoToRegister()
        {
            _application.NavigateTo(ApplicationPage.RegisterPage);
        }

        public override async Task OnLoaded()
        {
            if (await _storeService.HasStoredLogin())
            {
                // TODO : Verify token via API
                _application.NavigateTo(ApplicationPage.RegisterPage);
            }
        }
    }
}
