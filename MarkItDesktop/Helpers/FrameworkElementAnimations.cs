using MarkItDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MarkItDesktop.Helpers
{
    public static class FrameworkElementAnimations
    {
        public static async Task ScaleAndFadeIn(this FrameworkElement element, Duration duration)
        {
            Storyboard sb = new();

            element.RenderTransform = new ScaleTransform(1, 1);
            element.RenderTransformOrigin = new Point(0.5, 0.5);

            sb.AddFadeIn(duration, true)
              .AddScaleUp(duration);

            element.Visibility = Visibility.Visible;

            sb.Begin(element);

            await Task.Delay(duration.TimeSpan);
        }

        public static async Task ScaleAndFadeOut(this FrameworkElement element, Duration duration)
        {
            Storyboard sb = new();

            element.RenderTransform = new ScaleTransform(1, 1);
            element.RenderTransformOrigin = new Point(0.5, 0.5);

            sb.AddFadeOut(duration, true)
              .AddScaleUp(duration, 1, 1.8f);
            element.Visibility = Visibility.Visible;

            sb.Begin(element);
            
            await Task.Delay(duration.TimeSpan);

            element.Visibility = Visibility.Hidden;
        }

        public static async Task SlideAndFadeIn(this FrameworkElement element, Duration duration)
        {
            Storyboard sb = new();


            sb.AddFadeIn(duration, true)
              .AddSlideIn(duration, SlideDirection.Bottom, element.ActualHeight);

            element.Visibility = Visibility.Visible;

            sb.Begin(element);

            await Task.Delay(duration.TimeSpan);
        }

    }
}
