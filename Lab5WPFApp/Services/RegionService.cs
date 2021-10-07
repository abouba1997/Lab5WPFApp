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
    public class RegionService : BaseViewModel
    {
        private ObservableCollection<Region> regions;
        public ObservableCollection<Region> Regions
        {
            get { return regions; }
            set { regions = value; RaisePropertyChanged("Regions"); }
        }

        private DatabaseConnect dbConnection;
        public DatabaseConnect DbConnection
        {
            get { return dbConnection; }
            set { dbConnection = value; RaisePropertyChanged("DbConnection"); }
        }

        public RegionService()
        {
            DbConnection = new DatabaseConnect();
            DbConnection.Username = DatabaseStatic.Username;
            DbConnection.Password = DatabaseStatic.Password;
            Regions = new ObservableCollection<Region>();
            Select();
        }

        public ObservableCollection<Region> Select()
        {
            string query = "SELECT * FROM mknsangarea.region;";

            if (DbConnection.OpenConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(query, DbConnection.Connection);
                MySqlDataReader dataReader = command.ExecuteReader();

                Region region = null;
                Regions.Clear();
                while (dataReader.Read())
                {
                    region = new Region();
                    region.RegionID = dataReader.GetInt32(0);
                    region.RegionName = dataReader.GetString(1);
                    Regions.Insert(0, region);
                }

                dataReader.Close();
                DbConnection.CloseConnection();
                return Regions;
            }
            else
            {
                return Regions;
            }
        }

        public bool Add(Region regionToAdd)
        {
            string query = null;
            if (String.IsNullOrEmpty(regionToAdd.RegionName))
            {
                MessageBox.Show("хотя Поле Название должно быть заполнено");
                return false;
            }

            bool isAdded = false;
            foreach (Region region in Regions.ToList())
            {
                if (region.RegionID == regionToAdd.RegionID || region.RegionName == regionToAdd.RegionName)
                {
                    isAdded = true;
                    break;
                }
            }

            if (!isAdded)
            {
                query = "INSERT INTO mknsangarea.region (Region_Name) VALUES ('" + regionToAdd.RegionName + "');";
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

        public bool Update(Region regionToUpdate)
        {
            string query = null;
            if (regionToUpdate.RegionID == 0 && String.IsNullOrEmpty(regionToUpdate.RegionName))
            {
                MessageBox.Show("Поле ID или Название должно быть заполнено");
                return false;
            }

            bool isUpdated = false;
            foreach (Region region in Regions.ToList())
            {
                if (region.RegionID == regionToUpdate.RegionID)
                {
                    isUpdated = true;
                    break;
                }
            }

            if (isUpdated)
            {
                if (String.IsNullOrEmpty(regionToUpdate.RegionName) == false)
                {
                    query = "UPDATE mknsangarea.region SET Region_Name = '" + regionToUpdate.RegionName +
                        "' WHERE(Region_ID = '" + regionToUpdate.RegionID + "');";
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

        public bool Delete(Region regionToDelete)
        {
            string query = null;
            if (regionToDelete.RegionID == 0 && String.IsNullOrEmpty(regionToDelete.RegionName))
            {
                MessageBox.Show("Хотя одно поле должно быть заполнено");
                return false;
            }

            bool isDeleted = false;
            foreach (Region region in Regions.ToList())
            {
                if (region.RegionID == regionToDelete.RegionID || regionToDelete.RegionName == regionToDelete.RegionName)
                {
                    isDeleted = true;
                    break;
                }
            }

            if (isDeleted)
            {
                if (regionToDelete.RegionID != 0)
                {
                    query = "DELETE FROM mknsangarea.region WHERE(Region_ID = '" + regionToDelete.RegionID + "');";
                }
                else if (!String.IsNullOrEmpty(regionToDelete.RegionName))
                {
                    query = "DELETE FROM mknsangarea.region WHERE(Region_Name = '" + regionToDelete.RegionName + "');";
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

        public Region Search(Region regionToSearch)
        {
            Region region = null;
            if (regionToSearch.RegionID == 0 && String.IsNullOrEmpty(regionToSearch.RegionName))
            {
                MessageBox.Show("Заполните поле");
                return region;
            }

            foreach (Region region1 in Regions.ToList())
            {
                if (region1.RegionID == regionToSearch.RegionID || region1.RegionName == regionToSearch.RegionName)
                {
                    region = region1;
                    break;
                }
            }
            return region;
        }
    }
}
