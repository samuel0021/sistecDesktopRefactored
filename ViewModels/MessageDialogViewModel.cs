using Prism.Mvvm;
using Prism.Services.Dialogs;
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
        
        // Constructor
        public MessageDialogViewModel()
        {
        }

        // ====== IDialogAware ======
        #region Navigation
        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            throw new NotImplementedException();
        }

        public void OnDialogClosed()
        {
            throw new NotImplementedException();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
