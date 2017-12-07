using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    /// <summary>
    /// DB Connection Class
    /// </summary>
    class DBConnection
    {
        internal static SqlConnection GetConnection()
        {
            var connString = @"Data Source=DESKTOP-O5U6AVG\SQLEXPRESS;Initial Catalog=ksmanDB;Integrated Security=True";
            var conn = new SqlConnection(connString);
            return conn;
        }
    }
}
