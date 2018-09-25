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
    public interface IBase
    {
        int Id { get; set; }
        string Name { get; set; }
    }
}
