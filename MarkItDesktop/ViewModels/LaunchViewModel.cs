using MarkItDesktop.Models;
using MarkItDesktop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkItDesktop.ViewModels
{
    public class LaunchViewModel : BaseViewModel
    {
        private readonly ApplicationViewModel _application;
        private readonly IClientDataStore _store;

        public LaunchViewModel(ApplicationViewModel application, IClientDataStore store)
        {
            this._application = application;
            this._store = store;
        }

        public LaunchViewModel()
        {

        }

        public override async Task OnLoaded()
        {
            await _store.EnsureCreated();

            await Task.Delay(1000);

            if (await _store.HasStoredLogin())
            {
                // TODO : Verify token via API
                _application.NavigateTo(ApplicationPage.MainPage);
                return;
            }

            _application.NavigateTo(ApplicationPage.LoginPage);
        }
    }
}
