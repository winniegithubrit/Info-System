using Microsoft.AspNetCore.Mvc;
using InfoSystem.Services;
using InfoSystem.Models;

namespace InfoSystem.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly ProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(ProductService productService, ILogger<ProductController> logger)
    {
      _productService = productService;
      _logger = logger;
    }
    // end point to get all products
    [HttpGet("products")]
    public async Task<IActionResult> GetProducts()
    {
      _logger.LogInformation("Received request to retrieve all products");

      try
      {
        var products = await _productService.GetProductsAsync();
        return Ok(products);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error occurred while retrieving products");
        return StatusCode(500, "Internal server error");
      }
    }

    // get product by id
    [HttpGet("products/{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
      _logger.LogInformation($"Received request to get product with ID {id}");
      try
      {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
          _logger.LogWarning($"Product with ID {id} not found");
          return NotFound();
        }
        _logger.LogInformation($"Retrieved product with ID {id}");
        return Ok(product);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Error occurred while retrieving product with ID {id}");
        return StatusCode(500, "Internal server error");
      }
    }
    // add a product 
    [HttpPost("products")]
    public async Task<IActionResult> AddProduct([FromBody] Product product)
    {
      if (product == null)
      {
        _logger.LogWarning("Received a null product object");
        return BadRequest("Product object is null");
      }

      _logger.LogInformation("Received request to add a new product");
      try
      {
        await _productService.AddProductAsync(product);
        _logger.LogInformation("Product added successfully");
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error occurred while adding the product");
        return StatusCode(500, "Internal server error");
      }
    }
  // Delete a product
    [HttpDelete("products/{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
      try
      {
        var isDeleted = await _productService.DeleteProductAsync(id);
        if (isDeleted)
        {
          return Ok(new { message = "Product deleted successfully" }); 
        }
        else
        {
          return NotFound(new { message = "Product not found" }); 
        }
      }
      catch (Exception)
      {
        return StatusCode(500, new { message = "Internal server error" }); 
      }
    }
    // PUT FUNCTIONALITY
    [HttpPut("products/{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
      if (product == null || id != product.Id)
      {
        return BadRequest("Product object is null or ID mismatch");
      }

      try
      {
        var updatedProduct = await _productService.UpdateProductAsync(id, product);
        if (updatedProduct != null)
        {
          return Ok(updatedProduct);
        }
        else
        {
          return NotFound(new { message = "Product not found" });
        }
      }
      catch (Exception)
      {
        return StatusCode(500, "Internal server error");
      }
    }

    // deleting the most recent product
    [HttpDelete("mostRecentProduct")]
    public async Task<IActionResult> DeleteMostRecentProduct()
    {
      _logger.LogInformation("Received request to delete the most recent product");

      try
      {
        var mostRecentProductId = await _productService.GetMostRecentProductIdAsync();
        if (mostRecentProductId == null)
        {
          _logger.LogWarning("No products found to delete");
          return NotFound(new { message = "No products found" });
        }

        var isDeleted = await _productService.DeleteProductAsync(mostRecentProductId.Value);
        if (isDeleted)
        {
          _logger.LogInformation($"Product with ID {mostRecentProductId} deleted successfully");
          return Ok(new { message = "Product deleted successfully" });
        }
        else
        {
          _logger.LogWarning($"Failed to delete product with ID {mostRecentProductId}");
          return StatusCode(500, new { message = "Failed to delete the product" });
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error occurred while deleting the most recent product");
        return StatusCode(500, "Internal server error");
      }
    }
  }
}