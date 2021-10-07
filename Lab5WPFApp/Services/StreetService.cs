using Lab5WPFApp.Models;
using Lab5WPFApp.ViewModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab5WPFApp.Services
{
    public class StreetService : BaseViewModel
    {
        private ObservableCollection<Street> streets;
        public ObservableCollection<Street> Streets
        {
            get { return streets; }
            set { streets = value; RaisePropertyChanged("Clients"); }
        }

        private DatabaseConnect dbConnection;
        public DatabaseConnect DbConnection
        {
            get { return dbConnection; }
            set { dbConnection = value; RaisePropertyChanged("DbConnection"); }
        }

        public StreetService()
        {
            DbConnection = new DatabaseConnect();
            DbConnection.Username = DatabaseStatic.Username;
            DbConnection.Password = DatabaseStatic.Password;
            Streets = new ObservableCollection<Street>();
            Select();
        }

        public ObservableCollection<Street> Select()
        {
            string query = "SELECT * FROM mknsangarea.street;";

            if (DbConnection.OpenConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(query, DbConnection.Connection);
                MySqlDataReader dataReader = command.ExecuteReader();

                Street street = null;
                Streets.Clear();
                while (dataReader.Read())
                {
                    street = new Street();
                    street.StreetID = dataReader.GetInt32(0);
                    street.StreetName = dataReader.GetString(1);
                    Streets.Insert(0, street);
                }

                dataReader.Close();
                DbConnection.CloseConnection();
                return Streets;
            }
            else
            {
                return Streets;
            }
        }

        public bool Add(Street streetToAdd)
        {
            string query = null;
            if (String.IsNullOrEmpty(streetToAdd.StreetName))
            {
                MessageBox.Show("хотя Поле Название должно быть заполнено");
                return false;
            }

            bool isAdded = false;
            foreach (Street street in Streets.ToList())
            {
                if (street.StreetID == streetToAdd.StreetID || street.StreetName == streetToAdd.StreetName)
                {
                    isAdded = true;
                    break;
                }
            }

            if (!isAdded)
            {
                query = "INSERT INTO mknsangarea.street (Street_Name) VALUES ('" + streetToAdd.StreetName + "');";
                if (DbConnection.OpenConnection() == true)
                {
                    try
                    {
                        MySqlCommand cmd = new MySqlCommand(query, DbConnection.Connection);
                        cmd.ExecuteNonQuery();
                        DbConnection.CloseConnection();
                        Select();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                        isAdded = false;
                    }
                }
            }
            return isAdded;
        }

        public bool Update(Street streetToUpdate)
        {
            string query = null;
            if (streetToUpdate.StreetID == 0 && String.IsNullOrEmpty(streetToUpdate.StreetName))
            {
                MessageBox.Show("Поле ID или Название должно быть заполнено");
                return false;
            }

            bool isUpdated = false;
            foreach (Street street in Streets.ToList())
            {
                if (street.StreetID == streetToUpdate.StreetID)
                {
                    isUpdated = true;
                    break;
                }
            }

            if (isUpdated)
            {
                if(String.IsNullOrEmpty(streetToUpdate.StreetName) == false)
                {
                    query = "UPDATE mknsangarea.street SET Street_Name = '" + streetToUpdate.StreetName +
                        "' WHERE Street_ID = " + streetToUpdate.StreetID + ";";
                    if (DbConnection.OpenConnection() == true)
                    {
                        try
                        {
                            MySqlCommand cmd = new MySqlCommand(query, DbConnection.Connection);
                            cmd.ExecuteNonQuery();
                            DbConnection.CloseConnection();
                            Select();
                        }
                        catch (MySqlException ex)
                        {
                            MessageBox.Show(ex.Message);
                            isUpdated = false;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Название не заполнено");
                }
            }

            return isUpdated;
        }

        public bool Delete(Street streetToDelete)
        {
            string query = null;
            if (streetToDelete.StreetID == 0 && String.IsNullOrEmpty(streetToDelete.StreetName))
            {
                MessageBox.Show("Хотя одно поле должно быть заполнено");
                return false;
            }

            bool isDeleted = false;
            foreach (Street street in Streets.ToList())
            {
                if (street.StreetID == streetToDelete.StreetID || streetToDelete.StreetName == streetToDelete.StreetName)
                {
                    isDeleted = true;
                    break;
                }
            }

            if (isDeleted)
            {
                if (streetToDelete.StreetID != 0)
                {
                    query = "DELETE FROM mknsangarea.street WHERE Street_ID = " + streetToDelete.StreetID + ";";
                }
                else if (!String.IsNullOrEmpty(streetToDelete.StreetName))
                {
                    query = "DELETE FROM mknsangarea.street WHERE Street_Name = '" + streetToDelete.StreetName + "';";
                }

                if (DbConnection.OpenConnection() == true)
                {
                    try
                    {
                        MySqlCommand cmd = new MySqlCommand(query, DbConnection.Connection);
                        cmd.ExecuteNonQuery();
                        DbConnection.CloseConnection();
                        Select();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                        isDeleted = false;
                    }
                }
            }
            return isDeleted;
        }

        public Street Search(Street streetToSearch)
        {
            Street street = null;
            if (streetToSearch.StreetID == 0 && String.IsNullOrEmpty(streetToSearch.StreetName))
            {
                MessageBox.Show("Заполните поле");
                return street;
            }

            foreach (Street street1 in Streets.ToList())
            {
                if (street1.StreetID == streetToSearch.StreetID || street1.StreetName == streetToSearch.StreetName)
                {
                    street = street1;
                    break;
                }
            }
            return street;
        }
    }
}
