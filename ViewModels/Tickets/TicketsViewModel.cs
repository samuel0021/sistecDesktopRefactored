using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using sistecDesktopRefactored.Interfaces;
using sistecDesktopRefactored.Models;
using sistecDesktopRefactored.Models.Tickets;
using sistecDesktopRefactored.Services;
using sistecDesktopRefactored.ViewModels.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace sistecDesktopRefactored.ViewModels
{
    public class TicketsViewModel : BindableBase
    {
        private readonly ApiClient _apiClient;
        private readonly IDialogService _dialogService;
        private IBusyService _busyService;

        private string _title = "Detalhes do Chamado";

        private ObservableCollection<Ticket> _tickets;
        private string _errorMessage;
        public Ticket SelectedTicket { get; set; }

        #region Encapsulations
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public ObservableCollection<Ticket> Tickets
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
        public DelegateCommand OpenCreateTicketCommand { get; }
        public DelegateCommand<object> OpenResolveTicketCommand { get; }
        public DelegateCommand<Ticket> ShowDetailsCommand { get; }

        #endregion

        // Constructor
        public TicketsViewModel(ApiClient apiClient, IDialogService dialogService, IBusyService busyService)
        {
            _apiClient = apiClient;
            _dialogService = dialogService;
            _busyService = busyService;

            Tickets = new ObservableCollection<Ticket>();

            LoadTicketsCommand = new DelegateCommand(async () => await LoadTicketsAsync());
            OpenCreateTicketCommand = new DelegateCommand(OpenCreateTicket);
            OpenResolveTicketCommand = new DelegateCommand<object>(OpenResolveTicket);
            ShowDetailsCommand = new DelegateCommand<Ticket>(ShowDetails);

            _ = LoadTicketsAsync();
        }

        public async Task LoadTicketsAsync()
        {
            _busyService.IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                List<Ticket> list = await _apiClient.GetTicketsAsync();

                Tickets = new ObservableCollection<Ticket>(list);
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


        private void OpenCreateTicket()
        {
            _busyService.IsBusy = true;

            // abre dialog de criar chamado
            _dialogService.ShowDialog("TicketCreateDialog", r =>
            {
                // se o dialog fechou e trouxe um parâmetro do id, 
                if (r.Result == ButtonResult.OK && r.Parameters.ContainsKey("ticketId"))
                {
                    // salva o valor numa variável
                    var ticketId = r.Parameters.GetValue<int>("ticketId");

                    // transforma o valor no tipo DialogParameters porque só aceita assim
                    var successParams = new DialogParameters { { "ticketId", ticketId } };

                    // abre o dialogo de conclusao passando o parametro 
                    _dialogService.ShowDialog("TicketCreatedMessage", successParams, result =>
                    {
                        _ = LoadTicketsAsync();
                    });
                }
            });
            _busyService.IsBusy = false;
        }

        private void OpenResolveTicket(object parameter)
        {
            

            _busyService.IsBusy = true;

            if (parameter is Ticket ticket)  // ← Extrai Ticket da linha
            {
                SelectedTicket = ticket;  // ← Salva pra usar

                var parameters = new DialogParameters
                {
                    { "ticketId", ticket.Id },
                    { "category", ticket.Category },
                    { "problem", ticket.Problem }
                };

                _dialogService.ShowDialog("TicketResolveDialog", parameters, r =>
                {
                    _ = LoadTicketsAsync();
                });

                _busyService.IsBusy = false;
            }
        }

        private async void ShowDetails(Ticket ticket)
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
