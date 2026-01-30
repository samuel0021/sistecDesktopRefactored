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
    public class ConfirmDialogViewModel : BindableBase, IDialogAware
    {
        private string _title = "Confirmação";
        private string _message;
        private string _confirmText = "Confirmar";
        private string _cancelText = "Cancelar";

        #region Encapsulations
        public string Title
        { 
            get => _title; 
            set => SetProperty(ref _title, value); 
        }
        public string Message
        { 
            get => _message; 
            set => SetProperty(ref _message, value); 
        }
        public string ConfirmText
        { 
            get => _confirmText; 
            set => SetProperty(ref _confirmText, value); 
        }
        public string CancelText
        { 
            get => _cancelText; 
            set => SetProperty(ref _cancelText, value); 
        }
        #endregion

        public DelegateCommand ConfirmCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public ConfirmDialogViewModel()
        {
            ConfirmCommand = new DelegateCommand(Confirm);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Confirm()
        {
            var result = new DialogParameters 
            { 
                { "confirmed", true } 
            };

            RequestClose(new DialogResult(ButtonResult.OK, result));
        }

        private void Cancel()
        {
            RequestClose(new DialogResult(ButtonResult.Cancel));
        }

        public event Action<IDialogResult> RequestClose;
        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("title") ?? "Confirmação";
            Message = parameters.GetValue<string>("message");
            ConfirmText = parameters.GetValue<string>("confirmText") ?? "Confirmar";
            CancelText = parameters.GetValue<string>("cancelText") ?? "Cancelar";
        }
    }
}
