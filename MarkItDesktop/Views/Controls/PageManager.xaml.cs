using MarkItDesktop.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace MarkItDesktop.Views.Controls
{

    /// <summary>
    /// Interaction logic for PageManager.xaml
    /// </summary>
    public partial class PageManager : UserControl
    {
        public BasePage Page
        {
            get { return (BasePage)GetValue(PageProperty); }
            set { SetValue(PageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Page.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageProperty =
            DependencyProperty.Register("Page", typeof(BasePage), typeof(PageManager), new UIPropertyMetadata(null, PagePropertyChanged));

        private static async void PagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is null)
                return;

            PageManager manager = (PageManager)d;
            manager.CurrentFrame.Content = e.NewValue;

            if(e.OldValue is BasePage oldPage)
            {
                manager.OldFrame.Content = oldPage;
                oldPage.Loaded -= oldPage.PageLoaded;
                oldPage.AnimateOut = true;
                // Use Task Completion Source
                await Task.Delay(oldPage.AnimationDuration.TimeSpan);
                manager.OldFrame.Content = null;
            }
        }

        public PageManager()
        {
            InitializeComponent();
        }
    }
}
