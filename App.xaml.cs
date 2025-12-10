using dotenv.net;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;
using sistecDesktopRefactored.Interfaces;
using sistecDesktopRefactored.Models;
using sistecDesktopRefactored.Services;
using sistecDesktopRefactored.ViewModels;
using sistecDesktopRefactored.ViewModels.Tickets;
using sistecDesktopRefactored.Views;
using sistecDesktopRefactored.Views.Auth;
using sistecDesktopRefactored.Views.Home;
using sistecDesktopRefactored.Views.Shell;
using sistecDesktopRefactored.Views.Tickets;
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

        public App()
        {
            DotEnv.Load();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var regionManager = Container.Resolve<IRegionManager>();

            // Ao iniciar o app, mostra LoginView na ShellRegion
            regionManager.RequestNavigate("ShellRegion", "LoginView");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(new ApiClient());

            // views de navegação
            containerRegistry.RegisterForNavigation<LoginView>("LoginView");

            containerRegistry.RegisterForNavigation<MainLayoutView>("MainLayoutView");
            containerRegistry.RegisterForNavigation<HomeView>("HomeView");
            containerRegistry.RegisterForNavigation<DashboardView>("DashboardView");
            containerRegistry.RegisterForNavigation<TicketsView>("TicketsView");
            containerRegistry.RegisterForNavigation<UsersView>("UsersView");

            containerRegistry.RegisterDialog<TicketDetailsView, TicketDetailsViewModel>("TicketDetailsDialog");
            containerRegistry.RegisterDialog<TicketCreateView, TicketCreateViewModel>("TicketCreateDialog");

            containerRegistry.Register<IFileDialogService, FileDialog>();
            containerRegistry.RegisterSingleton<IBusyService, BusyService>();

        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<LoginView, LoginViewModel>();
            ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
        }
    }
}
