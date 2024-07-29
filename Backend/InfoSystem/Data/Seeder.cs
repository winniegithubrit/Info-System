using MySql.Data.MySqlClient;
using InfoSystem.Models;
using Bogus;
using System.Data;


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
      var faker = new Faker<Product>()
          .RuleFor(p => p.ProductName, f => f.Commerce.ProductName())
          .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
          .RuleFor(p => p.Price, f => f.Finance.Amount())
          .RuleFor(p => p.StockQuantity, f => f.Random.Int(1, 1000))
          .RuleFor(p => p.Supplier, f => f.Company.CompanyName())
          .RuleFor(p => p.Description, f => f.Commerce.ProductDescription());

      var products = faker.Generate(10); // Generate 10 fake products

      using (var connection = new MySqlConnection(_connectionString))
      {
        await connection.OpenAsync();

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
