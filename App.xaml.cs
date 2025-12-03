using Prism.Ioc;
using Prism.Unity;
using sistecDesktopRefactored.Interfaces;
using sistecDesktopRefactored.Models;
using sistecDesktopRefactored.Services;
using sistecDesktopRefactored.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace sistecDesktopRefactored
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public static User LoggedUser { get; set; }

        protected override void OnInitialized()
        {
            // 1) Resolve a LoginWindow via container ou diretamente
            var loginVm = Container.Resolve<LoginWindowViewModel>();
            var loginWindow = new LoginWindow
            {
                DataContext = loginVm
            };

            bool loginOk = false;

            loginVm.LoginResult += (sender, success) =>
            {
                loginOk = success;
                loginWindow.Close();
            };

            // 2) Mostra o login como dialog
            var result = loginWindow.ShowDialog();

            if (!loginOk)
            {
                Shutdown();
                return;
            }

            // 3) Cria a Shell normalmente
            var shell = CreateShell();         // Container.Resolve<MainWindow>()
            Current.MainWindow = shell;
            shell.Show();

            base.OnInitialized();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<ApiClient>(new ApiClient());

            containerRegistry.RegisterForNavigation<Views.HomeView>("HomeView");
            containerRegistry.RegisterForNavigation<Views.DashboardView>("DashboardView");
            containerRegistry.RegisterForNavigation<Views.TicketsView>("TicketsView");
            containerRegistry.RegisterForNavigation<Views.UsersView>("UsersView");

            containerRegistry.Register<IFileDialogService, FileDialogService>();

        }
    }
}
