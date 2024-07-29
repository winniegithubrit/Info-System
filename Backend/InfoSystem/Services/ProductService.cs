using MySql.Data.MySqlClient;
using System.Data;
using InfoSystem.Models;

namespace InfoSystem.Services
{
  public class ProductService
  {
    private readonly string _connectionString;

    public ProductService(IConfiguration configuration)
    {
      _connectionString = configuration.GetConnectionString("DefaultConnection")
                          ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task AddProductAsync(Product product)
    {
      using (var connection = new MySqlConnection(_connectionString))
      {
        using (var command = new MySqlCommand("AddProduct", connection))
        {
          command.CommandType = CommandType.StoredProcedure;
          command.Parameters.AddWithValue("@p_ProductName", product.ProductName ?? (object)DBNull.Value);
          command.Parameters.AddWithValue("@p_Category", product.Category ?? (object)DBNull.Value);
          command.Parameters.AddWithValue("@p_Price", product.Price);
          command.Parameters.AddWithValue("@p_StockQuantity", product.StockQuantity);
          command.Parameters.AddWithValue("@p_Supplier", product.Supplier ?? (object)DBNull.Value);
          command.Parameters.AddWithValue("@p_Description", product.Description ?? (object)DBNull.Value);

          await connection.OpenAsync();
          await command.ExecuteNonQueryAsync();
        }
      }
    }

    public async Task<List<Product>> GetProductsAsync()
    {
      var products = new List<Product>();

      using (var connection = new MySqlConnection(_connectionString))
      {
        using (var command = new MySqlCommand("GetProducts", connection))
        {
          command.CommandType = CommandType.StoredProcedure;

          await connection.OpenAsync();
          using (var reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              products.Add(new Product
              {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                Category = reader.GetString(reader.GetOrdinal("Category")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                Supplier = reader.GetString(reader.GetOrdinal("Supplier")),
                Description = reader.GetString(reader.GetOrdinal("Description"))
              });
            }
          }
        }
      }

      return products;
    }
  }
}
