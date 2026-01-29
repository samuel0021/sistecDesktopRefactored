using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using sistecDesktopRefactored.Interfaces;
using sistecDesktopRefactored.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.ViewModels.Shell
{
    public class MainLayoutViewModel : BindableBase, INavigationAware
    {
        private readonly ApiClient _apiClient;
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        private IBusyService _busyService;

        private string _selectedTag;

        public string NameLoggedUser => App.LoggedUser?.NomeUsuario ?? "Carregando...";

        #region Tags
        public string TagHome => SelectedTag == "Home" ? "Selected" : null;
        public string TagDashboard => SelectedTag == "Dashboard" ? "Selected" : null;
        public string TagTickets => SelectedTag == "Tickets" ? "Selected" : null;
        public string TagUsers => SelectedTag == "Users" ? "Selected" : null;
        public string TagDeletedUsers => SelectedTag == "ActiveUsers" ? "Selected" : null;
        public string TagActiveUsers => SelectedTag == "DeletedUsers" ? "Selected" : null;
        #endregion

        #region Encapsulations
        public string SelectedTag
        {
            get => _selectedTag;
            set
            {
                if (SetProperty(ref _selectedTag, value))
                {
                    RaisePropertyChanged(nameof(TagHome));
                    RaisePropertyChanged(nameof(TagDashboard));
                    RaisePropertyChanged(nameof(TagTickets));
                    RaisePropertyChanged(nameof(TagUsers));
                    RaisePropertyChanged(nameof(TagDeletedUsers));
                    RaisePropertyChanged(nameof(TagActiveUsers));
                }
            }
        }
        #endregion

        #region Commands
        public DelegateCommand<string> NavigateCommand { get; }
        public DelegateCommand LogoutConfirmationCommand { get; }
        public DelegateCommand LogoutCommand { get; }

        #endregion

        // Constructor
        public MainLayoutViewModel(ApiClient apiClient, IRegionManager regionManager, IDialogService dialogService, IBusyService busyService)
        {
            _apiClient = apiClient;
            _regionManager = regionManager;
            _dialogService = dialogService;
            _busyService = busyService;

            NavigateCommand = new DelegateCommand<string>(OnNavigate);
            LogoutConfirmationCommand = new DelegateCommand(OpenLogoutConfirmation);
            LogoutCommand = new DelegateCommand(async () => await LogoutAsync());
        }

        #region Navigation
        private void OnNavigate(string pageKey)
        {
            if (string.IsNullOrWhiteSpace(pageKey))
                return;

            SelectedTag = pageKey;
            var viewName = pageKey + "View";

            _regionManager.RequestNavigate("MainRegion", viewName);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            SelectedTag = "Home";

            RaisePropertyChanged(nameof(NameLoggedUser));
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
        #endregion

        #region Logout

        private void OpenLogoutConfirmation()
        {
            _busyService.IsBusy = true;
            _dialogService.ShowDialog("MessageDialog", async r =>
            {
                if (r.Result != ButtonResult.OK)
                {
                    _busyService.IsBusy = false;
                    return;
                }

                try
                {
                    await LogoutAsync();
                }
                finally
                {
                    _busyService.IsBusy = false;
                }
            });
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _apiClient.LogoutAsync();
                _apiClient.Logout();
                _regionManager.RequestNavigate("ShellRegion", "LoginView");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro no logout: {ex.Message}");
            }
        }
        #endregion
    }
}
