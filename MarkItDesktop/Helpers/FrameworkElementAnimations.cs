using MarkItDesktop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            element.Visibility = Visibility.Collapsed;
        }

        public static async Task SlideAndFadeInAsync(this FrameworkElement element, Duration duration, SlideDirection direction = SlideDirection.Bottom, double? offset = null, bool hasConstraints = true)
        {

            Storyboard sb = new();

            element.Visibility = Visibility.Visible;
            double d = offset ?? direction switch
            {
                SlideDirection.Bottom or SlideDirection.Top => element.ActualHeight,
                SlideDirection.Left or SlideDirection.Right => element.ActualWidth,
                _ => 0
            };

            sb.AddFadeIn(duration, true)
              .AddSlideIn(duration, direction, d, hasConstraints: hasConstraints);


            sb.Begin(element);
            await Task.Delay(duration.TimeSpan);
        }

        public static async Task SlideAndFadeOutAsync(this FrameworkElement element, Duration duration, SlideDirection direction = SlideDirection.Bottom, double? offset = null, bool hasConstraints = true)
        {
            Storyboard sb = new();

            element.Visibility = Visibility.Visible;
            double d = offset ?? direction switch
            {
                SlideDirection.Bottom or SlideDirection.Top => element.ActualHeight,
                SlideDirection.Left or SlideDirection.Right => element.ActualWidth,
                _ => 0
            };


            sb.AddFadeOut(duration, true)
              .AddSlideOut(duration, direction, d, hasConstraints: hasConstraints);

            // Refactor this later so that it uses Task Completion Source and can cancel if another animation started ( Don't collapse if cancelled )
            //sb.Completed += (_, _) => Debug.WriteLine("Completed");
            sb.Begin(element);
            await Task.Delay(duration.TimeSpan);
            // Check if another animation is playing on that element then doesn't collapse

            if(element.Opacity == 0)
                element.Visibility = Visibility.Collapsed;
        }

    }
}
