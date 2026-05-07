using Microsoft.AspNetCore.Mvc;
using Tet.Repository;
using Tet.Service.Product;
using Tet.Service.Seller;

namespace TET.API.Controllers;
[ApiController]
[Route("[controller]")]
public class ProductController: ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IService _serviceProduct;

    public ProductController(AppDbContext dbContext, IService serviceProduct)
    {
        _dbContext = dbContext;
        _serviceProduct = serviceProduct;
    }

    
    [HttpPost("")]
    public async Task<IActionResult> CreateProduct([FromBody] Request.CreateProductRequest request)
    {
        var result = await _serviceProduct.CreateProduct(request);
        return Ok(result);
    }
}