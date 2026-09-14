
namespace CoffeeShop.Api.Models;

public class Coffee
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public required string Description { get; set; }
    public required string Category { get; set; }
    public string? ImageUrl { get; set; }
}