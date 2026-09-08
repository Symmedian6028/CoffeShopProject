using CoffeeShop.Api.Dtos;

namespace CoffeeShop.Api.Services;

public interface IOrderService
{
    Task<OrderResponseDto> CreateOrderAsync(int userId, CreateOrderRequestDto request);
    Task<List<OrderResponseDto>> GetUserOrdersAsync(int userId);
}