using CoffeeShop.Api.Models;

namespace CoffeeShop.Api.Services;

public interface ICoffeeService
{
    Task<List<Coffee>> GetAllCoffeesAsync(string? category = null);
}
