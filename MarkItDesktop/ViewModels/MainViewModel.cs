using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MarkItDesktop.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<TodoItemViewModel> TodoItems { get; set; } = new();

        #region Private members

        private string _text;

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


        #endregion

        #region Commands
        
        public ICommand AddCommand { get; private set; }

        #endregion

        public MainViewModel()
        {

            AddCommand = new RelayComamnd(AddTodo);
        }

        public void AddTodo()
        {
            if (string.IsNullOrWhiteSpace(Text))
                return;

            TodoItems.Add(
                    new TodoItemViewModel
                    {
                        Text = Text
                    }
                );

            Text = string.Empty;
        }
    }
}
