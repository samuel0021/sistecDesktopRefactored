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
    public class TicketScaleViewModel : BindableBase, IDialogAware
    {
        private readonly ApiClient _apiClient;
        private readonly BusyService _busyService;
        private readonly DialogService _dialogService;

        private string _title;
        private string _motivo;
        private int _ticketId;


        #region Encapsulations
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
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
        #endregion

        // Commands

        public DelegateCommand ScaleTicketCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public TicketScaleViewModel(ApiClient apiClient, BusyService busyService, DialogService dialogService)
        {
            _apiClient = apiClient;
            _busyService = busyService;
            _dialogService = dialogService;

            CancelCommand = new DelegateCommand(ExecuteCancel);
            ScaleTicketCommand = new DelegateCommand(ExecuteScale);
        }

        private void ExecuteCancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        private async void ExecuteScale()
        {
            if (string.IsNullOrWhiteSpace(Motivo) || Motivo.Length < 10)
            {
                MessageBox.Show("Motivo deve ter pelo menos 10 caracteres!");
                return;
            }

            // dialog confirmação
            var confirmParams = new DialogParameters
            {
                { "message", $"Confirmar escalonamento do chamado #{TicketId}?\n\nMotivo: {Motivo}" }
            };

            _busyService.IsBusy = true;

            _dialogService.ShowDialog("ConfirmDialog", confirmParams, async r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    try
                    {
                        await _apiClient.ScaleTicketAsync(TicketId, Motivo);

                        RequestClose?.Invoke(new DialogResult(ButtonResult.OK));

                        _dialogService.ShowDialog("ResultDialog", new DialogParameters
                        {
                            { "message", $"Chamado #{TicketId} escalado com sucesso!" }
                        }, null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao escalar: {ex.Message}");
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
            Title = parameters.GetValue<string>("title") ?? "Confirmação";

            if (parameters.ContainsKey("ticketId"))
                TicketId = parameters.GetValue<int>("ticketId");
        }
    }
}
