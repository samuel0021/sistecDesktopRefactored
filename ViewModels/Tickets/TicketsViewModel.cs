using Prism.Commands;
using Prism.Mvvm;
using sistecDesktopRefactored.Models;
using sistecDesktopRefactored.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.ViewModels
{
    public class TicketsViewModel : BindableBase
    {
        private readonly ApiClient _apiClient;

        private ObservableCollection<Chamado> _tickets;
        private bool _isLoading;
        private string _errorMessage;


        #region Encapsulations
        public ObservableCollection<Chamado> Tickets
        {
            get => _tickets;
            set => SetProperty(ref _tickets, value);
        }
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        #endregion

        // Commands
        public DelegateCommand LoadTicketsCommand { get; }

        // Constructor
        public TicketsViewModel(ApiClient apiClient)
        {
            _apiClient = apiClient;

            Tickets = new ObservableCollection<Chamado>();
            LoadTicketsCommand = new DelegateCommand(async () => await LoadTicketsAsync());

            _ = LoadTicketsAsync();
        }

        public async Task LoadTicketsAsync()
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                List<Chamado> list = await _apiClient.GetChamadosAsync();

                Tickets = new ObservableCollection<Chamado>(list);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro ao carregar chamados: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
