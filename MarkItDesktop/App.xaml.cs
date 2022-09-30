using MarkItDesktop.Data;
using MarkItDesktop.Services;
using MarkItDesktop.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using System.Net.Http.Headers;

namespace MarkItDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; } = null!;
        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(config =>
                {
                    config.AddEnvironmentVariables();
                })
                .ConfigureAppConfiguration((context,config) =>
                {
                    
                    config.SetBasePath(Environment.CurrentDirectory);
                    config.AddJsonFile("appsettings.json", false, true);
                    config.AddEnvironmentVariables();

                })
                .ConfigureServices((context,services) =>
                {
                    
                    var connectionString = context.Configuration.GetConnectionString("ClientStore");
                    services.AddDbContext<ClientDbContext>(options => options.UseSqlite(connectionString), ServiceLifetime.Singleton);

                    // Register services
                    services.AddTransient<AuthorizationHandler>();
                    services.AddSingleton<IClientDataStore, ClientDataStore>();
                    services.AddHttpClient<IAuthService, AuthService>(client =>
                        {
                            client.BaseAddress = new Uri("https://localhost:5000/api/auth/");
                        })
                        .AddHttpMessageHandler<AuthorizationHandler>();
                    services.AddHttpClient<ITodoService, TodoService>(client =>
                        {
                            client.BaseAddress = new Uri("https://localhost:5000/api/todos/");
                        })
                        .AddHttpMessageHandler<AuthorizationHandler>();

                    services.AddSingleton<MainWindow>();
                    services.AddApplicationViewModels();

                })
                .Build();

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Start the generic Host
            AppHost.Start();
            // Request the main window as a singleton
            MainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            MainWindow.Visibility = Visibility.Visible;

        }
    }
}
