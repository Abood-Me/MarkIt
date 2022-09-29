using MarkItDesktop.Exceptions;
using MarkItDesktop.Models;
using MarkItDesktop.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MarkItDesktop.ViewModels
{
    public class LoginViewModel : BaseValidationViewModel
    {

        #region Private Members

        private string? _username;
        private string? _password;
        private string? _errorMessage;
        private bool _isLoading;

        private readonly ApplicationViewModel _application;
        private readonly IAuthService _authService;
        private readonly IClientDataStore _storeService;

        #endregion

        #region Public Members

        [Required(AllowEmptyStrings = false, ErrorMessage = "Username field is required")]
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

        public string? ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsIdle));
            }
        }

        public bool IsIdle => !IsLoading;

        #endregion

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
            ErrorMessage = string.Empty;

            if (HasErrors)
                return;
            
            try
            {
                IsLoading = true;
                if(await _authService.LoginAsync(Username, Password))
                {
                    _application.NavigateTo(ApplicationPage.MainPage);
                }
                
            }
            catch(HttpRequestException)
            {
                ErrorMessage = "Connection to server failed. Try again later.";
            }
            catch(AuthenticationException e)
            {
                ErrorMessage = e.Message;
            }
            finally
            {
                IsLoading = false;
            }

        }

        public void GoToRegister()
        {
            _application.NavigateTo(ApplicationPage.RegisterPage);
        }

    }
}
