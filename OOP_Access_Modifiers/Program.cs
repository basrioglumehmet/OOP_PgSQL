using Entities;
using Repository.@abstract;
using Repository.concrete;
using System;

namespace OOP_Access_Modifiers
{
    class Program
    {
        public static void Main(string[] args)
        {

            /*
             *  1. Public : Her yerden erişilebilir.

                2. Private : Sadece tanımlandığı sınıf içerisinden erişilebilir.

                3. Internal : Sadece bulunduğu proje içerisinden erişilebilir

                4. Protected : Sadece tanımlandığı sınıfta ya da o sınıfı miras alan sınıflardan erişilebilir.
            */

            string connectionString = "Host=localhost;Username=postgres;Password=yourpassword;Database=mydatabase";

            using (var connection = new PostgreSQLConnection(connectionString))
            {
                connection.Connect(); // Bağlantıyı açmak için Connect() kullanılıyor

                var employeeRepository = new EmployeeRepository(connection);

                // Yeni çalışan ekle
                var newEmployee = new Employee
                {
                    Name = "Mehmet Basrioğlu",
                    Position = "Manager",
                    Salary = 75000
                };

                employeeRepository.Add(newEmployee);

                // Çalışanı ID'ye göre getir
                var employee = employeeRepository.GetById(1);
                if (employee != null)
                {
                    Console.WriteLine($"ID: {employee.Id}, Name: {employee.Name}, Position: {employee.Position}, Salary: {employee.Salary}");
                }

                // Tüm çalışanları getir
                var employees = employeeRepository.GetAll();
                foreach (var emp in employees)
                {
                    Console.WriteLine($"ID: {emp.Id}, Name: {emp.Name}, Position: {emp.Position}, Salary: {emp.Salary}");
                }

                connection.Close();
            }

            Console.ReadLine();
        }
    }
}
