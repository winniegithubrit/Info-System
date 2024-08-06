using MySql.Data.MySqlClient;
using InfoSystem.Models;
using Bogus;
using System.Data;
using System.Threading.Tasks;

namespace InfoSystem.Data
{
  public class Seeder
  {
    private readonly string _connectionString;

    public Seeder(IConfiguration configuration)
    {
      _connectionString = configuration.GetConnectionString("DefaultConnection")
                          ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task SeedAsync()
    {
      var faker = new Faker<Student>()
          .RuleFor(s => s.FirstName, f => f.Name.FirstName())
          .RuleFor(s => s.LastName, f => f.Name.LastName())
          .RuleFor(s => s.Age, f => f.Random.Int(6, 18))
          .RuleFor(s => s.Grade, f => f.Random.AlphaNumeric(2).ToUpper());

      var students = faker.Generate(10);

      using (var connection = new MySqlConnection(_connectionString))
      {
        await connection.OpenAsync();

        // foreach (var student in students)
        // {
        //   using (var command = new MySqlCommand("AddStudent", connection))
        //   {
        //     command.CommandType = CommandType.StoredProcedure;
        //     command.Parameters.AddWithValue("@p_FirstName", student.FirstName ?? (object)DBNull.Value);
        //     command.Parameters.AddWithValue("@p_LastName", student.LastName ?? (object)DBNull.Value);
        //     command.Parameters.AddWithValue("@p_Age", student.Age);
        //     command.Parameters.AddWithValue("@p_Grade", student.Grade ?? (object)DBNull.Value);

        //     await command.ExecuteNonQueryAsync();
        //   }
        // }
      }
    }
  }
}
