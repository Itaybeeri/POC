using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LogExtensionMethod;
using LogExtensionMethod.Models;
using LogExtensionMethod.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Configure both loggers
InfraLogger.Configure(builder.Configuration);
AppsLogger.Configure(builder.Configuration);

// Add services
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

// Application logging endpoint - for external applications
app.MapPost("/api/log", async (HttpContext context, ILogger<Program> logger) =>
{
    try
    {
        // Log the incoming request
        logger.LogInformation("Received log request");

        // Check content type
        if (!context.Request.HasJsonContentType())
        {
            logger.LogWarning("Invalid content type: {ContentType}", context.Request.ContentType);
            return Results.BadRequest("Content-Type must be application/json");
        }

        // Read and validate the request
        var logData = await context.Request.ReadFromJsonAsync<LogRequest>();
        if (logData == null)
        {
            logger.LogWarning("Invalid log data received");
            return Results.BadRequest("Invalid log data");
        }

        // Validate required fields
        if (string.IsNullOrEmpty(logData.Message))
        {
            logger.LogWarning("Message is required");
            return Results.BadRequest("Message is required");
        }

        // Convert string level to LogLevel enum
        if (!Enum.TryParse<LogLevel>(logData.Level, true, out var logLevel))
        {
            logger.LogWarning("Invalid log level: {Level}", logData.Level);
            return Results.BadRequest($"Invalid log level. Valid values are: {string.Join(", ", Enum.GetNames(typeof(LogLevel)))}");
        }

        // Log the data
        await logger.LogAppAsync(
            logLevel,
            logData.Message,
            logData.Exception,
            logData.AdditionalData
        );

        logger.LogInformation("Log entry created successfully");
        return Results.Ok(new { message = "Log entry created successfully" });
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error processing log request");
        return Results.Problem(
            title: "Error processing log request",
            detail: ex.Message,
            statusCode: StatusCodes.Status500InternalServerError
        );
    }
});

// Internal logging endpoint - for demonstration
app.MapGet("/api/internal", async (ILogger<Program> logger) =>
{
    await logger.LogInfraAsync(
        LogLevel.Information,
        "Internal operation completed",
        additionalData: new { Operation = "Internal", Status = "Success" }
    );

    return Results.Ok("Internal operation logged");
});

app.Run();

public class LogRequest
{
    public string Level { get; set; } = "Information";
    public string Message { get; set; } = string.Empty;
    public Exception? Exception { get; set; }
    public object? AdditionalData { get; set; }
}

// Example of using the logger in another class
public class UserService
{
    private readonly ILogger _logger;

    public UserService(ILogger logger)
    {
        _logger = logger;
    }

    public async Task ProcessUser(string userId)
    {
        try
        {
            // Simulate some work
            await Task.Delay(100);

            // Log success
            await _logger.LogInfraAsync(
                LogLevel.Information,
                "User processed successfully",
                additionalData: new { UserId = userId, Status = "Completed" }
            );
        }
        catch (Exception ex)
        {
            // Log error
            await _logger.LogInfraAsync(
                LogLevel.Error,
                "Failed to process user",
                ex,
                new { UserId = userId, Status = "Failed" }
            );
            throw;
        }
    }
}