using CoffeeShop.Api.Data;
using CoffeeShop.Api.Dtos;
using CoffeeShop.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Api.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrderResponseDto> CreateOrderAsync(int userId, CreateOrderRequestDto request)
    {
        // 1. İstenen kahvelerin ID'lerini topla
        var coffeeIds = request.Items.Select(i => i.CoffeeId).ToList();

        // 2. Bu kahveleri veritabanından tek seferde çek
        var coffees = await _context.Coffees
            .Where(c => coffeeIds.Contains(c.Id))
            .ToListAsync();

        // 3. İstenen kahvelerden biri veritabanında yoksa hata ver
        if (coffees.Count != coffeeIds.Distinct().Count())
        {
            throw new Exception("Bazı kahveler bulunamadı.");
        }

        // 4. Order nesnesini oluştur (henüz kaydetmeden)
        var order = new Order
        {
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            OrderItems = new List<OrderItem>()
        };

        decimal totalPrice = 0;

        // 5. Her istenen kahve için OrderItem oluştur, toplamı hesapla
        foreach (var item in request.Items)
        {
            var coffee = coffees.First(c => c.Id == item.CoffeeId);

            order.OrderItems.Add(new OrderItem
            {
                CoffeeId = coffee.Id,
                Quantity = item.Quantity
            });

            totalPrice += coffee.Price * item.Quantity;
        }

        order.TotalPrice = totalPrice;

        // 6. Veritabanına kaydet
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // 7. Response DTO'sunu hazırla
        return new OrderResponseDto
        {
            OrderId = order.Id,
            TotalPrice = order.TotalPrice,
            Status = order.Status.ToString(),
            Items = request.Items.Select(item =>
            {
                var coffee = coffees.First(c => c.Id == item.CoffeeId);
                return new OrderItemResponseDto
                {
                    CoffeeName = coffee.Name,
                    Quantity = item.Quantity,
                    UnitPrice = coffee.Price
                };
            }).ToList()
        };
    }
    public async Task<List<OrderResponseDto>> GetUserOrdersAsync(int userId)
{
    var orders = await _context.Orders
        .Where(o => o.UserId == userId)
        .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Coffee)
        .OrderByDescending(o => o.CreatedAt)
        .ToListAsync();

    return orders.Select(order => new OrderResponseDto
    {
        OrderId = order.Id,
        TotalPrice = order.TotalPrice,
        Status = order.Status.ToString(),
        Items = order.OrderItems.Select(oi => new OrderItemResponseDto
        {
            CoffeeName = oi.Coffee!.Name,
            Quantity = oi.Quantity,
            UnitPrice = oi.Coffee!.Price
        }).ToList()
    }).ToList();
}
}