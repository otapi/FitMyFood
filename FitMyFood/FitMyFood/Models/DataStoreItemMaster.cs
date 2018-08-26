using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FitMyFood.Models
{
    public class DataStoreItemMaster
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
