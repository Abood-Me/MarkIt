using MarkItDesktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkItDesktop.Helpers
{
    public static class DIServicesExtensions
    {
        public static IServiceCollection AddApplicationViewModels(this IServiceCollection services)
        {
            services.AddSingleton<ApplicationViewModel>();
            services.AddTransient<LaunchViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<MainViewModel>();

            return services;
        }
    }
}
