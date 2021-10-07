using Lab5WPFApp.ViewModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5WPFApp.Models
{
    public class DatabaseConnect : BaseViewModel
    {
        private MySqlConnection connection;
        public MySqlConnection Connection
        {
            get { return connection; }
            set { connection = value; RaisePropertyChanged("Connection"); }
        }

        private string username;
        public string Username
        {
            get { return username; }
            set { username = value; RaisePropertyChanged("Username"); }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; RaisePropertyChanged("Password"); }
        }

        private bool isConnected;
        public bool IsConnected
        {
            get { return isConnected; }
            set { isConnected = value; RaisePropertyChanged("IsConnected"); }
        }

        private ObservableCollection<string> log = new ObservableCollection<string>();
        public ObservableCollection<string> LOG
        {
            get { return log; }
            set { log = value; }
        }
        public bool OpenConnection()
        {
            try
            {
                MySqlConnectionStringBuilder _connectionString = new MySqlConnectionStringBuilder();
                connection = null;
                _connectionString.AllowBatch = true;
                _connectionString.Server = "localhost";
                _connectionString.Database = "mknsangarea";
                _connectionString.UserID = Username;
                _connectionString.ConnectionTimeout = 30;
                _connectionString.Password = Password;

                connection = new MySqlConnection(_connectionString.ConnectionString);
                connection.Open();
                IsConnected = true;
                LOG.Clear();
                LOG.Add("Connexion to database successfully!");
                return IsConnected;
            }
            catch (MySqlException ex)
            {
                LOG.Add(ex.Message);
                IsConnected = false;
                return IsConnected;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                if (connection != null)
                {
                    connection.Close();
                }
                return true;
            }
            catch (MySqlException)
            {
                return false;
            }
        }
    }
}
