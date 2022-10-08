using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MarkItDesktop.Views.Controls
{
    public class TextBox : System.Windows.Controls.TextBox
    {

        public string PlaceHolder
        {
            get { return (string)GetValue(PlaceHolderProperty); }
            set { SetValue(PlaceHolderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceHolder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceHolderProperty =
            DependencyProperty.Register("PlaceHolder", typeof(string), typeof(TextBox), new PropertyMetadata(string.Empty));



        public bool ForceFocus
        {
            get { return (bool)GetValue(ForceFocusProperty); }
            set { SetValue(ForceFocusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ForceFocus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForceFocusProperty =
            DependencyProperty.Register("ForceFocus", typeof(bool), typeof(TextBox), new PropertyMetadata(false, null, CoerceTextFocus));

        private static object CoerceTextFocus(DependencyObject d, object value)
        {
            TextBox textBox = (TextBox)d;
            if ((bool)value)
            {
                textBox.Focus();
                textBox.CaretIndex = textBox.Text.Length;
            }

            return value;
        }
    }
}
