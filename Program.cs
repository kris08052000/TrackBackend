using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TRACK_BACKEND.Data;
using TRACK_BACKEND.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Read secret key from appsettings.json
string jwtKey = builder.Configuration["Jwt:Key"]!;

// DB
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Services
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddSingleton(new JwtService(jwtKey));

// Auth
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:3000") // frontend URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
    );
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();
// Use CORS
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();