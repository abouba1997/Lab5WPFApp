using Lab5WPFApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab5WPFApp.ViewModels
{
    public class ContractorViewModel : BaseViewModel
    {
        private ObservableCollection<Contractor> contractors;
        public ObservableCollection<Contractor> Contractors
        {
            get { return contractors; }
            set { contractors = value; RaisePropertyChanged("Contractors"); }
        }

        private HashSet<Street> streets;
        public HashSet<Street> Streets
        {
            get { return streets; }
            set { streets = value; RaisePropertyChanged("Streets"); }
        }

        private HashSet<City> cities;
        public HashSet<City> Cities
        {
            get { return cities; }
            set { cities = value; RaisePropertyChanged("Cities"); }
        }

        private HashSet<Locality> localities;
        public HashSet<Locality> Localities
        {
            get { return localities; }
            set { localities = value; RaisePropertyChanged("Localities"); }
        }

        private HashSet<Region> regions;
        public HashSet<Region> Regions
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

        public ContractorViewModel()
        {
            DbConnection = new DatabaseConnect();
            DbConnection.Username = DatabaseStatic.Username;
            DbConnection.Password = DatabaseStatic.Password;
            Contractors = new ObservableCollection<Contractor>();
            Streets = new HashSet<Street>(new StreetNameComparer());
            Cities = new HashSet<City>(new CityNameComparer());
            Localities = new HashSet<Locality>(new LocalityNameComparer());
            Regions = new HashSet<Region>(new RegionNameComparer());
            Select();
        }

        private ObservableCollection<Contractor> Select()
        {
            string query = "Select Contractor_ID, Contractor_Name, Contractor_Contact_Person, Contractor_Phone, " +
                "Contractor_Home_Number, Contractor_Office_Number, street.Street_ID, Street_Name," +
                "city.City_ID, City_name, locality.Locality_ID, Locality_Name, region.Region_ID, Region_Name " +
                "from mknsangarea.contractor join street on contractor.Contractor_Street_ID = street.Street_ID " +
                "join city on contractor.Contractor_City_ID = city.City_ID " +
                "join locality on contractor.Contractor_Locality_ID = locality.Locality_ID " +
                "join region on contractor.Contractor_Region_ID = region.Region_ID order by Contractor_ID asc";

            if (DbConnection.OpenConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(query, DbConnection.Connection);
                MySqlDataReader dataReader = command.ExecuteReader();

                Contractor contractor = null;
                Street street = null;
                City city = null;
                Locality locality = null;
                Region region = null;

                Contractors.Clear();
                while (dataReader.Read())
                {
                    contractor = new Contractor();
                    street = new Street();
                    city = new City();
                    locality = new Locality();
                    region = new Region();

                    contractor.ContractorID = dataReader.GetInt32(0);
                    contractor.Name = dataReader.GetString(1);
                    contractor.ContactName = dataReader.GetString(2);
                    contractor.Phone = dataReader.GetString(3);
                    contractor.NumberHome = dataReader.GetString(4);
                    contractor.NumberOffice = dataReader.GetString(5);
                    street.StreetID = dataReader.GetInt32(6);
                    street.StreetName = dataReader.GetString(7);
                    city.CityID = dataReader.GetInt32(8);
                    city.CityName = dataReader.GetString(9);
                    locality.LocalityID = dataReader.GetInt32(10);
                    locality.LocalityName = dataReader.GetString(11);
                    region.RegionID = dataReader.GetInt32(12);
                    region.RegionName = dataReader.GetString(13);
                    contractor.Street_ = street;
                    contractor.City_ = city;
                    contractor.Locality_ = locality;
                    contractor.Region_ = region;
                    Contractors.Insert(0, contractor);
                    Streets.Add(street);
                    Cities.Add(city);
                    Localities.Add(locality);
                    Regions.Add(region);
                }
                dataReader.Close();
                DbConnection.CloseConnection();
                return Contractors;
            }
            else
            {
                return Contractors;
            }
        }
        public bool Add(Contractor contractorToAdd)
        {
            string query = null;
            if (contractorToAdd == null)
            {
                MessageBox.Show("Все поля должны быть заполнены");
                return false;
            }
            else if (String.IsNullOrEmpty(contractorToAdd.Name) || String.IsNullOrEmpty(contractorToAdd.ContactName) ||
               String.IsNullOrEmpty(contractorToAdd.Phone) || String.IsNullOrEmpty(contractorToAdd.NumberHome) ||
               String.IsNullOrEmpty(contractorToAdd.NumberOffice) || String.IsNullOrEmpty(contractorToAdd.Street_.StreetName) ||
               String.IsNullOrEmpty(contractorToAdd.City_.CityName) || String.IsNullOrEmpty(contractorToAdd.Locality_.LocalityName) ||
               String.IsNullOrEmpty(contractorToAdd.Region_.RegionName))
            {
                MessageBox.Show("Все поля должны быть заполнены");
                return false;
            }
            else
            {
                if (contractorToAdd.ContractorID != 0)
                {
                    query = "INSERT INTO mknsangarea.contractor (Contractor_ID, Contractor_Name, Contractor_Contact_Person, Contractor_Phone, Contractor_Home_Number," +
                        "Contractor_Office_Number, Contractor_Region_ID, Contractor_Locality_ID, Contractor_City_ID, Contractor_Street_ID) VALUES(" + contractorToAdd.ContractorID + ", '" + contractorToAdd.Name + "', '" + contractorToAdd.ContactName +
                        "', '" + contractorToAdd.Phone + "', '" + contractorToAdd.NumberHome + "', '" + contractorToAdd.NumberOffice + "', '" +
                        contractorToAdd.Region_.RegionID + "', '" + contractorToAdd.Locality_.LocalityID + "', '" + contractorToAdd.City_.CityID + "', '" + contractorToAdd.Street_.StreetID + "');";
                }
                else
                {
                    query = "INSERT INTO mknsangarea.contractor (Contractor_Name, Contractor_Contact_Person, Contractor_Phone, Contractor_Home_Number," +
                        "Contractor_Office_Number, Contractor_Region_ID, Contractor_Locality_ID, Contractor_City_ID, Contractor_Street_ID) VALUES('" + contractorToAdd.Name + "', '" + contractorToAdd.ContactName +
                        "', '" + contractorToAdd.Phone + "', '" + contractorToAdd.NumberHome + "', '" + contractorToAdd.NumberOffice + "', '" +
                        contractorToAdd.Region_.RegionID + "', '" + contractorToAdd.Locality_.LocalityID + "', '" + contractorToAdd.City_.CityID + "', '" + contractorToAdd.Street_.StreetID + "');";
                }
            }

            bool isAdded = false;
            foreach (Contractor contractor in Contractors.ToList())
            {
                if (contractorToAdd.ContractorID == contractor.ContractorID || contractorToAdd.ContactName == contractor.ContactName ||
                   contractorToAdd.Phone == contractor.Phone || contractorToAdd.Name == contractor.Name)
                {
                    isAdded = true;
                    break;
                }
            }

            if (!isAdded)
            {
                if (DbConnection.OpenConnection() == true)
                {
                    //create command and assign the query and connection from the constructor
                    try
                    {
                        MySqlCommand cmd = new MySqlCommand(query, DbConnection.Connection);

                        //Execute command
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
        public bool Delete(Contractor contractorToDelete)
        {
            string query = null;
            if (contractorToDelete == null || contractorToDelete.ContractorID == 0)
            {
                MessageBox.Show("Заполни поле ID (Удаление идет только по ID)");
                return false;
            }

            bool isExisted = false;
            foreach (Contractor contractor in Contractors.ToList())
            {
                if (contractorToDelete.ContractorID == contractor.ContractorID)
                {
                    isExisted = true;
                    break;
                }
            }

            if (isExisted)
            {
                query = "DELETE FROM mknsangarea.contractor WHERE Contractor_ID =" + contractorToDelete.ContractorID + ";";

                if (DbConnection.OpenConnection() == true)
                {
                    try
                    {
                        MySqlCommand cmd = new MySqlCommand(query, DbConnection.Connection);

                        //Execute command
                        cmd.ExecuteNonQuery();
                        DbConnection.CloseConnection();
                        Select();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                        isExisted = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Не существует такого подрядчика");
                isExisted = false;
            }

            return isExisted;
        }
        public bool Update(Contractor contractorToUpdate)
        {
            string query = null;
            if (contractorToUpdate == null)
            {
                MessageBox.Show("Заполни поля");
                return false;
            }

            bool isUpdated = false;
            foreach (Contractor contractor in Contractors.ToList())
            {
                if (contractorToUpdate.ContractorID == contractor.ContractorID || contractorToUpdate.Name == contractor.Name ||
                    contractorToUpdate.ContactName == contractor.ContactName)
                {
                    isUpdated = true;
                    break;
                }
            }

            if (isUpdated)
            {
                if (contractorToUpdate.ContractorID != 0)
                {
                    query = "UPDATE mknsangarea.contractor SET Contractor_Name = '" + contractorToUpdate.Name + "', Contractor_Contact_Person = '" +
                        contractorToUpdate.ContactName + "', Contractor_Phone = '" + contractorToUpdate.Phone + "', Contractor_Home_Number = '" +
                        contractorToUpdate.NumberHome + "', Contractor_Office_Number = '" + contractorToUpdate.NumberOffice + "', Contractor_Region_ID = '" +
                        contractorToUpdate.Region_.RegionID + "', Contractor_Locality_ID = '" + contractorToUpdate.Locality_.LocalityID + "', Contractor_City_ID = '" + contractorToUpdate.City_.CityID +
                        "', Contractor_Street_ID = '" + contractorToUpdate.Street_.StreetID + "' WHERE Contractor_ID = " + contractorToUpdate.ContractorID + ";";
                }
                else if (!String.IsNullOrEmpty(contractorToUpdate.Name))
                {
                    query = "UPDATE mknsangarea.contractor SET Contractor_Name = '" + contractorToUpdate.Name + "', Contractor_Contact_Person = '" +
                        contractorToUpdate.ContactName + "', Contractor_Phone = '" + contractorToUpdate.Phone + "', Contractor_Home_Number = '" +
                        contractorToUpdate.NumberHome + "', Contractor_Office_Number = '" + contractorToUpdate.NumberOffice + "', Contractor_Region_ID = '" +
                        contractorToUpdate.Region_.RegionID + "', Contractor_Locality_ID = '" + contractorToUpdate.Locality_.LocalityID + "', Contractor_City_ID = '" +
                        contractorToUpdate.City_.CityID + "', Contractor_Street_ID = '" + contractorToUpdate.Street_.StreetID + "' WHERE Contractor_Name = '" +
                        contractorToUpdate.Name + "';";
                }
                else if (!String.IsNullOrEmpty(contractorToUpdate.ContactName))
                {
                    query = "UPDATE mknsangarea.contractor SET Contractor_Name = '" + contractorToUpdate.Name + "', Contractor_Contact_Person = '" +
                        contractorToUpdate.ContactName + "', Contractor_Phone = '" + contractorToUpdate.Phone + "', Contractor_Home_Number = '" +
                        contractorToUpdate.NumberHome + "', Contractor_Office_Number = '" + contractorToUpdate.NumberOffice + "', Contractor_Region_ID = '" +
                        contractorToUpdate.Region_.RegionID + "', Contractor_Locality_ID = '" + contractorToUpdate.Locality_.LocalityID + "', Contractor_City_ID = '" +
                        contractorToUpdate.City_.CityID + "', Contractor_Street_ID = '" + contractorToUpdate.Street_.StreetID + "' WHERE Contractor_Contact_Person = '" +
                        contractorToUpdate.ContactName + "';";
                }

                if (DbConnection.OpenConnection() == true)
                {
                    try
                    {
                        //create command and assign the query and connection from the constructor
                        MySqlCommand cmd = new MySqlCommand(query, DbConnection.Connection);

                        //Execute command
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
                MessageBox.Show("Не существует такого подрядчика");
            }
            return isUpdated;
        }
        public Contractor Search(Contractor contractorToSearch)
        {

            Contractor contractor = null;
            if (contractorToSearch == null)
            {
                MessageBox.Show("Заполни либо ID, Название, Контакное лицо или Телефон");
                return contractor;
            }

            if (contractorToSearch.ContractorID == 0 && String.IsNullOrEmpty(contractorToSearch.Name) &&
               String.IsNullOrEmpty(contractorToSearch.ContactName) && String.IsNullOrEmpty(contractorToSearch.Phone))
            {
                MessageBox.Show("Заполни либо ID, Название, Контакное лицо или Телефон");
                return contractor;
            }

            foreach (Contractor contractor1 in Contractors.ToList())
            {
                if (contractor1.ContractorID == contractorToSearch.ContractorID || contractor1.Name == contractorToSearch.Name || contractor1.ContactName == contractorToSearch.ContactName)
                {
                    contractor = contractor1;
                    break;
                }
            }

            if (contractor == null)
            {
                MessageBox.Show("Подрядчик не найден");
            }

            return contractor;
        }
    }
}
