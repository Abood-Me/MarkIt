using MarkItDesktop.Models;
using MarkItDesktop.Services;
using MarkItDesktop.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MarkItDesktop.ViewModels
{
    public class RegisterViewModel : BaseValidationViewModel
    {
        private readonly ApplicationViewModel _application;
        private readonly IAuthService _authService;

        #region Private Members

        private string? _username;
        private string? _email;
        private string? _fullName;
        private string? _password;
        private string? _confirmPassword;

        #endregion

        #region Public Properties
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username field is required")]
        [MinLength(5, ErrorMessage = "8 Minimum characters")]
        public string? Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email field is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password field is required")]
        [MinLength(8, ErrorMessage = "8 Minimum characters")]

        public string? Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password field is required")]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match")]
        public string? ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Full name field is required")]
        [RegularExpression("^([a-zA-Z]{2,}\\s[a-zA-Z]{1,}'?-?[a-zA-Z]{2,}\\s?([a-zA-Z]{1,})?)", ErrorMessage = "Valid characters include (A-Z), ( ' ) and ( - )")]
        public string? FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Commands
        public ICommand LoginCommand { get; private set; }
        public ICommand RegisterCommand { get; private set; }
        #endregion

        public RegisterViewModel(ApplicationViewModel application, IAuthService authService)
        {
            _application = application;
            _authService = authService;
            LoginCommand = new RelayCommand(GoToLogin);
            RegisterCommand = new RelayCommand(async () => await Register());
        }

        public RegisterViewModel() { }

        public async Task Register()
        {
            ValidateProperty(Username);
            ValidateProperty(Password);
            ValidateProperty(ConfirmPassword);
            ValidateProperty(Email);
            ValidateProperty(FullName);

            if (HasErrors)
                return;

            bool succeeded = await _authService.RegisterAsync(Username, Password, Email, FullName);
            // TODO: Try catch errors set error message with server response error if there is any.


            if(succeeded)
                _application.NavigateTo(ApplicationPage.LoginPage);
        }

        public void GoToLogin()
        {
            _application.NavigateTo(ApplicationPage.LoginPage);
        }
    }
}
