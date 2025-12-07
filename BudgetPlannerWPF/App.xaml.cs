using BudgetPlannerWPF.Data;
using BudgetPlannerWPF.Services;
using BudgetPlannerWPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace BudgetPlannerWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    //public partial class App : Application
    //{
    //    public static ServiceProvider ServiceProvider { get; private set; }

    //    private App()
    //    {
    //        // 1. Configure Services
    //        var services = new ServiceCollection();

    //        // Add DbContext — connection string ligger i BudgetContext
    //        services.AddDbContext<BudgetContext>();

    //        // Add Repository & Service
    //        services.AddScoped<IBudgetRepository, BudgetRepository>();
    //        services.AddScoped<IBudgetService, BudgetService>();

    //        // Add ViewModels
    //        services.AddSingleton<MainViewModel>();

    //        // Add MainWindow and inject MainViewModel
    //        services.AddSingleton<MainWindow>();

    //        ServiceProvider = services.BuildServiceProvider();

    //        // 2. Start app
    //        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
    //        mainWindow.Show();
    //    }

    //    private void Application_Startup(object sender, StartupEventArgs e)
    //    {
    //        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
    //        mainWindow.Show();
    //    }
    //}

    // App.xaml.cs (C#)
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
