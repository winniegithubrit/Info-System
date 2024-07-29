using Microsoft.AspNetCore.Mvc;
using InfoSystem.Services;

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

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts()
    {
      _logger.LogInformation("Received request to get products");
      try
      {
        var products = await _productService.GetProductsAsync();
        _logger.LogInformation($"Retrieved {products.Count} products");
        return Ok(products);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error occurred while retrieving products");
        return StatusCode(500, "Internal server error");
      }
    }
  }
}
