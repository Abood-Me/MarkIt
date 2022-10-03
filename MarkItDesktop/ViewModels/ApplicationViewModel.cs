using MarkItDesktop.Data;
using MarkItDesktop.Models;
using MarkItDesktop.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MarkItDesktop.ViewModels
{
	public class ApplicationViewModel : BaseViewModel
    {
		private ApplicationPage _currentPage;
        private ClientData? _loggedUser;

        private readonly IClientDataStore _clientStore;

		public ApplicationPage CurrentPage
		{
			get => _currentPage;
			private set
			{
				_currentPage = value;
				OnPropertyChanged();

            }
        }

        public ApplicationViewModel(IClientDataStore clientStore)
		{
			this._clientStore = clientStore ?? throw new ArgumentNullException(nameof(clientStore));
			CurrentPage = ApplicationPage.LaunchPage;
		}


		public void NavigateTo(ApplicationPage page)
		{
			CurrentPage = page;
		}


		public async Task UpdateUIDataAsync()
		{
			SideMenuViewModel vm = App.AppHost.Services.GetRequiredService<SideMenuViewModel>();
			ClientData? userData = await _clientStore.GetStoredLoginAsync();
			if (userData is null)
				return;

            vm.IsMenuOpen = true;
			vm.Username = userData.Username;
			vm.FullName = userData.FullName;
        }
    }
}
