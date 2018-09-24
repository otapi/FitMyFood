using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

/*
 * https://www.dsibinski.pl/2017/05/sqlite-net-extensions-one-to-one-relationships/
 * https://www.dsibinski.pl/2017/05/sqlite-net-extensions-one-to-many-relationships/
 * https://www.dsibinski.pl/2017/05/sqlite-net-extensions-many-to-many-relationships/
 */
namespace FitMyFood.Models
{
    public class BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
