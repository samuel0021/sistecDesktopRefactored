using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using sistecDesktopRefactored.Interfaces;
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
        public IBusyService BusyService { get; }

        public MainWindowViewModel(IBusyService busyService)
        {
            BusyService = busyService;
        }
    }
}
