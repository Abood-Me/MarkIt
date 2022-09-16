using MarkItDesktop.Models;
using MarkItDesktop.ViewModels;
using MarkItDesktop.Views.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MarkItDesktop.ValueConverters
{
    public class PageValueConverter : IValueConverter
    {
        public static PageValueConverter Instance = new();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ApplicationPage page = (ApplicationPage)value;
            switch (page)
            {
                case ApplicationPage.LoginPage:
                    return new LoginPage()
                    {
                        DataContext = new LoginViewModel()
                    };
                case ApplicationPage.MainPage:
                    return new MainPage();
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
