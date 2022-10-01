using MarkItDesktop.Helpers;
using MarkItDesktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MarkItDesktop.Views
{
    public class BasePage : UserControl
    {
        private BaseViewModel? _viewModel;
        protected BaseViewModel? ViewModelObject
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

        public bool AnimateOut { get; set; }

        public Duration AnimationDuration { get; set; } = TimeSpan.FromMilliseconds(500);


        public BasePage(BaseViewModel? viewModel = null)
        {
            ViewModelObject = viewModel;
            Loaded += PageLoaded;
            Loaded += OnPageAnimate;
        }

        public async void OnPageAnimate(object sender, RoutedEventArgs e)
        {
            BasePage page = (BasePage)sender;
            if(AnimateOut)
                // TODO : Switch on page animation
                await page.ScaleAndFadeOut(AnimationDuration);
            else
                await page.ScaleAndFadeIn(AnimationDuration);

        }

        public virtual async void PageLoaded(object sender, RoutedEventArgs e)  {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            if(ViewModelObject is not null)
                await ViewModelObject.OnLoaded();
        }
    }
    public class BasePage<TViewModel> : BasePage
        where TViewModel : BaseViewModel, new()
    {
        #region Public Properties

        public TViewModel ViewModel
        {
            get => (TViewModel?)ViewModelObject;
            set => ViewModelObject = value;
        }

        #endregion

        public BasePage() : base()
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
