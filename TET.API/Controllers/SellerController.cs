using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TET.API.Extensions;
using Tet.Repository;
using Tet.Repository.Entity;
using Tet.Service.Identity;
using Tet.Service.Seller;

namespace TET.API.Controllers;
//[Authorize(Policy = JwtExtension.AdminPolicy)]
[ApiController]
[Route("[controller]")]
public class SellerController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly ISellerService _sellerService;
    public SellerController(AppDbContext dbContext, ISellerService sellerService)
    {
        _dbContext = dbContext;
        _sellerService = sellerService;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetSellers([FromQuery] string? searchTerm, int pageSize = 10, int pageIndex = 1)
    {
        var result = await _sellerService.GetSellers(searchTerm, pageSize, pageIndex);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSellerById(Guid id)
    {
        var result = await _sellerService.GetSellerById(id);
        return Ok(result);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateSeller([FromBody] SellerRequest.CreateSellerRequest request)
    {
        var result = await _sellerService.CreateSeller(request);
        return Ok(result);
    }


}