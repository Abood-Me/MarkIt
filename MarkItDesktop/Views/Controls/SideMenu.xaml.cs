using MarkItDesktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace MarkItDesktop.Views.Controls
{
    /// <summary>
    /// Interaction logic for SideMenu.xaml
    /// </summary>
    public partial class SideMenu : AnimatedControl
    {
        public SideMenu()
        {
            InitializeComponent();
            DataContext = App.AppHost.Services.GetRequiredService<SideMenuViewModel>();
        }
    }
}
