using Lab5WPFApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab5WPFApp.ViewModels
{
    public class CustomViewModel : BaseViewModel
    {
        private IDictionary<string, string> queries;
        public IDictionary<string, string> Queries
        {
            get { return queries; }
            set { queries = value; RaisePropertyChanged(nameof(Queries)); }
        }

        private DatabaseConnect dbConnection;
        public DatabaseConnect DbConnection
        {
            get { return dbConnection; }
            set { dbConnection = value; RaisePropertyChanged(nameof(DbConnection)); }
        }

        public CustomViewModel()
        {
            DbConnection = new DatabaseConnect();
            DbConnection.Username = DatabaseStatic.Username;
            DbConnection.Password = DatabaseStatic.Password;
            Queries = new Dictionary<string, string>()
            {
                {"1 - Список клиентов по заказу объектов c данными справочниками", "select Client_Name as 'Клиент' , Client_Contact_Person as 'Контакное лицо', " +
                "Client_Phone as 'Телефон', Object_Name as 'Объект', Object_Cost as 'Стоймость объекта', Street_Name as 'Улица', " +
                "Client_Home_Number as '№ Дом', Client_Office_Number as '№ Офис', City_Name as 'Город', Locality_Name as 'Населеный пункт', " +
                "Region_Name as 'Регион' from mknsangarea.client " +
                "join object on client.Client_ID = Object.Client_ID join street on client.Street_ID = street.Street_ID " +
                "join city on client.City_ID = city.City_ID join locality on client.Locality_ID = locality.Locality_ID " +
                "join region on client.Region_ID = region.Region_ID"},

                {"2 - Список подрядчиков по производству объектов c данными справочниками", "select Contractor_Name as 'Подрядчик', Contractor_Contact_Person" +
                " as 'Контакное лицо', Contractor_Phone as 'Телефон', Object_Name as 'Объект', Object_Cost as 'Стоймость объекта', Street_Name as 'Улица'," +
                " Contractor_Home_Number as '№ Дом', Contractor_Office_Number as '№ Офис', City_Name as 'Город', Locality_Name as 'Населеный пункт', Region_Name as 'Регион' " +
                "from mknsangarea.contractor join object on contractor.Contractor_ID = Object.Contractor_ID " +
                "join street on contractor.Contractor_Street_ID = street.Street_ID join city on contractor.Contractor_City_ID" +
                " = city.City_ID join locality on contractor.Contractor_Locality_ID = locality.Locality_ID join region " +
                "on contractor.Contractor_Region_ID = region.Region_ID"},

                {"3 - Список объектов по производству подрядчиков и по заказам клиентов", "select Object_Name as 'Объект', Object_Cost as 'Стоймость объекта', Object_Date_Begin " +
                "as 'Дата начала', Object_Date_End as 'Дата конец', Client_Name as 'Клиент', " +
                "Client_Contact_Person as 'Контакное лицо клиента', Contractor_Name as 'Подрядчик', Contractor_Contact_Person as 'Контакное лицо подрядчика' from mknsangarea.object " +
                "join client on object.Client_ID = client.Client_ID join contractor on object.Contractor_ID = " +
                "contractor.Contractor_ID"}
            };
        }

        public DataTable ShowQueryResult(string query)
        {
            DataTable dataTable = new DataTable();
            if(DbConnection.OpenConnection() == true)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, DbConnection.Connection);
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
                    
                    dataAdapter.Fill(dataTable);
                    DbConnection.CloseConnection();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dataTable;
        }
    }
}
