using Lab5WPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lab5WPFApp.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private AppViewModel viewModel;

        public UpdateViewCommand(AppViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(parameter.ToString() == "Street")
            {
                viewModel.SelectedViewModel = new StreetViewModel();
            }else if(parameter.ToString() == "City")
            {
                viewModel.SelectedViewModel = new CityViewModel();
            }else if(parameter.ToString() == "Locality")
            {
                viewModel.SelectedViewModel = new LocalityViewModel();
            }else if(parameter.ToString() == "Region")
            {
                viewModel.SelectedViewModel = new RegionViewModel();
            }
        }
    }
}
