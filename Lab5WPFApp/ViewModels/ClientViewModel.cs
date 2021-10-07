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
    public class ClientViewModel : BaseViewModel
    {
        private ObservableCollection<Client> clients;
        public ObservableCollection<Client> Clients
        {
            get { return clients; }
            set { clients = value; RaisePropertyChanged("Clients"); }
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
            set { localities = value;  RaisePropertyChanged("Localities"); }
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
        public ClientViewModel()
        {
            DbConnection = new DatabaseConnect();
            DbConnection.Username = DatabaseStatic.Username;
            DbConnection.Password = DatabaseStatic.Password;
            Clients = new ObservableCollection<Client>();
            Streets = new HashSet<Street>(new StreetNameComparer());
            Cities = new HashSet<City>(new CityNameComparer());
            Localities = new HashSet<Locality>(new LocalityNameComparer());
            Regions = new HashSet<Region>(new RegionNameComparer());
            Select();
        }
        private ObservableCollection<Client> Select()
        {
            string query = "Select Client_ID, Client_Name, Client_Contact_Person, Client_Phone," +
                "Client_Home_Number, Client_Office_Number, street.Street_ID, Street_Name, " +
                "city.City_ID, City_name, locality.Locality_ID, Locality_Name, region.Region_ID, Region_Name " +
                "from mknsangarea.client join street on client.Street_ID = street.Street_ID " +
                "join city on client.City_ID = city.City_ID " +
                "join locality on client.Locality_ID = locality.Locality_ID " +
                "join region on client.Region_ID = region.Region_ID order by Client_ID asc";

            if (DbConnection.OpenConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(query, DbConnection.Connection);
                MySqlDataReader dataReader = command.ExecuteReader();

                Client client = null;
                Street street = null;
                City city = null;
                Locality locality = null;
                Region region = null;

                Clients.Clear();
                while (dataReader.Read())
                {
                    client = new Client();
                    street = new Street();
                    city = new City();
                    locality = new Locality();
                    region = new Region();

                    client.ClientID = dataReader.GetInt32(0);
                    client.Name = dataReader.GetString(1);
                    client.ContactName = dataReader.GetString(2);
                    client.Phone = dataReader.GetString(3);
                    client.NumberHome = dataReader.GetString(4);
                    client.NumberOffice = dataReader.GetString(5);
                    street.StreetID = dataReader.GetInt32(6);
                    street.StreetName = dataReader.GetString(7);
                    city.CityID = dataReader.GetInt32(8);
                    city.CityName = dataReader.GetString(9);
                    locality.LocalityID = dataReader.GetInt32(10);
                    locality.LocalityName = dataReader.GetString(11);
                    region.RegionID = dataReader.GetInt32(12);
                    region.RegionName = dataReader.GetString(13);
                    client.Street_ = street;
                    client.City_ = city;
                    client.Locality_ = locality;
                    client.Region_ = region;
                    Clients.Insert(0, client);
                    Streets.Add(street);
                    Cities.Add(city);
                    Localities.Add(locality);
                    Regions.Add(region);
                }
                dataReader.Close();
                DbConnection.CloseConnection();
                return Clients;
            }
            else
            {
                return Clients;
            }
        }
        public bool Add(Client clientToAdd)
        {
            string query = null;
            if (clientToAdd == null)
            {
                MessageBox.Show("Все поля должны быть заполнены");
                return false;
            }
            else if (String.IsNullOrEmpty(clientToAdd.Name) || String.IsNullOrEmpty(clientToAdd.ContactName) ||
               String.IsNullOrEmpty(clientToAdd.Phone) || String.IsNullOrEmpty(clientToAdd.NumberHome) ||
               String.IsNullOrEmpty(clientToAdd.NumberOffice) || String.IsNullOrEmpty(clientToAdd.Street_.StreetName) ||
               String.IsNullOrEmpty(clientToAdd.City_.CityName) || String.IsNullOrEmpty(clientToAdd.Locality_.LocalityName) || 
               String.IsNullOrEmpty(clientToAdd.Region_.RegionName))
            {
                MessageBox.Show("Все поля должны быть заполнены");
                return false;
            }
            else
            {
                if (clientToAdd.ClientID != 0)
                {
                    query = "INSERT INTO mknsangarea.client (Client_ID, Client_Name, Client_Contact_Person, Client_Phone, Client_Home_Number," +
                        "Client_Office_Number, Region_ID, Locality_ID, City_ID, Street_ID) VALUES(" + clientToAdd.ClientID + ", '" + clientToAdd.Name + "', '" + clientToAdd.ContactName +
                        "', '" + clientToAdd.Phone + "', '" + clientToAdd.NumberHome + "', '" + clientToAdd.NumberOffice + "', '" +
                        clientToAdd.Region_.RegionID + "', '" + clientToAdd.Locality_.LocalityID + "', '" + clientToAdd.City_.CityID + "', '" + clientToAdd.Street_.StreetID + "');";
                }else
                {
                    query = "INSERT INTO mknsangarea.client (Client_Name, Client_Contact_Person, Client_Phone, Client_Home_Number," +
                        "Client_Office_Number, Region_ID, Locality_ID, City_ID, Street_ID) VALUES('" + clientToAdd.Name + "', '" + clientToAdd.ContactName +
                        "', '" + clientToAdd.Phone + "', '" + clientToAdd.NumberHome + "', '" + clientToAdd.NumberOffice + "', '" +
                        clientToAdd.Region_.RegionID + "', '" + clientToAdd.Locality_.LocalityID + "', '" + clientToAdd.City_.CityID + "', '" + clientToAdd.Street_.StreetID + "');";
                }
            }
            
            bool isAdded = false;
            foreach (Client client in Clients.ToList())
            {
                if(clientToAdd.ClientID == client.ClientID || clientToAdd.ContactName == client.ContactName || 
                   clientToAdd.Phone == client.Phone || clientToAdd.Name == client.Name)
                {
                    isAdded = true;
                    break;
                }
            }

            if (!isAdded)
            {
                if(DbConnection.OpenConnection() == true)
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
        public bool Delete(Client clientToDelete)
        {
            string query = null;
            if (clientToDelete == null || clientToDelete.ClientID == 0)
            {
                MessageBox.Show("Заполни поле ID (Удаление идет только по ID)");
                return false;
            }

            bool isExisted = false;
            foreach (Client client in Clients.ToList())
            {
                if (clientToDelete.ClientID == client.ClientID)
                {
                    isExisted = true;
                    break;
                }
            }

            if(isExisted)
            {
                query = "DELETE FROM mknsangarea.client WHERE Client_ID =" + clientToDelete.ClientID + ";";
                
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
            }else
            {
                MessageBox.Show("Не существует такого клиента");
                isExisted = false;
            }

            return isExisted;
        }
        public bool Update(Client clientToUpdate)
        {
            string query = null;
            if(clientToUpdate == null)
            {
                MessageBox.Show("Заполни поля");
                return false;
            }
            
            bool isUpdated = false;
            foreach (Client client in Clients.ToList())
            {
                if (clientToUpdate.ClientID == client.ClientID || clientToUpdate.Name == client.Name || 
                    clientToUpdate.ContactName == client.ContactName)
                {
                    isUpdated = true;
                    break;
                }
            }

            if(isUpdated)
            {
                if(clientToUpdate.ClientID != 0)
                {
                    query = "UPDATE mknsangarea.client SET Client_Name = '" + clientToUpdate.Name + "', Client_Contact_Person = '" +
                        clientToUpdate.ContactName + "', Client_Phone = '" + clientToUpdate.Phone + "', Client_Home_Number = '" +
                        clientToUpdate.NumberHome + "', Client_Office_Number = '" + clientToUpdate.NumberOffice + "', Region_ID = '" +
                        clientToUpdate.Region_.RegionID + "', Locality_ID = '" + clientToUpdate.Locality_.LocalityID + "', City_ID = '" + clientToUpdate.City_.CityID +
                        "', Street_ID = '" + clientToUpdate.Street_.StreetID + "' WHERE Client_ID = " + clientToUpdate.ClientID + ";";
                }else if(!String.IsNullOrEmpty(clientToUpdate.Name))
                {
                    query = "UPDATE mknsangarea.client SET Client_Name = '" + clientToUpdate.Name + "', Client_Contact_Person = '" +
                        clientToUpdate.ContactName + "', Client_Phone = '" + clientToUpdate.Phone + "', Client_Home_Number = '" +
                        clientToUpdate.NumberHome + "', Client_Office_Number = '" + clientToUpdate.NumberOffice + "', Region_ID = '" +
                        clientToUpdate.Region_.RegionID + "', Locality_ID = '" + clientToUpdate.Locality_.LocalityID + "', City_ID = '" +
                        clientToUpdate.City_.CityID + "', Street_ID = '" + clientToUpdate.Street_.StreetID + "' WHERE Client_Name = '" +
                        clientToUpdate.Name + "';";
                }else if(!String.IsNullOrEmpty(clientToUpdate.ContactName))
                {
                    query = "UPDATE mknsangarea.client SET Client_Name = '" + clientToUpdate.Name + "', Client_Contact_Person = '" +
                        clientToUpdate.ContactName + "', Client_Phone = '" + clientToUpdate.Phone + "', Client_Home_Number = '" +
                        clientToUpdate.NumberHome + "', Client_Office_Number = '" + clientToUpdate.NumberOffice + "', Region_ID = '" +
                        clientToUpdate.Region_.RegionID + "', Locality_ID = '" + clientToUpdate.Locality_.LocalityID + "', City_ID = '" +
                        clientToUpdate.City_.CityID + "', Street_ID = '" + clientToUpdate.Street_.StreetID + "' WHERE Client_Contact_Person = '" +
                        clientToUpdate.ContactName + "';";
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
            }else
            {
                MessageBox.Show("Не существует такого клиента");
            }
            return isUpdated;
        }
        public Client Search(Client clientToSearch)
        {

            Client client = null;
            if(clientToSearch == null)
            {
                MessageBox.Show("Заполни либо ID, Название, Контакное лицо или Телефон");
                return client;
            }

            if (clientToSearch.ClientID == 0 && String.IsNullOrEmpty(clientToSearch.Name) &&
               String.IsNullOrEmpty(clientToSearch.ContactName) && String.IsNullOrEmpty(clientToSearch.Phone))
            {
                MessageBox.Show("Заполни либо ID, Название, Контакное лицо или Телефон");
                return client;
            }

            foreach (Client client1 in Clients.ToList())
            {
                if (client1.ClientID == clientToSearch.ClientID || client1.Name == clientToSearch.Name || client1.ContactName == clientToSearch.ContactName)
                {
                    client = client1;
                    break;
                }
            }

            if(client == null)
            {
                MessageBox.Show("Клиент не найден");
            }

            return client;
        }
    }
}
