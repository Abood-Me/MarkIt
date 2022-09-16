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
			set
			{
				_currentPage = value;
				OnPropertyChanged();
			}
		}

		public ApplicationViewModel()
		{
			CurrentPage = ApplicationPage.LoginPage;
		}

	}
}
