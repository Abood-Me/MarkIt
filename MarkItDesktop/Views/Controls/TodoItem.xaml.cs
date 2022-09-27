using MarkItDesktop.Helpers;
using System;
using System.Collections.Generic;
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

namespace MarkItDesktop.Views
{
    /// <summary>
    /// Interaction logic for TodoItem.xaml
    /// </summary>
    public partial class TodoItem : UserControl
    {



        public bool IsNew
        {
            get { return (bool)GetValue(IsNewProperty); }
            set { SetValue(IsNewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsNew.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsNewProperty =
            DependencyProperty.Register("IsNew", typeof(bool), typeof(TodoItem), new PropertyMetadata(false, OnNewValueChanged));



        public TodoItem()
        {
            InitializeComponent();
        }

        private static void OnNewValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if((bool)e.NewValue)
            {
                FrameworkElement element = d as FrameworkElement;
                RoutedEventHandler loaded = null;
                loaded = async (_, _) =>
                {
                    element.BringIntoView();
                    await element.SlideAndFadeIn(TimeSpan.FromMilliseconds(250));
                    element.Loaded -= loaded;
                };

                element.Loaded += loaded;
                
            }
        }
    }
}
