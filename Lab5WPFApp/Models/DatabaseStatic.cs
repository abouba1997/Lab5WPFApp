using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5WPFApp.Models
{
    public static class DatabaseStatic
    {
        private static string username;
        public static string Username
        {
            get { return username; }
            set { username = value; }
        }

        private static string password;
        public static string Password
        {
            get { return password; }
            set { password = value; }
        }
    }
}
