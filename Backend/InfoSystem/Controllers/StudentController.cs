using Microsoft.AspNetCore.Mvc;
using InfoSystem.Services;
using InfoSystem.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace InfoSystem.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class StudentController : ControllerBase
  {
    private readonly StudentService _studentService;
    private readonly ILogger<StudentController> _logger;

    public StudentController(StudentService studentService, ILogger<StudentController> logger)
    {
      _studentService = studentService;
      _logger = logger;
    }

    // Endpoint to get all students
    [HttpGet("students")]
    public async Task<IActionResult> GetStudents()
    {
      _logger.LogInformation("Received request to retrieve all students");

      try
      {
        var students = await _studentService.GetStudentsAsync();
        return Ok(students);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error occurred while retrieving students");
        return StatusCode(500, "Internal server error");
      }
    }
  }
}
