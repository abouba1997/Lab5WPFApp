using Lab5WPFApp.Commands;
using Lab5WPFApp.Models;
using Lab5WPFApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lab5WPFApp.ViewModels
{
    public class RegionViewModel : BaseViewModel
    {
        private RegionService serviceRegion;
        public RegionService ServiceRegion
        {
            get { return serviceRegion; }
            set { serviceRegion = value; RaisePropertyChanged("ServiceRegion"); }
        }

        private Region currentRegion;
        public Region CurrentRegion
        {
            get { return currentRegion; }
            set { currentRegion = value; RaisePropertyChanged("CurrentRegion"); }
        }

        public ICommand BtnRegion { get; set; }

        public RegionViewModel()
        {
            CurrentRegion = new Region();
            BtnRegion = new AllCommand(OperationRegion);
            ServiceRegion = new RegionService();
        }

        private void OperationRegion(object parameter)
        {
            if (parameter.ToString() == "Add")
            {
                ServiceRegion.Add(CurrentRegion);
                CurrentRegion = null;
            }
            else if (parameter.ToString() == "Update")
            {
                ServiceRegion.Update(CurrentRegion);
                CurrentRegion = null;
            }
            else if (parameter.ToString() == "Delete")
            {
                ServiceRegion.Delete(CurrentRegion);
                CurrentRegion = null;
            }
            else if (parameter.ToString() == "Search")
            {
                CurrentRegion = ServiceRegion.Search(CurrentRegion);
            }

            if (CurrentRegion == null)
            {
                CurrentRegion = new Region();
            }
        }
    }
}
