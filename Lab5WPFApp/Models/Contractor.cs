using Lab5WPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5WPFApp.Models
{
    public class Contractor : BaseViewModel
    {
        private int contractorID;
        public int ContractorID
        {
            get { return contractorID; }
            set { contractorID = value; RaisePropertyChanged("ContractorID"); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged("Name"); }
        }

        private string contactName;
        public string ContactName
        {
            get { return contactName; }
            set { contactName = value; RaisePropertyChanged("ContactName"); }
        }

        private string phone;
        public string Phone
        {
            get { return phone; }
            set { phone = value; RaisePropertyChanged("Phone"); }
        }

        private string numberHome;
        public string NumberHome
        {
            get { return numberHome; }
            set { numberHome = value; RaisePropertyChanged("NumberHome"); }
        }

        private string numberOffice;
        public string NumberOffice
        {
            get { return numberOffice; }
            set { numberOffice = value; RaisePropertyChanged("NumberOffice"); }
        }

        private Street street;
        public Street Street_
        {
            get { return street; }
            set { street = value; RaisePropertyChanged("Street_"); }
        }

        private City city;
        public City City_
        {
            get { return city; }
            set { city = value; RaisePropertyChanged("City_"); }
        }

        private Locality locality;
        public Locality Locality_
        {
            get { return locality; }
            set { locality = value; RaisePropertyChanged("Locality_"); }
        }

        private Region region;
        public Region Region_
        {
            get { return region; }
            set { region = value; RaisePropertyChanged("Region_"); }
        }
    }
}
