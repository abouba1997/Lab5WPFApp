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
    public class CityService : BaseViewModel
    {
        private ObservableCollection<City> cities;
        public ObservableCollection<City> Cities
        {
            get { return cities; }
            set { cities = value; RaisePropertyChanged("Clients"); }
        }

        private DatabaseConnect dbConnection;
        public DatabaseConnect DbConnection
        {
            get { return dbConnection; }
            set { dbConnection = value; RaisePropertyChanged("DbConnection"); }
        }

        public CityService()
        {
            DbConnection = new DatabaseConnect();
            DbConnection.Username = DatabaseStatic.Username;
            DbConnection.Password = DatabaseStatic.Password;
            Cities = new ObservableCollection<City>();
            Select();
        }

        public ObservableCollection<City> Select()
        {
            string query = "SELECT * FROM mknsangarea.city;";

            if (DbConnection.OpenConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(query, DbConnection.Connection);
                MySqlDataReader dataReader = command.ExecuteReader();

                City city = null;
                Cities.Clear();
                while (dataReader.Read())
                {
                    city = new City();
                    city.CityID = dataReader.GetInt32(0);
                    city.CityName = dataReader.GetString(1);
                    Cities.Insert(0, city);
                }

                dataReader.Close();
                DbConnection.CloseConnection();
                return Cities;
            }
            else
            {
                return Cities;
            }
        }

        public bool Add(City cityToAdd)
        {
            string query = null;
            if (String.IsNullOrEmpty(cityToAdd.CityName))
            {
                MessageBox.Show("хотя Поле Название должно быть заполнено");
                return false;
            }

            bool isAdded = false;
            foreach (City city in Cities.ToList())
            {
                if (city.CityID == cityToAdd.CityID || city.CityName == cityToAdd.CityName)
                {
                    isAdded = true;
                    break;
                }
            }

            if (!isAdded)
            {
                query = "INSERT INTO mknsangarea.city (City_Name) VALUES ('" + cityToAdd.CityName + "');";
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

        public bool Update(City cityToUpdate)
        {
            string query = null;
            if (cityToUpdate.CityID == 0 && String.IsNullOrEmpty(cityToUpdate.CityName))
            {
                MessageBox.Show("Поле ID или Название должно быть заполнено");
                return false;
            }

            bool isUpdated = false;
            foreach (City city in Cities.ToList())
            {
                if (city.CityID == cityToUpdate.CityID)
                {
                    isUpdated = true;
                    break;
                }
            }

            if (isUpdated)
            {
                if (String.IsNullOrEmpty(cityToUpdate.CityName) == false)
                {
                    query = "UPDATE mknsangarea.city SET City_Name = '" + cityToUpdate.CityName +
                        "' WHERE(City_ID = '" + cityToUpdate.CityID + "');";
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

        public bool Delete(City cityToDelete)
        {
            string query = null;
            if (cityToDelete.CityID == 0 && String.IsNullOrEmpty(cityToDelete.CityName))
            {
                MessageBox.Show("Хотя одно поле должно быть заполнено");
                return false;
            }

            bool isDeleted = false;
            foreach (City city in Cities.ToList())
            {
                if (city.CityID == cityToDelete.CityID || cityToDelete.CityName == cityToDelete.CityName)
                {
                    isDeleted = true;
                    break;
                }
            }

            if (isDeleted)
            {
                if (cityToDelete.CityID != 0)
                {
                    query = "DELETE FROM mknsangarea.city WHERE(City_ID = '" + cityToDelete.CityID + "');";
                }
                else if (!String.IsNullOrEmpty(cityToDelete.CityName))
                {
                    query = "DELETE FROM mknsangarea.city WHERE(City_Name = '" + cityToDelete.CityName + "');";
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

        public City Search(City cityToSearch)
        {
            City city = null;
            if (cityToSearch.CityID == 0 && String.IsNullOrEmpty(cityToSearch.CityName))
            {
                MessageBox.Show("Заполните поле");
                return city;
            }

            foreach (City city1 in Cities.ToList())
            {
                if (city1.CityID == cityToSearch.CityID || city1.CityName == cityToSearch.CityName)
                {
                    city = city1;
                    break;
                }
            }
            return city;
        }
    }
}
