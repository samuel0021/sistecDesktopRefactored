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
        private readonly DialogService _dialogService;

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

        public TicketResolveViewModel(ApiClient apiClient, BusyService busyService, DialogService dialogService)
        {
            _apiClient = apiClient;
            _busyService = busyService;
            _dialogService = dialogService;

            CancelCommand = new DelegateCommand(ExecuteCancel);
            ResolveTicketCommand = new DelegateCommand(ExecuteResolve);
            _dialogService = dialogService;
        }

        private void ExecuteCancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        private async void ExecuteResolve()
        {
            if (string.IsNullOrWhiteSpace(Motivo) || Motivo.Length < 20)
            {
                MessageBox.Show("Motivo deve ter pelo menos 20 caracteres!");
                return;
            }

            // dialog confirmação
            var confirmParams = new DialogParameters
            {
                { "message", $"Confirmar resolução do chamado #{TicketId}?\n\nMotivo: {Motivo}..." }
            };

            _busyService.IsBusy = true;

            _dialogService.ShowDialog("ConfirmDialog", confirmParams, async r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    _busyService.IsBusy = true;

                    try
                    {
                        // objeto anônimo para relatório de resolução
                        // com as propriedades do backend pro json serializar
                        var reportBody = new
                        {
                            id_chamado = TicketId,
                            relatorio_resposta = Motivo,
                            id_usuario_abertura = App.LoggedUser.IdPerfilUsuario.Id
                        };

                        await _apiClient.ResolveTicketWithReportAsync(reportBody);

                        await _apiClient.ResolveTicketAsync(TicketId);

                        // fecha com OK (TicketsViewModel recarrega lista)
                        RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
                        MessageBox.Show($"Chamado resolvido");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao resolver: {ex.Message}");
                    }
                    finally
                    {
                        _busyService.IsBusy = false;
                    }
                }
            });


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
                Console.WriteLine($"DEBUG Category: '{TicketCategory}'");
            }


            if (parameters.ContainsKey("problem"))
            {
                TicketProblem = parameters.GetValue<string>("problem");
                Console.WriteLine($"DEBUG Problem: '{TicketProblem}'");
            }
        }
    }
}
