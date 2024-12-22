using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cainta_MIS_Ticket
{
    public class DatabaseConnection
    {
        private readonly string connectionString;

        public DatabaseConnection()
        {
            //connectionString = @"datasource=127.0.0.1;port=3306;SslMode=none;username=master;password=master;database=db-clcr-document";

            //connectionString = @"datasource=127.0.0.1;port=3307;SslMode=none;username=user;password=user;database=db-clcr-document";

            connectionString = @"server=localhost;port=3306;userid=root;password=;database=ticketing_system";
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
