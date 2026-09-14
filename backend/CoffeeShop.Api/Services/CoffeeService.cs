using CoffeeShop.Api.Data;
using CoffeeShop.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Api.Services;

public class CoffeeService : ICoffeeService
{
    private readonly AppDbContext _context;

    public CoffeeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Coffee>> GetAllCoffeesAsync(string? category = null)
{
    var query = _context.Coffees.AsQueryable();

    if (!string.IsNullOrEmpty(category))
    {
        query = query.Where(c => c.Category == category);
    }

    return await query.ToListAsync();
}
}