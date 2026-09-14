namespace CoffeeShop.Api.Dtos;

public class OrderResponseDto
{
    public int OrderId { get; set; }
    public decimal TotalPrice { get; set; }
    public required string Status { get; set; }
    public required  List<OrderItemResponseDto> Items { get; set; }
}