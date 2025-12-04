using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using sistecDesktopRefactored.Models;
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

        private string _email;
        private string _password;
        private string _errorMessage;
        private bool _isLoading;

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
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }
        #endregion

        #region Commands
        //public ICommand EsqueciSenhaCommand { get; }
        public ICommand LoginCommand { get; }
        #endregion

        #region Constructor
        public LoginViewModel(IRegionManager regionManager, ApiClient apiClient)
        {
            _regionManager = regionManager;
            _apiClient = apiClient;

            LoginCommand = new DelegateCommand(async () => await ExecutarLoginAsync());
        }
        #endregion

        private async Task ExecutarLoginAsync()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Email ou senha inválidos.";
                return;
            }

            IsLoading = true;

            try
            {
                var loginRequest = new LoginRequest
                {
                    Email = Email,
                    Password = Password
                };

                var result = await _apiClient.LoginAsync(loginRequest);

                // aqui é importante bater com o modelo real da sua API
                if (result != null &&
                    result.Success &&
                    result.Data != null &&
                    result.Data.User != null)
                {
                    App.LoggedUser = result.Data.User;

                    // navega a ShellRegion para o layout principal
                    _regionManager.RequestNavigate("ShellRegion", "MainLayoutView");
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
                IsLoading = false;
            }
        }
    }
}
