using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.ViewModels.Shell
{
    public class MainLayoutViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        public DelegateCommand<string> NavigateCommand { get; }

        public MainLayoutViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(OnNavigate);

        }

        private void OnNavigate(string viewName)
        {
            if (string.IsNullOrWhiteSpace(viewName))
                return;

            _regionManager.RequestNavigate("MainRegion", viewName);
        }
    }
}
