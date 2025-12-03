using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using sistecDesktopRefactored.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        public DelegateCommand<string> NavigateCommand { get; }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            NavigateCommand = new DelegateCommand<string>(OnNavigate);

            _regionManager.RequestNavigate("MainRegion", "HomeView");
        }

        private void OnNavigate(string viewName)
        {
            if (string.IsNullOrEmpty(viewName))
                return;

            _regionManager.RequestNavigate("MainRegion", viewName);
        }
    }
}
