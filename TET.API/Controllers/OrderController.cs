using Microsoft.AspNetCore.Mvc;
using Tet.Service.Models;
using Tet.Service.Order;

namespace TET.API.Controllers;
[ApiController]
[Route("[controller]")]
public class OrderController: ControllerBase
{
    private readonly IService _orderService;

    public OrderController(IService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateOrder(Request.CreateOrderRequest request) //bỏ vào đây ta được là sau dấu chấm hỏi
    {
        var result = await _orderService.CreateOrder(request);
        return Ok(ApiResponseFactory.SuccessResponse(result, "Order created", HttpContext.TraceIdentifier ));
    }
    [HttpPost("sepay/webhook")]
    public async Task<IActionResult> SepayWebHook(Request.SepayWebhookRequest request) //bỏ vào đây ta được là sau dấu chấm hỏi
    {
        await _orderService.SepayWebHokkHandler(request);
        return Ok(ApiResponseFactory.SuccessResponse("", "WebHook response", HttpContext.TraceIdentifier ));
    }
}