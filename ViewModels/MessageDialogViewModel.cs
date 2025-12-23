using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using sistecDesktopRefactored.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.ViewModels
{
    public class MessageDialogViewModel : BindableBase, IDialogAware
    {        
        private string _dialogTitle;

        #region Encapsulations
        public string Title     // Prism Window.Title
        {
            get => _dialogTitle;
            set => SetProperty(ref _dialogTitle, value);
        }
        #endregion

        // Commands
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand ConfirmCommand { get; }

        // Constructor
        public MessageDialogViewModel()
        {
            CancelCommand = new DelegateCommand(OnCancel);
            ConfirmCommand = new DelegateCommand(OnConfirm);
        }

        // ====== IDialogAware ======
        #region Navigation
        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters) { }
        #endregion

        // ==========================
        private void OnCancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        private void OnConfirm()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }
    }
}
