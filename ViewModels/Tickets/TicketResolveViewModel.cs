using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using sistecDesktopRefactored.Interfaces;
using sistecDesktopRefactored.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace sistecDesktopRefactored.ViewModels.Tickets
{
    public class TicketResolveViewModel : BindableBase, IDialogAware
    {
        private readonly ApiClient _apiClient;
        private readonly BusyService _busyService;
        public string Title => "";

        private string _motivo;

        private int _ticketId;
        private string _ticketCategory;
        private string _ticketProblem;

        #region Encapsulations
        public string Motivo
        {
            get => _motivo;
            set => SetProperty(ref _motivo, value);
        }
        public int TicketId
        {
            get => _ticketId;
            set => SetProperty(ref _ticketId, value);
        }
        public string TicketCategory
        {
            get => _ticketCategory;
            set => SetProperty(ref _ticketCategory, value);
        }
        public string TicketProblem
        {
            get => _ticketProblem;
            set => SetProperty(ref _ticketProblem, value);
        }
        #endregion

        // ====== Comands ======
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand ResolveTicketCommand { get; }

        public TicketResolveViewModel(ApiClient apiClient, BusyService busyService)
        {
            _apiClient = apiClient;
            _busyService = busyService;

            CancelCommand = new DelegateCommand(ExecuteCancel);
            ResolveTicketCommand = new DelegateCommand(ExecuteResolve);
        }

        private void ExecuteCancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        private async void ExecuteResolve()
        {
            if (string.IsNullOrWhiteSpace(Motivo))
            {
                // Validação simples
                return;
            }

            _busyService.IsBusy = true;
            try
            {
                await _apiClient.ResolveTicketAsync(TicketId, Motivo);

                // ← Fecha com OK (TicketsViewModel recarrega lista)
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
                MessageBox.Show($"Chamado resolvido");
            }
            catch (Exception ex)
            {
                // Trate erro (MessageBox ou ErrorMessage)
                MessageBox.Show($"Erro ao resolver: {ex.Message}");
            }
            finally
            {
                _busyService.IsBusy = false;
            }
        }


        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("ticketId"))
                TicketId = parameters.GetValue<int>("ticketId");

            if (parameters.ContainsKey("category"))
            {
                TicketCategory = parameters.GetValue<string>("category");
                Console.WriteLine($"DEBUG Category: '{TicketCategory}'");  // ← TESTE
            }


            if (parameters.ContainsKey("problem"))
            {
                TicketProblem = parameters.GetValue<string>("problem");
                Console.WriteLine($"DEBUG Problem: '{TicketProblem}'");  // ← TESTE
            }
        }
    }
}
