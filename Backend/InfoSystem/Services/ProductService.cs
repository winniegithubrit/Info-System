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
    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
      var products = new List<Product>();

      // Create a new MySQL connection
      using (var connection = new MySqlConnection(_connectionString))
      {
        // Create a new MySQL command with the stored procedure
        using (var command = new MySqlCommand("GetProducts", connection))
        {
          // Indicate that the command is a stored procedure
          command.CommandType = CommandType.StoredProcedure;

          // Open the database connection asynchronously
          await connection.OpenAsync();

          // Execute the stored procedure and read the results
          using (var reader = await command.ExecuteReaderAsync())
          {
            while (await reader.ReadAsync())
            {
              var product = new Product
              {
                Id = reader.GetInt32("Id"),
                ProductName = reader.GetString("ProductName"),
                Category = reader.GetString("Category"),
                Price = reader.GetDecimal("Price"),
                StockQuantity = reader.GetInt32("StockQuantity"),
                Supplier = reader.GetString("Supplier"),
                Description = reader.GetString("Description")
              };
              products.Add(product);
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
      // creating a new mysql connection
      using (var connection = new MySqlConnection(_connectionString))
      {
        // new mysql command with the stored procedure
        using (var command = new MySqlCommand("AddProduct", connection))
        {
          // stating that the command is a stored procedure
          command.CommandType = CommandType.StoredProcedure;
          // Add parameters to the command for each property of the Product object dbnull is for null values
          command.Parameters.AddWithValue("@p_ProductName", product.ProductName ?? (object)DBNull.Value);
          command.Parameters.AddWithValue("@p_Category", product.Category ?? (object)DBNull.Value);
          command.Parameters.AddWithValue("@p_Price", product.Price);
          command.Parameters.AddWithValue("@p_StockQuantity", product.StockQuantity);
          command.Parameters.AddWithValue("@p_Supplier", product.Supplier ?? (object)DBNull.Value);
          command.Parameters.AddWithValue("@p_Description", product.Description ?? (object)DBNull.Value);
// opening the database connection asynchronously
          await connection.OpenAsync();
        // executing the stored procedure asynchronously
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
     // Adding a parameter for the product ID to the command

          command.Parameters.AddWithValue("@p_Id", id);

          await connection.OpenAsync();
          // Executing the stored procedure asynchronously and store the number of affected rows
          var result = await command.ExecuteNonQueryAsync();

          // If the number of affected rows is greater than 0, deletion was successful
          return result > 0;
        }
      }
    }
    // PUT FUNCTIONALITY
    public async Task<Product?> UpdateProductAsync(int id, Product product)
    {
      using (var connection = new MySqlConnection(_connectionString))
      {
        using (var command = new MySqlCommand("UpdateProduct", connection))
        {
          command.CommandType = CommandType.StoredProcedure;
          command.Parameters.AddWithValue("@p_Id", id);
          command.Parameters.AddWithValue("@p_ProductName", product.ProductName ?? (object)DBNull.Value);
          command.Parameters.AddWithValue("@p_Category", product.Category ?? (object)DBNull.Value);
          command.Parameters.AddWithValue("@p_Price", product.Price);
          command.Parameters.AddWithValue("@p_StockQuantity", product.StockQuantity);
          command.Parameters.AddWithValue("@p_Supplier", product.Supplier ?? (object)DBNull.Value);
          command.Parameters.AddWithValue("@p_Description", product.Description ?? (object)DBNull.Value);

          await connection.OpenAsync();
          var affectedRows = await command.ExecuteNonQueryAsync();

          // If the number of affected rows is greater than 0, update was successful
          if (affectedRows > 0)
          {
            // Return updated product details
            return await GetProductByIdAsync(id); 
          }
          else
          {
            return null; 
          }
        }
      }
    }
    // service to delete the most recent id
    public async Task<int?> GetMostRecentProductIdAsync()
    {
      int? mostRecentProductId = null;

      using (var connection = new MySqlConnection(_connectionString))
      {
        using (var command = new MySqlCommand("GetMostRecentProductId", connection))
        {
          command.CommandType = CommandType.StoredProcedure;

          await connection.OpenAsync();
          var result = await command.ExecuteScalarAsync();
          mostRecentProductId = result != null ? Convert.ToInt32(result) : (int?)null;
        }
      }

      return mostRecentProductId;
    }
  }
}
