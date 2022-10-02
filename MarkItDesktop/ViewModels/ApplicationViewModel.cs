using MarkItDesktop.Data;
using MarkItDesktop.Models;
using MarkItDesktop.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                OnPropertyChanged(nameof(IsMenuOpen));

            }
        }

		public ClientData? LoggedUser
		{
			get => _loggedUser;
			set
			{
                _loggedUser = value;
				OnPropertyChanged();
			}
		}


		public bool IsMenuOpen => CurrentPage == ApplicationPage.MainPage;

		public ApplicationViewModel(IClientDataStore clientStore)
		{
			CurrentPage = ApplicationPage.LaunchPage;
			this._clientStore = clientStore ?? throw new ArgumentNullException(nameof(clientStore));
		}


		public void NavigateTo(ApplicationPage page)
		{
			CurrentPage = page;
		}

		public async Task UpdateUserInfoAsync()
		{
			LoggedUser = await _clientStore.GetStoredLoginAsync();
		}

	}
}
