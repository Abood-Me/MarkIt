using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MarkItDesktop.ViewModels
{
    public class TodoItemViewModel : BaseViewModel
    {
		#region Private Members

		private string _text = string.Empty;
		private bool _isCompleted;
		private bool _isLoaded = false;

        #endregion

        #region Public Properties
        public int Id { get; }


        public string Text
		{
			get => _text;
			set
			{
				_text = value;
				OnPropertyChanged();
			}
		}


		public bool IsCompleted
        {
			get => _isCompleted;
			set
			{
				_isCompleted = value;
				OnPropertyChanged();
				if(_isLoaded)
				{
					MainViewModel viewModel = App.AppHost.Services.GetRequiredService<MainViewModel>();
					viewModel.UpdateTodo(this);
					// TODO:Sync all todos every now and then by marking changed ones as dirty
				}
				else
				{
					_isLoaded = true;
				}
			}
		}

		#endregion
		public TodoItemViewModel(int id)
		{
			Id = id;
		}


	}
}
