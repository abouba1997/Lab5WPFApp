using Lab5WPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5WPFApp.Models
{
    public class Street : BaseViewModel
    {
        private int streetID;
        public int StreetID
        {
            get { return streetID; }
            set { streetID = value; RaisePropertyChanged("StreetID"); }
        }

        private string streetName;
        public string StreetName
        {
            get { return streetName; }
            set { streetName = value; RaisePropertyChanged("StreetName"); }
        }
    }

    public class StreetNameComparer : IEqualityComparer<Street>
    {
        public bool Equals(Street x, Street y)
        {
            return x.StreetName.Equals(y.StreetName, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(Street obj)
        {
            return obj.StreetName.GetHashCode();
        }
    }
}
