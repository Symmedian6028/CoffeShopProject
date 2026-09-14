using CoffeeShop.Api.Dtos;

namespace CoffeeShop.Api.Services;

public interface IAuthService
{
    Task<AuthResponseDto> SignupAsync(SignupRequestDto request);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
}