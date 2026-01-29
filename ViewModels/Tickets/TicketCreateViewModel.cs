using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using sistecDesktopRefactored.Interfaces;
using sistecDesktopRefactored.Models;
using sistecDesktopRefactored.Models.Tickets;
using sistecDesktopRefactored.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace sistecDesktopRefactored.ViewModels.Tickets
{
    public class TicketCreateViewModel : BindableBase, IDialogAware
    {
        private readonly ApiClient _apiClient;
        private readonly IBusyService _busyService;

        private string _dialogTitle = "Abrir Novo Chamado";
        private string _ticketTitle;
        private string _description;
        private string _errorMessage;
        private int _priority;
        private string _selectedCategory;
        private ProblemItem _selectedProblem;
        private ObservableCollection<ProblemItem> _problemsList;

        #region Encapsulations
        public string Title          // esta é a que o Prism usa no Window.Title
        {
            get => _dialogTitle;
            set => SetProperty(ref _dialogTitle, value);
        }
        public string TicketTitle
        {
            get => _ticketTitle;
            set => SetProperty(ref _ticketTitle, value);
        }
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        public int Priority
        {
            get => _priority;
            set => SetProperty(ref _priority, value);
        }
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (SetProperty(ref _selectedCategory, value))
                {
                    RefreshProblems(value);
                    SelectedProblem = null;
                }
            }
        }
        public ProblemItem SelectedProblem
        {
            get => _selectedProblem;
            set => SetProperty(ref _selectedProblem, value);
        }
        public ObservableCollection<ProblemItem> ProblemsList
        {
            get => _problemsList;
            set => SetProperty(ref _problemsList, value);
        }
        #endregion

        // ====== IDialogAware ======
        #region Navigation
        public event Action<IDialogResult> RequestClose;

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("category"))
                SelectedCategory = parameters.GetValue<string>("category");
        }

        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }
        #endregion

        // ====== Comandos ======
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand CreateTicketCommand { get; }

        // ====== Constructor ======
        public TicketCreateViewModel(ApiClient apiClient, IBusyService busyService)
        {
            _apiClient = apiClient;
            _busyService = busyService;

            CancelCommand = new DelegateCommand(ExecuteCancel);
            CreateTicketCommand = new DelegateCommand(ExecuteCreate);
            ProblemsList = new ObservableCollection<ProblemItem>();
        }

        private void ExecuteCancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        private async void ExecuteCreate()
        {
            ErrorMessage = string.Empty;

            var input = new TicketInputModel
            {
                TitleInput = TicketTitle,
                PriorityInput = Priority,
                CategoryInput = SelectedCategory,
                ProblemInput = SelectedProblem?.Value,
                DescriptionInput = Description
            };

            var context = new ValidationContext(input);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(
                input, context, results, validateAllProperties: true);

            if (!isValid)
            {
                ErrorMessage = results.First().ErrorMessage;
                return;
            }

            await CreateTicketAsync(input);
        }

        private async Task CreateTicketAsync(TicketInputModel input)
        {
            var category = SelectedCategory?.ToLower();

            _busyService.IsBusy = true;

            try
            {
                var request = new CreateTicketRequest
                {
                    Title = TicketTitle,
                    Description = Description,
                    UserId = App.LoggedUser.IdPerfilUsuario.Id,
                    Priority = Priority,
                    Category = category,
                    DescricaoCategoria = category,
                    DescricaoCategoriaChamado = category,
                    Problem = SelectedProblem?.Value,

                    DescricaoDetalhada = Description
                };

                var ticket = await _apiClient.CreateTicketAsync(request);

                MessageBox.Show(
                    $"Chamado #{ticket.Id} criado com sucesso!",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro ao montar request: {ex.Message}";
            }
            finally
            {
                _busyService.IsBusy = false;
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
        }

        // Atualizar problemas quando categoria mudar
        private void RefreshProblems(string category)
        {
            ProblemsList.Clear();

            if (string.IsNullOrEmpty(category))
                return;

            var categoryFound = TicketCategoryProvider.GetAll().FirstOrDefault(c => c.Categoria == category);

            if (categoryFound != null)
            {
                foreach (var problema in categoryFound.Problemas)
                {
                    ProblemsList.Add(problema);
                }
            }
        }
    }
}
