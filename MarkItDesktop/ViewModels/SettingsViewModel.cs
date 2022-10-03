using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MarkItDesktop.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private bool _isSettingsOpen;

        private readonly ApplicationViewModel _application;

        public bool IsSettingsOpen
        {
            get => _isSettingsOpen;
            set
            {
                _isSettingsOpen = value;
                OnPropertyChanged();
            }
        }

        public ICommand CloseCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }

        public SettingsViewModel(ApplicationViewModel application)
        {
            _application = application ?? throw new ArgumentNullException(nameof(application));

            CloseCommand = new RelayCommand(Close);
            OpenCommand = new RelayCommand(Open);
            LogoutCommand = new RelayCommand(Logout);
        }

        private void Close()
        {
            IsSettingsOpen = false;
        }

        private void Open()
        {
            IsSettingsOpen = true;
        }

        private async void Logout()
        {
            IsSettingsOpen = false;
            await _application.LogoutAsync();
        }
    }
}
