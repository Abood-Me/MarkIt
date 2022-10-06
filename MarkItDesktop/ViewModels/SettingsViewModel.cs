using MarkIt.Common.Models;
using MarkItDesktop.Data;
using MarkItDesktop.Exceptions;
using MarkItDesktop.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace MarkItDesktop.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private const string _defaultText = "...";
        private bool _isSettingsOpen;

        private readonly ApplicationViewModel _application;
        private readonly IUserService _userService;

        private string? _errorMessage;

        public string? ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsSettingsOpen
        {
            get => _isSettingsOpen;
            set
            {
                _isSettingsOpen = value;
                OnPropertyChanged();
            }
        }

        public TextSettingViewModel UsernameViewModel { get; }
        public TextSettingViewModel FullNameViewModel { get; }
        public TextSettingViewModel EmailViewModel { get; }
        public PasswordSettingViewModel PasswordViewModel { get; }

        public ICommand CloseCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand LogoutCommand { get; }

        public SettingsViewModel(ApplicationViewModel application, IUserService userService)
        {
            _application = application ?? throw new ArgumentNullException(nameof(application));
            _userService = userService;

            CloseCommand = new RelayCommand(Close);
            OpenCommand = new RelayCommand(Open);
            LogoutCommand = new RelayCommand(Logout);

            UsernameViewModel = new TextSettingViewModel(value => UpdateSetting(new UserApiModel() {  Username = value } ) ) { CurrentText = _defaultText, Label = "Username" };
            FullNameViewModel = new TextSettingViewModel(value => UpdateSetting(new UserApiModel() { FullName = value })) { CurrentText = _defaultText, Label = "Full name" };
            EmailViewModel = new TextSettingViewModel(value => UpdateSetting(new UserApiModel() { Email = value })) { CurrentText = _defaultText, Label = "Email" };
            PasswordViewModel = new PasswordSettingViewModel( (currentPass, newPass) => UpdatePassword(currentPass, newPass)) { Label = "Current Password" };
        }


        private void Close()
        {
            IsSettingsOpen = false;
        }

        private void Open()
        {
            IsSettingsOpen = true;
        }

        private async Task<bool> UpdateSetting(UserApiModel model)
        {
            try
            {
                ErrorMessage = string.Empty;
                if (await _userService.UpdateUser(model))
                {
                    await _application.UpdateUIDataAsync();
                    return true;
                }
            }
            catch (HttpRequestException)
            {
                ErrorMessage = "Connection to server failed, try again later.";
            }
            catch (ValidationException e)
            {
                ErrorMessage = e.Message;
            }

            return false;
        }

        private async Task<bool> UpdatePassword(string currentPassword, string newPassword)
        {
            try
            {
                ErrorMessage = string.Empty;
                return await _userService.UpdateUserPassword(new UserPasswordApiModel() { CurrentPassword = currentPassword, NewPassword = newPassword });
            }
            catch (HttpRequestException)
            {
                ErrorMessage = "Connection to server failed, try again later.";
            }
            catch (ValidationException e)
            {
                ErrorMessage = e.Message;
            }

            return false;
        }

        private async void Logout()
        {
            Clear();
            await _application.LogoutAsync();
        }

        public void Update(ClientData data)
        {
            UsernameViewModel.CurrentText = data.Username;
            FullNameViewModel.CurrentText = data.FullName;
            EmailViewModel.CurrentText = data.Email;
        }

        public void Clear()
        {
            IsSettingsOpen = false;
            UsernameViewModel.CurrentText = _defaultText;
            FullNameViewModel.CurrentText = _defaultText;
            EmailViewModel.CurrentText = _defaultText;
        }
    }
}
