using Entities;
using Npgsql;
using Repository.@abstract;
using System;
using System.Collections.Generic;

namespace Repository.concrete
{
    public class EmployeeRepository : IRepository<Employee>
    {
        private readonly PostgreSQLConnection _connection;

        public EmployeeRepository(PostgreSQLConnection connection)
        {
            _connection = connection;
        }



        public void Add(Employee employee)
        {
            var query = "INSERT INTO employees (name, position, salary) VALUES (@name, @position, @salary)";
            _connection.ExecuteCommand(command =>
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@name", employee.Name);
                command.Parameters.AddWithValue("@position", employee.Position);
                command.Parameters.AddWithValue("@salary", employee.Salary);
                command.ExecuteNonQuery();
            });
        }

        public void Remove(int id)
        {
            var query = "DELETE FROM employees WHERE id = @id";
            _connection.ExecuteCommand(command =>
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            });
        }

        public Employee GetById(int id)
        {
            var query = "SELECT id, name, position, salary FROM employees WHERE id = @id";
            Employee employee = null;
            _connection.ExecuteCommand(command =>
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        employee = new Employee
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Position = reader.GetString(2),
                            Salary = reader.GetDecimal(3)
                        };
                    }
                }
            });
            return employee;
        }

        public IEnumerable<Employee> GetAll()
        {
            var employees = new List<Employee>();
            var query = "SELECT id, name, position, salary FROM employees";
            _connection.ExecuteCommand(command =>
            {
                command.CommandText = query;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Position = reader.GetString(2),
                            Salary = reader.GetDecimal(3)
                        });
                    }
                }
            });
            return employees;
        }
    }
}
