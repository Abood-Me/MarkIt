using MarkItDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace MarkItDesktop.Helpers
{
    public static class StoryboardHelpers
    {
        #region Fade Animations
        public static Storyboard AddFadeIn(this Storyboard storyboard, Duration duration, bool fromStart = false, float decelerationRatio = 0.9f)
        {
            DoubleAnimation animation = new()
            {
                DecelerationRatio = decelerationRatio,
                Duration = duration,
                To = 1,
            };

            if (fromStart)
                animation.From = 0;

            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

            storyboard.Children.Add(animation);

            return storyboard;
        }
        public static Storyboard AddFadeOut(this Storyboard storyboard, Duration duration, bool fromStart = false, float decelerationRatio = 0.9f)
        {
            DoubleAnimation animation = new()
            {
                DecelerationRatio = decelerationRatio,
                Duration = duration,
                To = 0,
            };

            if (fromStart)
                animation.From = 1;

            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

            storyboard.Children.Add(animation);
            return storyboard;
        }
        #endregion

        #region Scale animations
        public static Storyboard AddScaleUp(this Storyboard storyboard, Duration duration, float from = 0.8f, float to = 1, float decelerationRatio = 0.9f)
        {

            DoubleAnimation animationX = new()
            {
                DecelerationRatio = decelerationRatio,
                Duration = duration,
                To = to,
                From = from
            };

            DoubleAnimation animationY = new()
            {
                DecelerationRatio = decelerationRatio,
                Duration = duration,
                To = to,
                From = from
            };


            Storyboard.SetTargetProperty(animationX, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTargetProperty(animationY, new PropertyPath("RenderTransform.ScaleY"));

            storyboard.Children.Add(animationX);
            storyboard.Children.Add(animationY);

            return storyboard;
        }

        #endregion
        public static Storyboard AddSlideIn(this Storyboard storyboard, Duration duration, SlideDirection direction, double offset, float decelerationRatio = 0.9f, bool hasConstraints = true)
        {
            Thickness from = direction switch
            {
                SlideDirection.Left => new Thickness(-offset, 0, hasConstraints ? 0 : offset, 0),
                SlideDirection.Right => new Thickness(hasConstraints ? 0 : offset, 0, -offset, 0),
                SlideDirection.Top => new Thickness(0, -offset, 0, hasConstraints ? 0 : offset),
                SlideDirection.Bottom => new Thickness(0, hasConstraints ? 0 : offset, 0, -offset),
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };

            ThicknessAnimation animation = new()
            {
                DecelerationRatio = decelerationRatio,
                Duration = duration,
                From = from,
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
            storyboard.Children.Add(animation);

            return storyboard;
        }

        public static Storyboard AddSlideOut(this Storyboard storyboard, Duration duration, SlideDirection direction, double offset, float decelerationRatio = 0.9f, bool hasConstraints = true)
        {
            Thickness to = direction switch
            {
                SlideDirection.Left => new Thickness(-offset, 0, hasConstraints ? 0 : offset, 0),
                SlideDirection.Right => new Thickness(hasConstraints ? 0 : offset, 0, -offset, 0),
                SlideDirection.Top => new Thickness(0, -offset, 0, hasConstraints ? 0 : offset),
                SlideDirection.Bottom => new Thickness(0, hasConstraints ? 0 : offset, 0, -offset),
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };

            ThicknessAnimation animation = new()
            {
                DecelerationRatio = decelerationRatio,
                Duration = duration,
                To = to
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));
            storyboard.Children.Add(animation);

            return storyboard;
        }
    }
}
