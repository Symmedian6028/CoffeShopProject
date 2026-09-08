namespace CoffeeShop.Api.Dtos;

public class AuthResponseDto
{
    public int UserId { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Token { get; set; }
}