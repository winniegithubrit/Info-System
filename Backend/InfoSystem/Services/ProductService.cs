using MySql.Data.MySqlClient;
using System.Data;
using InfoSystem.Models;

namespace InfoSystem.Services
{
  public class ProductService
  {
    private readonly string _connectionString;
// constructor initializing connection string for configuration
    public ProductService(IConfiguration configuration)
    {
      _connectionString = configuration.GetConnectionString("DefaultConnection")
                          ?? throw new ArgumentNullException(nameof(configuration));
    }
    // Method to retrieve all products from the database
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
            // Reading  data from the database and add to the products list
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

    // Get product by ID functionality
    public async Task<Product?> GetProductByIdAsync(int id)
    {
      Product? product = null;

      using (var connection = new MySqlConnection(_connectionString))
      {
        using (var command = new MySqlCommand("GetProductById", connection))
        {
          command.CommandType = CommandType.StoredProcedure;
          command.Parameters.AddWithValue("@p_Id", id);

          await connection.OpenAsync();
          using (var reader = await command.ExecuteReaderAsync())
          {
            if (await reader.ReadAsync())
            {
              product = new Product
              {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                Category = reader.GetString(reader.GetOrdinal("Category")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                Supplier = reader.GetString(reader.GetOrdinal("Supplier")),
                Description = reader.GetString(reader.GetOrdinal("Description"))
              };
            }
          }
        }
      }

      return product;
    }

    // Post product functionality
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
    // delete functionality
    public async Task<bool> DeleteProductAsync(int id)
    {
      using (var connection = new MySqlConnection(_connectionString))
      {
        using (var command = new MySqlCommand("DeleteProduct", connection))
        {
          command.CommandType = CommandType.StoredProcedure;
          command.Parameters.AddWithValue("@p_Id", id);

          await connection.OpenAsync();
          var result = await command.ExecuteNonQueryAsync();

          // If the number of affected rows is greater than 0, deletion was successful
          return result > 0;
        }
      }
    }

  }
}
