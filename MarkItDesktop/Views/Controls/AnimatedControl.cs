using MarkItDesktop.Helpers;
using MarkItDesktop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MarkItDesktop.Views.Controls
{
    public class AnimatedControl : UserControl
    {
        private bool _animateOnLoad = false;
        private bool _firstSetup = true;
        public bool Animate
        {
            get { return (bool)GetValue(AnimateProperty); }
            set { SetValue(AnimateProperty, value); }
        }

        public SlideDirection Direction
        {
            get { return (SlideDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty AnimateProperty =
            DependencyProperty.Register("Animate", typeof(bool), typeof(AnimatedControl), new UIPropertyMetadata(false, (_,_) => { }, OnAnimateCoerce));

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(SlideDirection), typeof(AnimatedControl), new PropertyMetadata(SlideDirection.Bottom));


        private static object OnAnimateCoerce(DependencyObject d, object e)
        {
            AnimatedControl control = (AnimatedControl)d;
            bool value = (bool)e;
            if ((bool)d.GetValue(AnimateProperty) == value && control.IsLoaded)
                return e;

            if (control.IsLoaded)
                control.AnimateElement(value, false);
            else
            {
                control._animateOnLoad = value;
                if (control._firstSetup)
                {
                    control._firstSetup = false;
                    control.Loaded += ( _, _ ) =>
                    {
                        control.AnimateElement(control._animateOnLoad, true);
                    };
                }
            }
            return e;
        }

        async void AnimateElement(bool value, bool firstLoad)
        {
            Duration duration = TimeSpan.FromMilliseconds(500);

            if (value)
                await this.SlideAndFadeInAsync(firstLoad ? TimeSpan.Zero : duration, Direction);
            else
                await this.SlideAndFadeOutAsync(firstLoad ? TimeSpan.Zero : duration, Direction);

        }
    }
}
