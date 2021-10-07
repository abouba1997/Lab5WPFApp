using Lab5WPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5WPFApp.Models
{
    public class Region : BaseViewModel
    {
        private int regionID;
        public int RegionID
        {
            get { return regionID; }
            set { regionID = value; RaisePropertyChanged("RegionID"); }
        }

        private string regionName;
        public string RegionName
        {
            get { return regionName; }
            set { regionName = value; RaisePropertyChanged("RegionName"); }
        }
    }

    public class RegionNameComparer : IEqualityComparer<Region>
    {
        public bool Equals(Region x, Region y)
        {
            return x.RegionName.Equals(y.RegionName, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(Region obj)
        {
            return obj.RegionName.GetHashCode();
        }
    }
}
