using System.Configuration;
using System.Data.SqlClient;

namespace Regis.Helpers
{
    public class DBHelper
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["erpdb"].ConnectionString;

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}