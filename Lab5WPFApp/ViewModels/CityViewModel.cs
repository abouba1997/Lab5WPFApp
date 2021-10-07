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
    public class CityViewModel : BaseViewModel
    {
        private CityService serviceCity;
        public CityService ServiceCity
        {
            get { return serviceCity; }
            set { serviceCity = value; RaisePropertyChanged("ServiceCity"); }
        }

        private City currentCity;
        public City CurrentCity
        {
            get { return currentCity; }
            set { currentCity = value; RaisePropertyChanged("CurrentCity"); }
        }

        public ICommand BtnCity { get; set; }

        public CityViewModel()
        {
            CurrentCity = new City();
            BtnCity = new AllCommand(OperationCity);
            ServiceCity = new CityService();
        }

        private void OperationCity(object parameter)
        {
            if (parameter.ToString() == "Add")
            {
                ServiceCity.Add(CurrentCity);
                CurrentCity = null;
            }
            else if (parameter.ToString() == "Update")
            {
                ServiceCity.Update(CurrentCity);
                CurrentCity = null;
            }
            else if (parameter.ToString() == "Delete")
            {
                ServiceCity.Delete(CurrentCity);
                CurrentCity = null;
            }
            else if (parameter.ToString() == "Search")
            {
                CurrentCity = ServiceCity.Search(CurrentCity);
            }

            if (CurrentCity == null)
            {
                CurrentCity = new City();
            }
        }
    }
}
