using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CoffeeShop.Api.Services;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Api.Data;
using CoffeeShop.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]
        ?? throw new InvalidOperationException("Jwt:Key appsettings.json'da tanımlı değil.")))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=coffeeshop.db"));
builder.Services.AddScoped<ICoffeeService, CoffeeService>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Coffees.Any())
    {
        db.Coffees.AddRange(
            new Coffee { Name = "Espresso", Price = 45.00m, Description = "Yoğun ve güçlü, saf kahve deneyimi.", Category = "Sıcak İçecekler" },
            new Coffee { Name = "Latte", Price = 55.00m, Description = "Espresso ve buharla köpürtülmüş süt.", Category = "Sıcak İçecekler" },
            new Coffee { Name = "Cappuccino", Price = 55.00m, Description = "Eşit oranda espresso, süt ve süt köpüğü.", Category = "Sıcak İçecekler" },
            new Coffee { Name = "Iced Americano", Price = 50.00m, Description = "Espresso, su ve bol buz.", Category = "Soğuk İçecekler" },
            new Coffee { Name = "Cold Brew", Price = 60.00m, Description = "Soğuk suyla uzun süre demlenmiş, yumuşak içimli kahve.", Category = "Soğuk İçecekler" }
        );

        db.SaveChanges();
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}