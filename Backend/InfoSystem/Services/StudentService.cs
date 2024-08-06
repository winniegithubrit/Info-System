using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using InfoSystem.Models;
using Microsoft.Extensions.Configuration;

namespace InfoSystem.Services
{
  public class StudentService
  {
    private readonly string _connectionString;

    public StudentService(IConfiguration configuration)
    {
      _connectionString = configuration.GetConnectionString("DefaultConnection")
                          ?? throw new ArgumentNullException(nameof(configuration));
    }

    // Method to retrieve all students from the database
    public async Task<IEnumerable<Student>> GetStudentsAsync()
    {
      var students = new List<Student>();

      using (var connection = new MySqlConnection(_connectionString))
      {
        using (var command = new MySqlCommand("GetStudents", connection))
        {
          command.CommandType = CommandType.StoredProcedure;

          await connection.OpenAsync();

          using (var reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              var student = new Student
              {
                Id = reader.GetInt32("Id"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Age = reader.GetInt32("Age"),
                Grade = reader.GetString("Grade")
              };
              students.Add(student);
            }
          }
        }
      }

      return students;
    }
  }
}
