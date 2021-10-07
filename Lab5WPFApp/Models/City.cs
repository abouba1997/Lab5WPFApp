using Lab5WPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5WPFApp.Models
{
    public class City : BaseViewModel
    {
        private int cityID;
        public int CityID
        {
            get { return cityID; }
            set { cityID = value; RaisePropertyChanged("CityID"); }
        }

        private string cityName;
        public string CityName
        {
            get { return cityName; }
            set { cityName = value; RaisePropertyChanged("CityName"); }
        }
    }

    public class CityNameComparer : IEqualityComparer<City>
    {
        public bool Equals(City x, City y)
        {
            return x.CityName.Equals(y.CityName, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(City obj)
        {
            return obj.CityName.GetHashCode();
        }
    }
}
