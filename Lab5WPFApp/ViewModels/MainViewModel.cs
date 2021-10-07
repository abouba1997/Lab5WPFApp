using Lab5WPFApp.Commands;
using Lab5WPFApp.Models;
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
    public class MainViewModel : BaseViewModel
    {
        private AppViewModel appView;
        public AppViewModel AppView
        {
            get { return appView; }
            set { appView = value; RaisePropertyChanged("AppView"); }
        }
        private DatabaseConnect dbConnect;
        public DatabaseConnect DbConnect
        {
            get => dbConnect;
            set { dbConnect = value; RaisePropertyChanged("DbConnect"); }
        }
        public ICommand LoginCommand { get; }
        public MainViewModel()
        {
            DbConnect = new DatabaseConnect();
            LoginCommand = new LoginCommand(Connection);
        }

        private void Connection()
        {
            try
            {
                bool isConnected = DbConnect.OpenConnection();
                if(isConnected == true)
                {
                    AppView = new AppViewModel();
                    DatabaseStatic.Username = DbConnect.Username;
                    string password = DbConnect.Password;
                    DatabaseStatic.Password = password;
                }
                else
                {
                    AppView = null;
                }
            }
            catch (Exception)
            {
                if(DbConnect.Connection != null)
                {
                    DbConnect.CloseConnection();
                }
            }
        }
    }
}
