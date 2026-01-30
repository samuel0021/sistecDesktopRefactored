using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.ViewModels
{
    public class TicketCreateMessageViewModel : BindableBase, IDialogAware
    {
        private string _dialogTitle;
        private string _ticketMessage;
        private int _ticketId;

        #region Encapsulations
        public string Title
        {
            get => _dialogTitle;
            set => SetProperty(ref _dialogTitle, value);
        }
        public string TicketMessage
        {
            get => _ticketMessage;
            set => SetProperty(ref _ticketMessage, value);
        }
        #endregion

        public DelegateCommand OkCommand { get; }

        public TicketCreateMessageViewModel()
        {
            OkCommand = new DelegateCommand(OnOk);
        }

        private void OnOk()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }
        // ====== IDialogAware ======
        #region Navigation
        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters) 
        {
            if (parameters.ContainsKey("message"))
                TicketMessage = parameters.GetValue<string>("message");
            if (parameters.ContainsKey("ticketId"))
                _ticketId = parameters.GetValue<int>("ticketId");
        }
        #endregion
    }
}
