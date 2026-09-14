using System.Text;
using CoffeeShop.Api.Data;
using CoffeeShop.Api.Models;
using CoffeeShop.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICoffeeService, CoffeeService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddOpenApi();
builder.Services.AddControllers();

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is not configured. Set it in appsettings.Development.json or the Jwt__Key environment variable.");

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is not configured.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAngularApp");

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
