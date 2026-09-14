using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CoffeeShop.Api.Services;
using CoffeeShop.Api.Dtos;
using System.Security.Claims;

namespace CoffeeShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderRequestDto request)
    {
        try
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null)
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdString);

            var result = await _orderService.CreateOrderAsync(userId, request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("my-orders")]
    public async Task<IActionResult> GetMyOrders()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdString == null)
        {
            return Unauthorized();
        }
        var userId = int.Parse(userIdString);

        var orders = await _orderService.GetUserOrdersAsync(userId);
        return Ok(orders);
    }
}