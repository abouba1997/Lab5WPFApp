using Lab5WPFApp.Commands;
using Lab5WPFApp.Models;
using Lab5WPFApp.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Lab5WPFApp.ViewModels
{
    public class StreetViewModel : BaseViewModel
    {
        private StreetService serviceStreet;
        public StreetService ServiceStreet
        {
            get { return serviceStreet; }
            set { serviceStreet = value; RaisePropertyChanged("ServiceStreet"); }
        }

        private Street currentStreet;
        public Street CurrentStreet
        {
            get { return currentStreet; }
            set { currentStreet = value; RaisePropertyChanged("CurrentStreet"); }
        }

        public ICommand BtnStreet { get; set; }

        public StreetViewModel()
        {
            CurrentStreet = new Street();
            BtnStreet = new AllCommand(OperationStreet);
            ServiceStreet = new StreetService();
        }

        private void OperationStreet(object parameter)
        {
            if (parameter.ToString() == "Add")
            {
                ServiceStreet.Add(CurrentStreet);
                CurrentStreet = null;
            }
            else if (parameter.ToString() == "Update")
            {
                ServiceStreet.Update(CurrentStreet);
                CurrentStreet = null;
            }
            else if (parameter.ToString() == "Delete")
            {
                ServiceStreet.Delete(CurrentStreet);
                currentStreet = null;
            }
            else if (parameter.ToString() == "Search")
            {
                CurrentStreet = ServiceStreet.Search(CurrentStreet);
            }

            if (CurrentStreet == null)
            {
                CurrentStreet = new Street();
            }
        }
    }
}
