namespace CoffeeShop.Api.Dtos;

public class OrderItemRequestDto
{
    public int CoffeeId { get; set; }
    public int Quantity { get; set; }
}