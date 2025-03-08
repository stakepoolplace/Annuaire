using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using DevExpress.Xpf.Core;
using Annuaire.Services;
using Annuaire.ViewModels;
using Annuaire.Views;
using System.Windows.Data;

namespace Annuaire
{
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IAnnuaireService, AnnuaireService>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainView>();
        }

        static App()
        {
            CompatibilitySettings.UseLightweightThemes = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = serviceProvider.GetRequiredService<MainView>();
            var mainViewModel = serviceProvider.GetRequiredService<MainViewModel>();
            mainWindow.DataContext = mainViewModel;
            MainWindow = new Window { Content = mainWindow };
            BindingOperations.SetBinding(MainWindow, Window.TitleProperty, new Binding("WindowTitle") { Source = mainViewModel });
            MainWindow.Show();
        }
    }
}
