using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MarkItDesktop.ViewModels
{
    public class PasswordSettingViewModel : BaseValidationViewModel
    {
        private bool _isLoading;
        private bool _isEditing;
        private string? _label;
        private string? _currentPassword;
        private string? _newPassword;
        private string? _confirmPassword;

        private readonly Func<string, string, Task<bool>> _saveFunc;

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
				if(!_isEditing)
				{
                    CurrentPassword = string.Empty;
                    NewPassword = string.Empty;
                    ConfirmPassword = string.Empty;
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

		[Required]
		public string? CurrentPassword
		{
			get => _currentPassword;
			set
			{
				_currentPassword = value;
				OnPropertyChanged();
			}
		}

		[Required]
		[MinLength(8)]
		public string? NewPassword
		{
			get => _newPassword;
			set
			{
				_newPassword = value;
				OnPropertyChanged();
			}
		}

		[Required]
		[Compare(nameof(NewPassword), ErrorMessage = "Password doesn't match")]
		public string? ConfirmPassword
		{
			get => _confirmPassword;
			set
			{
				_confirmPassword = value;
				OnPropertyChanged();
			}
		}


		public ICommand EditCommand { get; }
		public ICommand CancelCommand { get; }
		public ICommand SaveCommand { get; }


		public PasswordSettingViewModel(Func<string, string, Task<bool>> saveFunc)
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

                ValidateProperty(ConfirmPassword);
                ValidateProperty(NewPassword);
                ValidateProperty(ConfirmPassword);

				if (HasErrors)
					return;

				IsLoading = true;
				
				if(await _saveFunc(CurrentPassword!, NewPassword!))
				{
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
