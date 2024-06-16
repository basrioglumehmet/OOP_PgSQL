using Npgsql;
using Repository.@abstract;
using System;

namespace Repository.concrete
{
    public class PostgreSQLConnection : DatabaseConnection, IDisposable
    {
        private const string ConnectionErrorMessage = "Error while connecting to the database.";
        private const string QueryExecutionErrorMessage = "Error while executing the query.";
        private NpgsqlConnection? connection;

        public PostgreSQLConnection(string connectionString) : base(connectionString) { }

        /// <summary>
        /// Connects to the PostgreSQL database.
        /// </summary>
        public override void Connect()
        {
            connection = new NpgsqlConnection(ConnectionString);
            try
            {
                connection.Open();
                EnsureEmployeeTableExists();
                Console.WriteLine("Connected to the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ConnectionErrorMessage}\n{ex.Message}");
                connection.Dispose();
                connection = null;
            }
        }

        private void EnsureEmployeeTableExists()
        {
            // Employee tablosunun var olup olmadığını kontrol et
            var checkTableQuery = "SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'employees')";
            using (var command = connection.CreateCommand())
            {
                command.CommandText = checkTableQuery;
                bool tableExists = (bool)command.ExecuteScalar();
                if (!tableExists)
                {
                    // Employee tablosunu oluştur
                    var createTableQuery = @"CREATE TABLE employees (
                                            id SERIAL PRIMARY KEY,
                                            name VARCHAR(100) NOT NULL,
                                            position VARCHAR(100) NOT NULL,
                                            salary DECIMAL(10, 2) NOT NULL
                                            )";
                    command.CommandText = createTableQuery;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Employee table created.");
                }
            }
        }

        /// <summary>
        /// Executes the specified SQL query.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        public override void ExecuteQuery(string query)
        {
            if (connection == null)
            {
                Console.WriteLine("Connection is not open.");
                return;
            }

            ExecuteCommand(command =>
            {
                command.CommandText = query;
                command.ExecuteNonQuery();
            });
        }

        /// <summary>
        /// Executes a command using the provided action.
        /// </summary>
        /// <param name="commandAction">Action to execute with the command.</param>
        public void ExecuteCommand(Action<NpgsqlCommand> commandAction)
        {
            if (connection == null)
            {
                Console.WriteLine("Connection is not open.");
                return;
            }

            try
            {
                using (var command = connection.CreateCommand())
                {
                    commandAction(command);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{QueryExecutionErrorMessage}\n{ex.Message}");
            }
        }

        /// <summary>
        /// Closes the connection to the PostgreSQL database.
        /// </summary>
        public void Close()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }


        public NpgsqlConnection GetOpenConnection()
        {
            if (connection == null || connection.State != System.Data.ConnectionState.Open)
            {
                Connect();
            }
            return connection;
        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Dispose();
                connection = null;
                Console.WriteLine("Connection disposed.");
            }
        }
    }
}
