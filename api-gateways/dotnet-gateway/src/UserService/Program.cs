using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the port from settings
var port = builder.Configuration.GetValue<int>("ServiceSettings:Port");
builder.WebHost.UseUrls($"http://localhost:{port}");

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Health check endpoint
app.MapGet("/ping", () => "pong");

// User endpoints
app.MapPost("/api/user/login", () => new { message = "login successful", token = "mock-jwt-token" });
app.MapGet("/api/user/profile", () => new { id = 1, name = "John Doe", email = "john@example.com" });

app.Run();
