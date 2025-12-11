using BudgetPlannerWPF.Data;
using BudgetPlannerWPF.Services;
using BudgetPlannerWPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace BudgetPlannerWPF
{
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            InitializeComponent(); // ensure App.xaml resources are loaded

            var services = new ServiceCollection();
            services.AddDbContext<BudgetContext>();
            services.AddScoped<IBudgetRepository, BudgetRepository>();
            services.AddScoped<IBudgetService, BudgetService>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainWindow>();

            ServiceProvider = services.BuildServiceProvider();

            // Show window after resources are available
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
