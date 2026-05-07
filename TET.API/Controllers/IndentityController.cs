using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tet.Service.Identity;

namespace TET.API.Controllers;
[ApiController]
[Route("[controller]")]
public class IndentityController : ControllerBase
{
    private readonly IService _identityService;

    public IndentityController(IService identityService)
    {
        _identityService = identityService;
    }
    

    
    [HttpGet("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var result = await _identityService.Login(email, password);
        return Ok(result);
    }
}
