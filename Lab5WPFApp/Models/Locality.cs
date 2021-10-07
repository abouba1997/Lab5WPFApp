using Lab5WPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5WPFApp.Models
{
    public class Locality : BaseViewModel
    {
        private int localityID;
        public int LocalityID
        {
            get { return localityID; }
            set { localityID = value; RaisePropertyChanged("LocalityID"); }
        }

        private string localityName;
        public string LocalityName
        {
            get { return localityName; }
            set { localityName = value; RaisePropertyChanged("LocalityName"); }
        }
    }

    public class LocalityNameComparer : IEqualityComparer<Locality>
    {
        public bool Equals(Locality x, Locality y)
        {
            return x.LocalityName.Equals(y.LocalityName, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(Locality obj)
        {
            return obj.LocalityName.GetHashCode();
        }
    }
}
