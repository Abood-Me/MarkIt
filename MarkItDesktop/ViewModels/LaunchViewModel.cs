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
        private readonly IAuthService _authService;

        public LaunchViewModel(ApplicationViewModel application, IClientDataStore store, IAuthService authService)
        {
            this._application = application;
            this._store = store;
            this._authService = authService;
        }

        public LaunchViewModel()
        {

        }

        public override async Task OnLoaded()
        {
            await Task.Run(_store.EnsureCreatedAsync);
            try
            {
                await Task.Delay(2000);
                if (await _authService.VerifyLogin())
                {
                    _application.NavigateTo(ApplicationPage.MainPage);
                    return;
                }
            }
            catch { }
            
            _application.NavigateTo(ApplicationPage.LoginPage);

        }
    }
}
