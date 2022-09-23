using MarkItDesktop.Models;
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

		public ApplicationPage CurrentPage
		{
			get => _currentPage;
			private set
			{
				_currentPage = value;
				OnPropertyChanged();
			}
		}

		public ApplicationViewModel()
		{
			CurrentPage = ApplicationPage.LoginPage;
		}


		public void NavigateTo(ApplicationPage page)
		{
			CurrentPage = page;
		}
	}
}
