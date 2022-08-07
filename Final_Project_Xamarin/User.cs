using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Final_Project_Xamarin
{
    class User
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
