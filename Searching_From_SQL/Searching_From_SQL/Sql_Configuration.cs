using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Searching_From_SQL
{
    class Sql_Configuration
    {
        string ConnectionString = @"Data Source=DESKTOP-D4E0FB3;Initial Catalog=SQL_Demo;Integrated Security=True";
        SqlConnection con;
        private static Sql_Configuration instance;
        public static Sql_Configuration getInstance()
        {
            if (instance == null)
                instance = new Sql_Configuration();
            return instance;
        }
        private Sql_Configuration()
        {
            con = new SqlConnection(ConnectionString);
            con.Open();
        }
        public SqlConnection getConnection()
        {
            return con;
        }
    }
}
