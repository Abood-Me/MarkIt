using MarkIt.Common.Models;
using MarkItDesktop.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MarkItDesktop.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<TodoItemViewModel> TodoItems { get; set; } = new();

        #region Private members

        private string _text = string.Empty;
        private bool _isEditing;
        private bool _ignoreChanges;
        private TodoItemViewModel? _currentlyEditing;

        private readonly ITodoService _todoService;

        #endregion

        #region Public properties

        public string Text
        {
            get => _text;
            set 
            { 
                _text = value;
                OnPropertyChanged();
            }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                
                _isEditing = value && _currentlyEditing is not null;
                if(_isEditing)
                {
                    Text = _currentlyEditing!.Text;
                }
                else
                {
                    Text = string.Empty;
                    _currentlyEditing = null;
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }

        #endregion

        public MainViewModel(ITodoService todoService)
        {
            AddCommand = new RelayCommand(AddTodo);
            EditCommand = new RelayCommand(EditTodo);
            _todoService = todoService;
        }
        public MainViewModel()
        {

        }

        public override async Task OnLoaded()
        {
            await LoadItems();
        }

        async Task LoadItems()
        {
            IList<TodoResponseModel>? items = await _todoService.GetTodosAsync();
            if (items is not null)
            {
                if(!_ignoreChanges)
                {
                    _ignoreChanges = true;
                    TodoItems.Clear();
                    foreach (TodoResponseModel todo in items)
                    {
                        TodoItems.Add(new(todo.Id, this)
                        {
                            Text = todo.Text,
                            IsCompleted = todo.IsCompleted
                        });
                    }
                    _ignoreChanges = false;
                }
            }
        }

        private void EditTodo()
        {

            if (string.IsNullOrWhiteSpace(Text) || !IsEditing)
                return;

            _currentlyEditing!.Text = Text;
            // Should pass TodoApiModel instead
            UpdateTodo(_currentlyEditing!);
            IsEditing = false;
        }

        private async void AddTodo()
        {
            if (string.IsNullOrWhiteSpace(Text) || IsEditing)
                return;

            TodoResponseModel? todo = await _todoService.CreateTodoAsync(
                new TodoApiModel()
                {
                    Text = Text,
                    IsCompleted = false
                });

            Text = string.Empty;

            if (todo is null)
                return;

            if (!_ignoreChanges)
            {
                _ignoreChanges = true;
                TodoItems.Add(new(todo.Id, this)
                {
                    Text = todo.Text,
                    IsCompleted = todo.IsCompleted,
                    NewItem = true
                });
                _ignoreChanges = false;
            }
        }
        public async void UpdateTodo(TodoItemViewModel item)
        {
            if (_ignoreChanges) return;
            await _todoService.UpdateTodoAsync( item.Id,
                new TodoApiModel()
                {
                    IsCompleted = item.IsCompleted,
                    Text = item.Text
                });

            // TODO: Update viewmodel here
        }

        public async Task DeleteTodo(TodoItemViewModel item)
        {
            if(await _todoService.DeleteTodoAsync(item.Id))
            {
                if (item == _currentlyEditing)
                    IsEditing = false;

                TodoItems.Remove(item);
            }
        }

        public void EditTodo(TodoItemViewModel item)
        {
            _currentlyEditing = item;
            IsEditing = true;
        }
    }
}
