using CoffeeShop.Api.Data;
using CoffeeShop.Api.Dtos;
using CoffeeShop.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoffeeShop.Api.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> SignupAsync(SignupRequestDto request)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (existingUser != null)
        {
            throw new Exception("Bu email zaten kayıtlı.");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var newUser = new User
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            Name = request.Name,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return new AuthResponseDto
        {
            UserId = newUser.Id,
            Email = newUser.Email,
            Name = newUser.Name,
            Token = GenerateJwtToken(newUser)
        };
    }

  public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
{
    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Email == request.Email);

    if (user == null)
    {
        throw new Exception("Email veya şifre hatalı.");
    }

    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

    if (!isPasswordValid)
    {
        throw new Exception("Email veya şifre hatalı.");
    }

    return new AuthResponseDto
    {
        UserId = user.Id,
        Email = user.Email,
        Name = user.Name,
        Token = GenerateJwtToken(user)
    };
}

    private string GenerateJwtToken(User user)
    {
    var jwtKey = _configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key appsettings.json'da tanımlı değil.");

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}