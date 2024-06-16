using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.@abstract
{
    public abstract class DatabaseConnection
    {
        protected string ConnectionString { get; set; }

        public DatabaseConnection() { }

        public DatabaseConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public abstract void Connect();
        public abstract void ExecuteQuery(string query);
    }
}
