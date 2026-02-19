using CarRent.DataBase;
using CarRent.Interfaces;
using CarRent.Models;
using CarRent.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Zapisany Connection string jest w pliku appsettings.json
// Bez tego nie będzie połączenia miedzy kodem a bazą danych
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
// Dodwananie obsługi logowania itp. Można tu ustawaić parametry hasła i tak dalej
builder.Services.AddIdentity<User,IdentityRole>()
// łączy Identity z moją baza danych dzięki temu może stworzyć tabele User
.AddEntityFrameworkStores<AppDbContext>(); 
//Bez tego bulder nie widzi atrybutów HTTP i Route
builder.Services.AddControllers();

// Powiązanie linków do pliku konfiguracyjnego
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
// Dodawanie do buildera opcje Autentykacji oraz przypisanie JWT jakie metody jej wykonania
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"], 
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICartInterface, CartService>();

var app = builder.Build();

// Sprawdza Adress URL
app.UseRouting();
// Uruchamia wybrany kontorler
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

