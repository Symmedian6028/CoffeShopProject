using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace CoffeeShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoffeesController : ControllerBase
{
    private readonly ICoffeeService _coffeeService;

    public CoffeesController(ICoffeeService coffeeService)
    {
        _coffeeService = coffeeService;
    }

   [HttpGet]
public async Task<IActionResult> GetAllCoffees([FromQuery] string? category = null)
{
    var coffees = await _coffeeService.GetAllCoffeesAsync(category);
    return Ok(coffees);
}
    [Authorize]
[HttpGet("test-protected")]
public IActionResult TestProtected()
{
    var userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
    return Ok(new { message = $"Merhaba {userName}, bu korumalı bir endpoint!" });
}
}