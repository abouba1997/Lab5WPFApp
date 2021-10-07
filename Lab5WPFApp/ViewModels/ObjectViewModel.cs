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
    public class ObjectViewModel : BaseViewModel
    {
        private ObservableCollection<Models.Object> objects;
        public ObservableCollection<Models.Object> Objects
        {
            get { return objects; }
            set { objects = value; }
        }

        private ObservableCollection<Client> clients;
        public ObservableCollection<Client> Clients
        {
            get { return clients; }
            set { clients = value; RaisePropertyChanged(nameof(Clients)); }
        }

        private ObservableCollection<Contractor> contractors;
        public ObservableCollection<Contractor> Contractors
        {
            get { return contractors; }
            set { contractors = value; RaisePropertyChanged(nameof(Contractors)); }
        }

        private DatabaseConnect dbConnection;
        public DatabaseConnect DbConnection
        {
            get { return dbConnection; }
            set { dbConnection = value; RaisePropertyChanged("DbConnection"); }
        }

        public ObjectViewModel()
        {
            DbConnection = new DatabaseConnect();
            DbConnection.Username = DatabaseStatic.Username;
            DbConnection.Password = DatabaseStatic.Password;
            Objects = new ObservableCollection<Models.Object>();
            Clients = new ObservableCollection<Client>();
            Contractors = new ObservableCollection<Contractor>();
            Select();
        }

        private void Select()
        {
            string query = "Select Object_ID, Object_Name, Object_Cost, Object_Date_Begin, Object_Date_End, " +
                "client.Client_ID, Client_Name, contractor.Contractor_ID, Contractor_Name from mknsangarea.object " +
                "join client on object.Client_ID = client.Client_ID " +
                "join contractor on object.Contractor_ID = contractor.Contractor_ID order by Object_ID asc; ";

            if(DbConnection.OpenConnection() == true)
            {
                try
                {
                    MySqlCommand command = new MySqlCommand(query, DbConnection.Connection);
                    MySqlDataReader dataReader = command.ExecuteReader();

                    Models.Object _object = null;
                    Client client = null;
                    Contractor contractor = null;
                    

                    while (dataReader.Read())
                    {
                        _object = new Models.Object();
                        client = new Client();
                        contractor = new Contractor();
                        

                        _object.ObjectID = dataReader.GetInt32(0);
                        _object.Name = dataReader.GetString(1);
                        _object.Cost = dataReader.GetString(2);
                        _object.DateBegin = dataReader.GetDateTime(3);
                        _object.DateEnd = dataReader.GetDateTime(4);

                        client.ClientID = dataReader.GetInt32(5);
                        client.Name = dataReader.GetString(6);

                        contractor.ContractorID = dataReader.GetInt32(7);
                        contractor.Name = dataReader.GetString(8);

                        _object.Client = client;
                        _object.Contractor = contractor;

                        Objects.Insert(0, _object);
                        Clients.Add(client);
                        Contractors.Add(contractor);
                    }
                    dataReader.Close();
                    DbConnection.CloseConnection();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public bool Add(Models.Object objectToAdd)
        {
            string query = null;
            if (objectToAdd == null)
            {
                MessageBox.Show("Все поля должны быть заполнены");
                return false;
            }
            else if (String.IsNullOrEmpty(objectToAdd.Name) || String.IsNullOrEmpty(objectToAdd.Cost) ||
               objectToAdd.DateBegin == null || objectToAdd.DateEnd == null && objectToAdd.ObjectID == 0 ||
               string.IsNullOrEmpty(objectToAdd.Client.Name) || string.IsNullOrEmpty(objectToAdd.Contractor.Name))
            {
                MessageBox.Show("Все поля должны быть заполнены");
                return false;
            } 

            bool isAdded = false;
            foreach (Models.Object _object in Objects.ToList())
            {
                if (objectToAdd.ObjectID == _object.ObjectID || (objectToAdd.Client.Name == _object.Client.Name && 
                    objectToAdd.Contractor.Name == _object.Contractor.Name))
                {
                    isAdded = true;
                    break;
                }
            }

            if (isAdded)
            {
                if (objectToAdd.ObjectID != 0)
                {
                    query = "INSERT INTO mknsangarea.object (Object_ID, Object_Name, Object_Cost, Object_Date_Begin, Object_Date_End," +
                        "Client_ID, Contractor_ID) VALUES(" + objectToAdd.ObjectID + ", '" + objectToAdd.Name + "', '" + objectToAdd.Cost +
                        "', '" + objectToAdd.DateBegin.ToString("yyyy-MM-dd") + "', '" + objectToAdd.DateEnd.ToString("yyyy-MM-dd") + "', " + objectToAdd.Client.ClientID + ", " +
                        objectToAdd.Contractor.ContractorID + ");";
                }
                else
                {
                    query = "INSERT INTO mknsangarea.object (Object_Name, Object_Cost, Object_Date_Begin, Object_Date_End," +
                        "Client_ID, Contractor_ID) VALUES('" + objectToAdd.Name + "', '" + objectToAdd.Cost +
                        "', '" + objectToAdd.DateBegin.ToString("yyyy-MM-dd") + "', '" + objectToAdd.DateEnd.ToString("yyyy-MM-dd") + "', '" + objectToAdd.Client.ClientID + "', '" +
                        objectToAdd.Contractor.ContractorID + "');";
                }

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
            }else
            {
                MessageBox.Show("Объект уже существует");
            }
            return isAdded;
        }
        public bool Delete(Models.Object objectToDelete)
        {
            string query = null;
            if (objectToDelete == null || objectToDelete.ObjectID == 0)
            {
                MessageBox.Show("Заполни поле ID (Удаление идет только по ID)");
                return false;
            }

            bool isExisted = false;
            foreach (Models.Object _object in Objects.ToList())
            {
                if (objectToDelete.ObjectID == _object.ObjectID)
                {
                    isExisted = true;
                    break;
                }
            }

            if (isExisted)
            {
                query = "DELETE FROM mknsangarea.object WHERE Object_ID =" + objectToDelete.ObjectID + ";";

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
        public bool Update(Models.Object objectToUpdate)
        {
            string query = null;
            if (objectToUpdate == null)
            {
                MessageBox.Show("Заполни поля");
                return false;
            }
            else if (String.IsNullOrEmpty(objectToUpdate.Name) || String.IsNullOrEmpty(objectToUpdate.Cost) ||
              objectToUpdate.DateBegin == null || objectToUpdate.DateEnd == null && objectToUpdate.ObjectID == 0 ||
              string.IsNullOrEmpty(objectToUpdate.Client.Name) || string.IsNullOrEmpty(objectToUpdate.Contractor.Name))
            {
                MessageBox.Show("Все поля должны быть заполнены");
                return false;
            }

            bool isUpdated = false;
            foreach (Models.Object _object in Objects.ToList())
            {
                if (objectToUpdate.ObjectID == _object.ObjectID || objectToUpdate.Name == _object.Name ||
                    (objectToUpdate.Client.ClientID == _object.Client.ClientID && objectToUpdate.Contractor.ContractorID == _object.Contractor.ContractorID))
                {
                    isUpdated = true;
                    break;
                }
            }

            if (isUpdated)
            {
                if (objectToUpdate.ObjectID != 0)
                {
                    query = "UPDATE mknsangarea.object SET Object_Name = '" + objectToUpdate.Name + "', Object_Cost = '" +
                        objectToUpdate.Cost + "', Object_Date_Begin = '" + objectToUpdate.DateBegin.ToString("yyyy-MM-dd") + "', Object_Date_End = '" +
                        objectToUpdate.DateEnd.ToString("yyyy-MM-dd") + "', Client_ID = " + objectToUpdate.Client.ClientID + 
                        ", Contractor_ID = " + objectToUpdate.Contractor.ContractorID + " WHERE Object_ID = " + objectToUpdate.ObjectID + ";";
                }
                else 
                {
                    MessageBox.Show("Заполни поле ID");
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
        public Models.Object Search(Models.Object objectToSearch)
        {

            Models.Object _object = null;
            if (objectToSearch == null)
            {
                MessageBox.Show("Заполни либо ID, Название, Контакное лицо или Телефон");
                return _object;
            }

            if (objectToSearch.ObjectID == 0)
            {
                MessageBox.Show("Заполни либо ID");
                return _object;
            }

            foreach (Models.Object _object1 in Objects.ToList())
            {
                if (_object1.ObjectID == objectToSearch.ObjectID)
                {
                    _object = _object1;
                    break;
                }
            }

            if (_object == null)
            {
                MessageBox.Show("Подрядчик не найден");
            }

            return _object;
        }
    }
}
