using Prism.Mvvm;
using Prism.Services.Dialogs;
using sistecDesktopRefactored.Models;
using sistecDesktopRefactored.Models.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.ViewModels.Tickets
{
    public class TicketDetailsViewModel : BindableBase, IDialogAware
    {
        private string _title = "Detalhes do Chamado";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private Ticket _ticket;
        public Ticket Ticket
        {
            get => _ticket;
            set => SetProperty(ref _ticket, value);
        }

        public event Action<IDialogResult> RequestClose;
        public void OnDialogOpened(IDialogParameters parameters)
        {
            Ticket = parameters.GetValue<Ticket>("ticket");
        }

        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }

        public void Close()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }
    }
}
