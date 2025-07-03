using LeaderboardApi.DbOperations;
using LeaderboardApi.Middlewares;
using LeaderboardApi.Services.Loggers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<LeaderboardDbContext>(x => x.UseInMemoryDatabase("LeaderboardDB"));
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<ILeaderboardDbContext>(provider => provider.GetService<LeaderboardDbContext>()!);
builder.Services.AddSingleton<ILoggerService, ConsoleLoggerService>();

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