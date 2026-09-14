using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Api.Dtos;

public class CreateOrderRequestDto
{
    [Required]
    [MinLength(1)]
    public required List<OrderItemRequestDto> Items { get; set; }
}