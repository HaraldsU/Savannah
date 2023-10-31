using Microsoft.EntityFrameworkCore;
using Savanna.Data.Models;
using Savanna.Data.Models.DB;

var MyAllowSpecificOrigins = "http://localhost:5266";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection") ?? throw new InvalidOperationException("Connection string not found.");
builder.Services.AddDbContext<SavannaContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddSingleton<CurrentGamesModel>();
builder.Services.AddSingleton<CurrentSessionModel>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
