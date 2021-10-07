using Lab5WPFApp.Commands;
using Lab5WPFApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lab5WPFApp.ViewModels
{
    public class AppViewModel : BaseViewModel
    {
        #region Changes View based on Addresses Informations
        private BaseViewModel selectedViewModel = new StreetViewModel();
        public BaseViewModel SelectedViewModel {
            get => selectedViewModel;
            set
            {
                selectedViewModel = value;
                RaisePropertyChanged(nameof(SelectedViewModel));
            }
        }
        public ICommand UpdateViewCommand { get; set; }
        #endregion

        #region Client Operation

        private ClientViewModel clientView;
        public ClientViewModel ClientView
        {
            get { return clientView; }
            set 
            { 
                clientView = value; 
                RaisePropertyChanged("ClientView"); 
            }
        }

        private Client currentClient;
        public Client CurrentClient
        {
            get { return currentClient; }
            set { currentClient = value; RaisePropertyChanged("CurrentClient"); }
        }
        public ICommand BtnClient { get; set; }

        private void OperationClient(object parameter)
        {
            if (parameter.ToString() == "Add")
            {
                ClientView.Add(CurrentClient);
            }
            else if (parameter.ToString() == "Delete")
            {
                ClientView.Delete(CurrentClient);
                CurrentClient = null;
            }
            else if (parameter.ToString() == "Update")
            {
                ClientView.Update(CurrentClient);
                CurrentClient = null;
            }
            else if (parameter.ToString() == "Search")
            {
                CurrentClient = ClientView.Search(CurrentClient);
            }else if(parameter.ToString() == "EmptyField")
            {
                CurrentClient = null;
            }

            if (CurrentClient == null)
            {
                CurrentClient = new Client();
            }
        }
        #endregion

        #region Contractor Operation
        private ContractorViewModel contractorView;
        public ContractorViewModel ContractorView
        {
            get { return contractorView; }
            set
            {
                contractorView = value;
                RaisePropertyChanged("ContractorView");
            }
        }

        private Contractor currentContractor;
        public Contractor CurrentContractor
        {
            get { return currentContractor; }
            set { currentContractor = value; RaisePropertyChanged("CurrentContractor"); }
        }
        public ICommand BtnContractor { get; set; }

        private void OperationContrator(object parameter)
        {
            if (parameter.ToString() == "Add")
            {
                ContractorView.Add(CurrentContractor);
            }
            else if (parameter.ToString() == "Delete")
            {
                ContractorView.Delete(CurrentContractor);
                CurrentClient = null;
            }
            else if (parameter.ToString() == "Update")
            {
                ContractorView.Update(CurrentContractor);
                CurrentClient = null;
            }
            else if (parameter.ToString() == "Search")
            {
                CurrentContractor = ContractorView.Search(CurrentContractor);
            }
            else if (parameter.ToString() == "EmptyField")
            {
                CurrentContractor = null;
            }

            if (CurrentContractor == null)
            {
                CurrentContractor = new Contractor();
            }
        }
        #endregion

        #region Object Operation
        private ObjectViewModel objectView;
        public ObjectViewModel ObjectView
        {
            get { return objectView; }
            set { objectView = value; RaisePropertyChanged(nameof(ObjectView)); }
        }

        private Models.Object currentObject;
        public Models.Object CurrentObject
        {
            get { return currentObject; }
            set { currentObject = value; RaisePropertyChanged(nameof(CurrentObject)); }
        }

        public ICommand BtnObject { get; set; }

        private void OperationObject(object parameter)
        {
            if(parameter.ToString() == "Add")
            {
                ObjectView.Add(CurrentObject);
            }else if(parameter.ToString() == "Update")
            {
                ObjectView.Update(CurrentObject);
                CurrentObject = null;
            }else if(parameter.ToString() == "Delete")
            {
                ObjectView.Delete(CurrentObject);
                CurrentObject = null;
            }else if(parameter.ToString() == "Search")
            {
                CurrentObject = ObjectView.Search(CurrentObject);
            }else if(parameter.ToString() == "EmptyField")
            {
                CurrentObject = null;
            }

            if(CurrentObject == null)
            {
                CurrentObject = new Models.Object();
            }
        }
        #endregion

        #region Custom Operation
        private CustomViewModel customView;
        public CustomViewModel CustomView
        {
            get { return customView; }
            set { customView = value; RaisePropertyChanged(nameof(CustomView)); }
        }

        private DataTable resulQuery;
        public DataTable ResultQuery
        {
            get { return resulQuery; }
            set { resulQuery = value; RaisePropertyChanged(nameof(ResultQuery)); }
        }

        private Custom currentCustom;
        public Custom CurrentCustom
        {
            get { return currentCustom; }
            set { currentCustom = value; RaisePropertyChanged(nameof(CurrentCustom)); }
        }

        private string query;
        public string Query
        {
            get { return query; }
            set { query = value; }
        }

        public ICommand QueryChangeCommand { get;}
        public ICommand ResultCommand { get;}
        private void queryChanged()
        {
            ResultQuery = CustomView.ShowQueryResult(CurrentCustom.SelectedString);
        }

        private void resultChanged()
        {
            ResultQuery = CustomView.ShowQueryResult(Query);
        }
        #endregion
        public AppViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
            
            CurrentClient = new Client();
            BtnClient = new AllCommand(OperationClient);
            ClientView = new ClientViewModel();

            CurrentContractor = new Contractor();
            BtnContractor = new AllCommand(OperationContrator);
            ContractorView = new ContractorViewModel();

            CurrentObject = new Models.Object();
            BtnObject = new AllCommand(OperationObject);
            ObjectView = new ObjectViewModel();

            CurrentCustom = new Custom();
            CustomView = new CustomViewModel();
            var first = CustomView.Queries.First();
            CurrentCustom.StringS = first.Key;
            CurrentCustom.SelectedString = first.Value;
            QueryChangeCommand = new LoginCommand(queryChanged);
            ResultQuery = CustomView.ShowQueryResult(CurrentCustom.SelectedString);
            
            ResultCommand = new LoginCommand(resultChanged);
        }
    }
}
