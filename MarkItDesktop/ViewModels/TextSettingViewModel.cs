using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MarkItDesktop.ViewModels
{
    public class TextSettingViewModel : BaseValidationViewModel
    {
        private bool _isLoading;
        private bool _isEditing;
        private string? _label;
        private string? _currentText;
		private string? _editText;
		private readonly Func<string, Task<bool>> _saveFunc;

        public bool IsLoading
		{
			get => _isLoading;
			set
			{
				_isLoading = value;
				OnPropertyChanged();
			}
		}

		public bool IsEditing
		{
			get => _isEditing;
			set
			{
				_isEditing = value;
                if (_isEditing)
                {
                    EditText = CurrentText;
                }
                OnPropertyChanged();
			}
		}

		public string? Label
		{
			get => _label;
			set
			{
				_label = value;
				OnPropertyChanged();
			}
		}

		public string? CurrentText
		{
			get => _currentText;
			set
			{
				_currentText = value;
				OnPropertyChanged();
			}
		}

		[Required(AllowEmptyStrings = false, ErrorMessage = "This field cannot be empty")]
		public string? EditText
		{
			get => _editText;
			set
			{
				_editText = value;
				OnPropertyChanged();
			}
		}

		public ICommand EditCommand { get; }
		public ICommand CancelCommand { get; }
		public ICommand SaveCommand { get; }


		public TextSettingViewModel(Func<string, Task<bool>> saveFunc)
		{
			EditCommand = new RelayCommand(Edit);
			CancelCommand = new RelayCommand(Cancel);
			SaveCommand = new RelayCommand(Save);

            _saveFunc = saveFunc;
		}

		private void Edit()
		{
			IsEditing = true;
		}
		
		private void Cancel()
		{
			IsEditing = false;
		}

		private async void Save()
		{
			try
			{
				if (IsLoading)
					return;

				ValidateProperty(EditText);

				if (HasErrors)
					return;

				IsLoading = true;
				
				if(await _saveFunc(EditText!))
				{
					CurrentText = EditText;
					IsEditing = false;
				}
			}
			finally
			{
				IsLoading = false;
			}
		}

	}
}
