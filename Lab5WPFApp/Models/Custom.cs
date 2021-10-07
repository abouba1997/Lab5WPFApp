using Lab5WPFApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5WPFApp.Models
{
    public class Custom : BaseViewModel
    {
        private string stringS; //Key
        public string StringS
        {
            get { return stringS; }
            set { stringS = value; RaisePropertyChanged(nameof(StringS)); }
        }

        private string selectedString; //Value
        public string SelectedString
        {
            get { return selectedString; }
            set { selectedString = value; RaisePropertyChanged(nameof(SelectedString)); }
        }

    }
}
