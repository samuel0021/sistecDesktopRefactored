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
    public class ResultDialogViewModel : BindableBase, IDialogAware
    {
        public string Title => "Sucesso";

        private string _message = "Operação concluída!";

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public DelegateCommand OkCommand { get; }
        public ResultDialogViewModel()
        {
            OkCommand = new DelegateCommand(OnOk);
        }

        private void OnOk()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }

        #region IDialogAware
        public event Action<IDialogResult> RequestClose;
        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Message = parameters.GetValue<string>("message") ?? "Operação concluída!";
        }
        #endregion
    }
}

/* uso
  _dialogService.ShowDialog("ResultDialog", new DialogParameters
    {
        { "message", "Chamado #123 criado com sucesso!" }
    }, null);  // Sem callback

// Com callback
_dialogService.ShowDialog("ResultDialog", new DialogParameters
{
    { "message", $"Erro: {ex.Message}" }
}, r => { if (r.Result == ButtonResult.OK) {   } });
*/

