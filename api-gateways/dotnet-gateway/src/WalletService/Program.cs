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

// Wallet endpoints
app.MapGet("/api/wallet/balance", () => new { balance = 150.75, currency = "USD" });
app.MapPost("/api/wallet/charge", () => new { status = "charged" });

app.Run();
