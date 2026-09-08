namespace CoffeeShop.Api.Dtos;

public class OrderItemResponseDto
{
    public required string CoffeeName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}