using MarkItDesktop.Models;
using MarkItDesktop.ViewModels;
using MarkItDesktop.Views;
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
    public class PageValueConverter : BaseValueConverter<PageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ApplicationPage page = (ApplicationPage)value;
            switch (page)
            {
                case ApplicationPage.LoginPage:
                    // TODO : Change Pages to have generic ViewModel type
                    return new LoginPage()
                    {
                        DataContext = new LoginViewModel()
                    };
                case ApplicationPage.MainPage:
                    return new MainPage()
                    {
                        DataContext = new MainViewModel()
                    };
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
