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
        private TodoItemViewModel? CurrentlyEditing
        {
            get => _currentlyEditing;
            set
            {
                if (_currentlyEditing == value)
                    return;

                if(_currentlyEditing is TodoItemViewModel oldVm)
                    oldVm.IsHighlighted = false;

                _currentlyEditing = value;

                if (_currentlyEditing is TodoItemViewModel newVm)
                    newVm.IsHighlighted = true;
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

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value && CurrentlyEditing is not null;
                if(_isEditing)
                {
                    Text = CurrentlyEditing!.Text;
                }
                else
                {
                    Text = string.Empty;
                    CurrentlyEditing = null;
                }

                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand ExitEditingCommand { get; }

        #endregion

        public MainViewModel(ITodoService todoService)
        {
            AddCommand = new RelayCommand(AddTodo);
            EditCommand = new RelayCommand(EditTodo);
            ExitEditingCommand = new RelayCommand(ExitEditing);
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

        private void ExitEditing()
        {
            IsEditing = false;
        }

        private void EditTodo()
        {
            if (string.IsNullOrWhiteSpace(Text) || !IsEditing)
                return;

            CurrentlyEditing!.Text = Text;
            // Should pass TodoApiModel instead
            UpdateTodo(CurrentlyEditing!);
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
                try
                {
                    _ignoreChanges = true;
                    TodoItems.Add(new(todo.Id, this)
                    {
                        Text = todo.Text,
                        IsCompleted = todo.IsCompleted,
                        NewItem = true
                    });
                }
                finally
                {
                    _ignoreChanges = false;
                }
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
        }

        public async Task DeleteTodo(TodoItemViewModel item)
        {
            if(await _todoService.DeleteTodoAsync(item.Id))
            {
                if (item == CurrentlyEditing)
                    IsEditing = false;

                TodoItems.Remove(item);
            }
        }

        public void EditTodo(TodoItemViewModel item)
        {
            CurrentlyEditing = item;
            IsEditing = true;
        }
    }
}
