using Microsoft.AspNetCore.Mvc;

namespace MigrationService.Controllers;

[ApiController]
[Route("[controller]")]
public class MigrationServiceController : ControllerBase
{
    public MigrationServiceController()
    {
        
    }
    [HttpGet]
    public string Get()
    {
        return "Hello from MigrationService!";
    }
}