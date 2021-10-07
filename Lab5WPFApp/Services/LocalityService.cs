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
    public class LocalityService : BaseViewModel
    {
        private ObservableCollection<Locality> localities;
        public ObservableCollection<Locality> Localities
        {
            get { return localities; }
            set { localities = value; RaisePropertyChanged("Localities"); }
        }

        private DatabaseConnect dbConnection;
        public DatabaseConnect DbConnection
        {
            get { return dbConnection; }
            set { dbConnection = value; RaisePropertyChanged("DbConnection"); }
        }

        public LocalityService()
        {
            DbConnection = new DatabaseConnect();
            DbConnection.Username = DatabaseStatic.Username;
            DbConnection.Password = DatabaseStatic.Password;
            Localities = new ObservableCollection<Locality>();
            Select();
        }

        public ObservableCollection<Locality> Select()
        {
            string query = "SELECT * FROM mknsangarea.locality;";

            if (DbConnection.OpenConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(query, DbConnection.Connection);
                MySqlDataReader dataReader = command.ExecuteReader();

                Locality locality = null;
                Localities.Clear();
                while (dataReader.Read())
                {
                    locality = new Locality();
                    locality.LocalityID = dataReader.GetInt32(0);
                    locality.LocalityName = dataReader.GetString(1);
                    Localities.Insert(0, locality);
                }

                dataReader.Close();
                DbConnection.CloseConnection();
                return Localities;
            }
            else
            {
                return Localities;
            }
        }

        public bool Add(Locality localityToAdd)
        {
            string query = null;
            if (String.IsNullOrEmpty(localityToAdd.LocalityName))
            {
                MessageBox.Show("хотя Поле Название должно быть заполнено");
                return false;
            }

            bool isAdded = false;
            foreach (Locality locality in Localities.ToList())
            {
                if (locality.LocalityID == localityToAdd.LocalityID || locality.LocalityName == localityToAdd.LocalityName)
                {
                    isAdded = true;
                    break;
                }
            }

            if (!isAdded)
            {
                query = "INSERT INTO mknsangarea.locality (Locality_Name) VALUES ('" + localityToAdd.LocalityName + "');";
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

        public bool Update(Locality localityToUpdate)
        {
            string query = null;
            if (localityToUpdate.LocalityID == 0 && String.IsNullOrEmpty(localityToUpdate.LocalityName))
            {
                MessageBox.Show("Поле ID или Название должно быть заполнено");
                return false;
            }

            bool isUpdated = false;
            foreach (Locality locality in Localities.ToList())
            {
                if (locality.LocalityID == localityToUpdate.LocalityID)
                {
                    isUpdated = true;
                    break;
                }
            }

            if (isUpdated)
            {
                if (String.IsNullOrEmpty(localityToUpdate.LocalityName) == false)
                {
                    query = "UPDATE mknsangarea.locality SET Locality_Name = '" + localityToUpdate.LocalityName +
                        "' WHERE(Locality_ID = '" + localityToUpdate.LocalityID + "');";
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

        public bool Delete(Locality localityToDelete)
        {
            string query = null;
            if (localityToDelete.LocalityID == 0 && String.IsNullOrEmpty(localityToDelete.LocalityName))
            {
                MessageBox.Show("Хотя одно поле должно быть заполнено");
                return false;
            }

            bool isDeleted = false;
            foreach (Locality locality in Localities.ToList())
            {
                if (locality.LocalityID == localityToDelete.LocalityID || localityToDelete.LocalityName == localityToDelete.LocalityName)
                {
                    isDeleted = true;
                    break;
                }
            }

            if (isDeleted)
            {
                if (localityToDelete.LocalityID != 0)
                {
                    query = "DELETE FROM mknsangarea.locality WHERE(Locality_ID = '" + localityToDelete.LocalityID + "');";
                }
                else if (!String.IsNullOrEmpty(localityToDelete.LocalityName))
                {
                    query = "DELETE FROM mknsangarea.locality WHERE(Locality_Name = '" + localityToDelete.LocalityName + "');";
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

        public Locality Search(Locality localityToSearch)
        {
            Locality locality = null;
            if (localityToSearch.LocalityID == 0 && String.IsNullOrEmpty(localityToSearch.LocalityName))
            {
                MessageBox.Show("Заполните поле");
                return locality;
            }

            foreach (Locality locality1 in Localities.ToList())
            {
                if (locality1.LocalityID == localityToSearch.LocalityID || locality1.LocalityName == localityToSearch.LocalityName)
                {
                    locality = locality1;
                    break;
                }
            }
            return locality;
        }
    }
}
