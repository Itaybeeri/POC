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
app.MapGet("/mcp/metadata", (ILogger<Program> logger) =>
{
    logger.LogInformation("Received request for MCP metadata");
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

    logger.LogInformation("Returning metadata: {Metadata}", JsonSerializer.Serialize(metadata));
    return Results.Json(metadata, new JsonSerializerOptions { WriteIndented = true });
})
.WithName("GetMcpMetadata")
.WithOpenApi();

app.MapGet("/mcp/customer/{id}", async (string id, ICustomerService service, ILogger<Program> logger) =>
{
    logger.LogInformation("Received request for customer ID: {Id}", id);
    try
    {
        var customer = await service.GetCustomerByIdAsync(id);
        if (customer is null)
        {
            logger.LogWarning("Customer not found for ID: {Id}", id);
            return Results.NotFound();
        }
        logger.LogInformation("Customer found: {Customer}", JsonSerializer.Serialize(customer));
        return Results.Ok(customer);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error processing request for customer ID: {Id}", id);
        return Results.Problem(ex.Message);
    }
})
.WithName("GetCustomer")
.WithOpenApi();

app.MapGet("/mcp/customers", async (ICustomerService service, ILogger<Program> logger) =>
{
    logger.LogInformation("Received request for all customers");
    try
    {
        var customers = await service.ListCustomersAsync();
        logger.LogInformation("Returning {Count} customers", customers.Count());
        return Results.Ok(customers);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error processing request for all customers");
        return Results.Problem(ex.Message);
    }
})
.WithName("GetAllCustomers")
.WithOpenApi();

app.Run();
