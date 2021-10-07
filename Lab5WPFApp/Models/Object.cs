using Lab5WPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5WPFApp.Models
{
    public class Object : BaseViewModel
    {
        private int objectID;
        public int ObjectID
        {
            get { return objectID; }
            set { objectID = value; RaisePropertyChanged(nameof(ObjectID)); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(nameof(Name)); }
        }

        private string cost;
        public string Cost
        {
            get { return cost; }
            set { cost = value; RaisePropertyChanged(nameof(Cost)); }
        }

        private DateTime dateBegin;
        public DateTime DateBegin
        {
            get { return dateBegin; }
            set { dateBegin = value; RaisePropertyChanged(nameof(DateBegin)); }
        }

        private DateTime dateEnd;
        public DateTime DateEnd
        {
            get { return dateEnd; }
            set { dateEnd = value; RaisePropertyChanged(nameof(DateEnd)); }
        }

        private Client client;
        public Client Client
        {
            get { return client; }
            set { client = value; RaisePropertyChanged(nameof(Client)); }
        }

        private Contractor contractor;
        public Contractor Contractor
        {
            get { return contractor; }
            set { contractor = value; RaisePropertyChanged(nameof(Contractor)); }
        }
    }
}
