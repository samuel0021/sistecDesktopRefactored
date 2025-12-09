using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using sistecDesktopRefactored.Interfaces;
using sistecDesktopRefactored.Models;
using sistecDesktopRefactored.Services;
using sistecDesktopRefactored.ViewModels.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace sistecDesktopRefactored.ViewModels
{
    public class TicketsViewModel : BindableBase
    {
        private readonly ApiClient _apiClient;
        private readonly IDialogService _dialogService;
        private IBusyService _busyService;

        private string _title = "Detalhes do Chamado";

        private ObservableCollection<Chamado> _tickets;
        private string _errorMessage;
        public Chamado SelectedTicket { get; set; }

        #region Encapsulations
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public ObservableCollection<Chamado> Tickets
        {
            get => _tickets;
            set => SetProperty(ref _tickets, value);
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        #endregion

        #region Commands
        public DelegateCommand LoadTicketsCommand { get; }
        public DelegateCommand<Chamado> ShowDetailsCommand { get; }

        #endregion

        // Constructor
        public TicketsViewModel(ApiClient apiClient, IDialogService dialogService, IBusyService busyService)
        {
            _apiClient = apiClient;
            _dialogService = dialogService;
            _busyService = busyService;

            Tickets = new ObservableCollection<Chamado>();

            LoadTicketsCommand = new DelegateCommand(async () => await LoadTicketsAsync());
            ShowDetailsCommand = new DelegateCommand<Chamado>(ShowDetails);

            _ = LoadTicketsAsync();
        }

        public async Task LoadTicketsAsync()
        {
            _busyService.IsBusy = true;
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
                _busyService.IsBusy = false;
            }
        }

        private async void ShowDetails(Chamado ticket)
        {
            if (ticket == null) return;
            SelectedTicket = ticket;

            _busyService.IsBusy = true;

            try
            {
                var updatedTicket = await _apiClient.GetTicketByIdAsync(SelectedTicket.Id);
                // se quiser, atualize SelectedTicket com updatedTicket
            }
            catch (UnauthorizedAccessException)
            {
                // depois trocamos MessageBox por dialog Prism
                MessageBox.Show("Sessão expirada. Faça login novamente.", "Erro de Autenticação",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar detalhes do chamado: {ex.Message}", "Erro",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            

            var parameters = new DialogParameters { { "ticket", SelectedTicket } };

            _dialogService.ShowDialog("TicketDetailsDialog", parameters, r =>
            {
                // tratar resultado se precisar
            });

            _busyService.IsBusy = false;
        }
    }
}
