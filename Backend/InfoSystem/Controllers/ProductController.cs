using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InfoSystem.Services;
using InfoSystem.Models;

namespace InfoSystem.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
      _productService = productService;
    }

   
    [HttpGet("products")]
    public async Task<IActionResult> GetProducts()
    {
      var products = await _productService.GetProductsAsync();
      return Ok(products);
    }

  }
}
