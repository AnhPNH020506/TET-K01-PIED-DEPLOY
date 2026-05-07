using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tet.Service.Cart;
using Tet.Service.Models;

namespace TET.API.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class CartController: ControllerBase
{
    private readonly IService _cartService;

    public CartController(IService cartService)
    {
        _cartService = cartService;
    }
    [HttpPost("")]
    public async Task<IActionResult> CreateCart()//bỏ vào đây ta được là sau dấu chấm hỏi
    {
        await _cartService.CreateCart();
        return Ok(ApiResponseFactory.SuccessResponse(null, "Cart created", HttpContext.TraceIdentifier));
    }

    [HttpGet("")]
    public async Task<IActionResult> GetCart() //bỏ vào đây ta được là sau dấu chấm hỏi
    {
        var result = await _cartService.GetCart();
        return Ok(ApiResponseFactory.SuccessResponse(result, "Cart Response", HttpContext.TraceIdentifier));
    }
    





    [HttpPost("product")]
    public async Task<IActionResult> AddProductToCart([FromBody] Request.AddProductToCartRequest request)
    {
        await _cartService.AddProductToCart(request);
        return Ok(ApiResponseFactory.SuccessResponse("Sucessfully", "Product added", HttpContext.TraceIdentifier));
    }
    
    [HttpDelete(template: "product")]
    public async Task<IActionResult> DeleteProductFromCart([FromBody]Request.RemoveProductFromCartRequest request)
    {
        await _cartService.RemoveProductFromCart(request);
        return Ok(ApiResponseFactory.SuccessResponse("Successfully", "Product removed", HttpContext.TraceIdentifier));
    }
    
}