using MySql.Data.MySqlClient;
using InfoSystem.Models;
// library for generating fake data
using Bogus;
using System.Data;


namespace InfoSystem.Data
{
  public class Seeder
  {
    private readonly string _connectionString;

// Constructor initializes the connection string from the configuration
    public Seeder(IConfiguration configuration)
    {
      _connectionString = configuration.GetConnectionString("DefaultConnection")
                          ?? throw new ArgumentNullException(nameof(configuration));
    }
// method to seed the fake data into the database
    public async Task SeedAsync()
    {
      // Create a Faker instance to generate fake Product data

      var faker = new Faker<Product>()
          .RuleFor(p => p.ProductName, f => f.Commerce.ProductName())
          .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
          .RuleFor(p => p.Price, f => f.Finance.Amount())
          .RuleFor(p => p.StockQuantity, f => f.Random.Int(1, 1000))
          .RuleFor(p => p.Supplier, f => f.Company.CompanyName())
          .RuleFor(p => p.Description, f => f.Commerce.ProductDescription());
// generating 10 fake products
      var products = faker.Generate(10); 
// creating a connection to mysql database
      using (var connection = new MySqlConnection(_connectionString))
      {
        // opening the connection asynchronously
        await connection.OpenAsync();
// inserts the generated data into the database
        foreach (var product in products)
        {
          // using (var command = new MySqlCommand("AddProduct", connection))
          // {
          //   command.CommandType = CommandType.StoredProcedure;
          //   command.Parameters.AddWithValue("@p_ProductName", product.ProductName ?? (object)DBNull.Value);
          //   command.Parameters.AddWithValue("@p_Category", product.Category ?? (object)DBNull.Value);
          //   command.Parameters.AddWithValue("@p_Price", product.Price);
          //   command.Parameters.AddWithValue("@p_StockQuantity", product.StockQuantity);
          //   command.Parameters.AddWithValue("@p_Supplier", product.Supplier ?? (object)DBNull.Value);
          //   command.Parameters.AddWithValue("@p_Description", product.Description ?? (object)DBNull.Value);

          //   await command.ExecuteNonQueryAsync();
          // }
        }
      }
    }
  }
}
