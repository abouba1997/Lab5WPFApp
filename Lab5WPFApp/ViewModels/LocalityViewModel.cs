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
    public class LocalityViewModel : BaseViewModel
    {
        private LocalityService serviceLocality;
        public LocalityService ServiceLocality
        {
            get { return serviceLocality; }
            set { serviceLocality = value; RaisePropertyChanged("ServiceLocality"); }
        }

        private Locality currentLocality;
        public Locality CurrentLocality
        {
            get { return currentLocality; }
            set { currentLocality = value; RaisePropertyChanged("CurrentLocality"); }
        }

        public ICommand BtnLocality { get; set; }

        public LocalityViewModel()
        {
            CurrentLocality = new Locality();
            BtnLocality = new AllCommand(OperationLocality);
            ServiceLocality = new LocalityService();
        }

        private void OperationLocality(object parameter)
        {
            if (parameter.ToString() == "Add")
            {
                ServiceLocality.Add(CurrentLocality);
                CurrentLocality = null;
            }
            else if (parameter.ToString() == "Update")
            {
                ServiceLocality.Update(CurrentLocality);
                CurrentLocality = null;
            }
            else if (parameter.ToString() == "Delete")
            {
                ServiceLocality.Delete(CurrentLocality);
                CurrentLocality = null;
            }
            else if (parameter.ToString() == "Search")
            {
                CurrentLocality = ServiceLocality.Search(CurrentLocality);
            }

            if (CurrentLocality == null)
            {
                CurrentLocality = new Locality();
            }
        }
    }
}
