using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using sistecDesktopRefactored.Interfaces;
using sistecDesktopRefactored.Models;
using sistecDesktopRefactored.Models.Auth;
using sistecDesktopRefactored.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace sistecDesktopRefactored.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly ApiClient _apiClient;
        private IBusyService _busyService;

        private string _email;
        private string _password;
        private string _errorMessage;

        #region Encapsulations
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        #endregion

        #region Commands
        //public ICommand EsqueciSenhaCommand { get; }
        public ICommand LoginCommand { get; }
        #endregion

        #region Constructor
        public LoginViewModel( ApiClient apiClient, IRegionManager regionManager, IBusyService busyService)
        {
            _apiClient = apiClient;
            _regionManager = regionManager;
            _busyService = busyService;

            LoginCommand = new DelegateCommand(async () => await ExecutarLoginAsync());
        }
        #endregion

        private async Task ExecutarLoginAsync()
        {
            _busyService.IsBusy = true;

            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Email ou senha inválidos.";
                return;
            }

            try
            {
                var loginRequest = new LoginRequest
                {
                    Email = Email,
                    Password = Password
                };

                var result = await _apiClient.LoginAsync(loginRequest);

                if (result != null &&
                    result.Success &&
                    result.Data != null &&
                    result.Data.User != null)
                {
                    App.LoggedUser = result.Data.User;

                    // navega a ShellRegion para o layout principal
                    _regionManager.RequestNavigate("ShellRegion", "MainLayoutView");
                    _regionManager.RequestNavigate("MainRegion", "TicketsView");
                }
                else
                {
                    ErrorMessage = "Email ou senha inválidos.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro ao conectar com o servidor: {ex.Message}";
            }
            finally
            {
                _busyService.IsBusy = false;
            }
        }
    }
}
