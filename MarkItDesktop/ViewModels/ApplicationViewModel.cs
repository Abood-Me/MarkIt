using MarkItDesktop.Data;
using MarkItDesktop.Models;
using MarkItDesktop.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace MarkItDesktop.ViewModels
{
	public class ApplicationViewModel : BaseViewModel
	{
		private ApplicationPage _currentPage;

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

			ClientData? userData = await _clientStore.GetStoredLoginAsync();
			if (userData is null)
				return;

			SideMenuViewModel sideMenuViewModel = App.AppHost.Services.GetRequiredService<SideMenuViewModel>();
			SettingsViewModel settingsViewModel = App.AppHost.Services.GetRequiredService<SettingsViewModel>();

			sideMenuViewModel.Update(userData);
			settingsViewModel.Update(userData);
		}

		public async Task LogoutAsync()
		{
			SideMenuViewModel sideMenuViewModel = App.AppHost.Services.GetRequiredService<SideMenuViewModel>();
			await _clientStore.ClearAllStoredLoginsAsync();
			sideMenuViewModel.Clear();

			NavigateTo(ApplicationPage.LoginPage);
		}
	}
}
