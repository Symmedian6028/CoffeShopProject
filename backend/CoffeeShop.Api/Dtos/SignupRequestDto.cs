using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Api.Dtos;

public class SignupRequestDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [MinLength(6)]
    public required string Password { get; set; }

    [Required]
    public required string Name { get; set; }
}