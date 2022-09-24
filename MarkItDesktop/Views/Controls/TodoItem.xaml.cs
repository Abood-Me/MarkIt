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

        public bool FirstLoad
        {
            get { return (bool)GetValue(FirstLoadProperty); }
            set { SetValue(FirstLoadProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FirstLoad.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirstLoadProperty =
            DependencyProperty.Register("FirstLoad", typeof(bool), typeof(TodoItem), new PropertyMetadata(false));


        public TodoItem()
        {
            InitializeComponent();
            Loaded += TodoItem_Loaded;
        }

        private void TodoItem_Loaded(object sender, RoutedEventArgs e)
        {

            // TODO : Make animation helpers to avoid this boilterplate
            if(FirstLoad)
            {
                TodoItem item = sender as TodoItem;
                Storyboard sb = new();
                DoubleAnimation animation = new(0, item.Opacity, TimeSpan.FromMilliseconds(500));
                ThicknessAnimation slideAnimation = new(new Thickness(0, item.ActualHeight, 0, -item.ActualHeight), item.Margin, TimeSpan.FromMilliseconds(500));
                slideAnimation.DecelerationRatio = 0.9f;
                animation.DecelerationRatio = 0.9f;
                Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
                Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("Margin"));
                sb.Children.Add(animation);
                sb.Children.Add(slideAnimation);
                sb.Begin(item);
                item.BringIntoView();
                FirstLoad = false;
            }
        }
    }
}
