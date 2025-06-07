using Anthropic.SDK;
using MpcDemoServer.Services;
using Microsoft.OpenApi.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ICustomerService, InMemoryCustomerService>();
builder.Services.AddSingleton<AnthropicClient>();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MCP Server API",
        Version = "v1",
        Description = "A Model Context Protocol (MCP) server that exposes customer data for AI models and clients."
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Commented out for testing

// MCP Metadata endpoint
app.MapGet("/mcp/metadata", () =>
{
    var metadata = new
    {
        version = "1.0",
        tools = new object[]
        {
            new
            {
                name = "getCustomer",
                description = "Get customer details by ID",
                parameters = new
                {
                    type = "object",
                    properties = new
                    {
                        id = new
                        {
                            type = "string",
                            description = "The ID of the customer to retrieve"
                        }
                    },
                    required = new[] { "id" }
                }
            },
            new
            {
                name = "listCustomers",
                description = "Get a list of all customers",
                parameters = new
                {
                    type = "object",
                    properties = new object { }
                }
            }
        },
        resources = new object[]
        {
            new
            {
                name = "customer",
                description = "Customer information resource",
                properties = new
                {
                    id = new { type = "string", description = "Unique identifier for the customer" },
                    name = new { type = "string", description = "Customer's full name" },
                    email = new { type = "string", description = "Customer's email address" }
                }
            }
        }
    };

    return Results.Json(metadata, new JsonSerializerOptions { WriteIndented = true });
})
.WithName("GetMcpMetadata")
.WithOpenApi();

app.MapGet("/mcp/customer/{id}", async (string id, ICustomerService service) =>
{
    var customer = await service.GetCustomerByIdAsync(id);
    return customer is null ? Results.NotFound() : Results.Ok(customer);
})
.WithName("GetCustomer")
.WithOpenApi();

app.MapGet("/mcp/customers", async (ICustomerService service) =>
{
    var customers = await service.ListCustomersAsync();
    return Results.Ok(customers);
})
.WithName("GetAllCustomers")
.WithOpenApi();

app.Run();
