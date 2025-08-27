using System.Text;
using LeaderboardApi.DbOperations;
using LeaderboardApi.Entities;
using LeaderboardApi.Middlewares;
using LeaderboardApi.Services.Loggers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TokenHandler = LeaderboardApi.TokenOperations.TokenHandler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = false,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Token:Issuer"],
        ValidAudience = builder.Configuration["Token:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]!)),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddDbContext<LeaderboardDbContext>(x => x.UseInMemoryDatabase("LeaderboardDB"));
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<ILeaderboardDbContext>(provider => provider.GetService<LeaderboardDbContext>()!);
builder.Services.AddSingleton<ILoggerService, ConsoleLoggerService>();
builder.Services.AddSingleton<TokenHandler>();
builder.Services.AddSingleton<PasswordHasher<User>>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    DataGenerator.Initialize(serviceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseMiddleware<CustomExceptionMiddleware>();
app.MapControllers();

app.Run();