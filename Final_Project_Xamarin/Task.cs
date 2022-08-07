using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Final_Project_Xamarin
{
    class Task
    {
        [PrimaryKey]
        public string TaskId { get; set; }
        public string Description { get; set; }
        public string Assigned { get; set; }

        public string DeadLine { get; set; }

       
    }
}
