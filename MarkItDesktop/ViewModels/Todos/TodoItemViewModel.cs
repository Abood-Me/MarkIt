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
		
		private string _text;
		private bool _isCompleted;

		#endregion

		#region Public Properties
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
			}
		} 

		#endregion



	}
}
