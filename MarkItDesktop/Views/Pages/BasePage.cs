using MarkItDesktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MarkItDesktop.Views
{

    public class BasePage<TViewModel> : UserControl
        where TViewModel : BaseViewModel, new()
    {
        #region Private Members
        private TViewModel _viewModel;
        #endregion

        #region Public Properties

        public TViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel == value)
                    return;

                _viewModel = value;
                DataContext = _viewModel;
            }
        }

        #endregion

        public BasePage()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                ViewModel = new TViewModel();
            }
            else
            {
                ViewModel = App.AppHost.Services.GetRequiredService<TViewModel>();
            }
        }
    }
}
