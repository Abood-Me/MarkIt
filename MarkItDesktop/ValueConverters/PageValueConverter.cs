using MarkItDesktop.Models;
using MarkItDesktop.ViewModels;
using MarkItDesktop.Views;
using Microsoft.Extensions.DependencyInjection;
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
                    // TODO :
                    // - Change Pages to have generic ViewModel type
                    // - Add a GoToPage method with a ViewModel as argument
                    return new LoginPage()
                    {
                        DataContext = App.AppHost.Services.GetRequiredService<LoginViewModel>()
                    };
                case ApplicationPage.MainPage:
                    return new MainPage()
                    {
                        DataContext = App.AppHost.Services.GetRequiredService<MainViewModel>()
                    };
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type tsargetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
