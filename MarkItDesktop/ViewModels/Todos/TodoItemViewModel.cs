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
		private bool _firstLoad;
		private readonly MainViewModel _mainViewModel;

		#endregion

		#region Public Properties
		public int Id { get; }

		public bool FirstLoad
		{
			get => _firstLoad;
			set
			{
				_firstLoad = value;
				OnPropertyChanged();
			}
		}


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
				_mainViewModel.UpdateTodo(this);
					// TODO: Sync all todos every now and then by marking changed ones as dirty
			}
		}

		#endregion
		public TodoItemViewModel(int id, MainViewModel mainViewModel)
		{
			Id = id;
			_mainViewModel = mainViewModel;
		}


	}
}
