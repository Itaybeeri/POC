using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LogExtensionMethod;

namespace LogExtensionMethod;

class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection()
            .AddLogging(builder =>
            {
                builder.AddConsole();
            })
            .BuildServiceProvider();

        // Configure the InfraLogger
        InfraLogger.Configure(configuration);

        // Get logger for Program class
        var logger = services.GetRequiredService<ILogger<Program>>();

        // Example 1: Basic logging
        await logger.LogAsync(LogLevel.Information, "Application started");
        Console.WriteLine("Logged: Application started");

        // Example 2: Logging with additional data
        await logger.LogAsync(
            LogLevel.Information,
            "User logged in",
            additionalData: new { UserId = "123", Action = "Login" }
        );
        Console.WriteLine("Logged: User logged in");

        // Example 3: Logging an error with exception
        try
        {
            throw new Exception("This is a test exception");
        }
        catch (Exception ex)
        {
            await logger.LogAsync(
                LogLevel.Error,
                "An error occurred during processing",
                ex,
                new { ProcessId = "456", Status = "Failed" }
            );
            Console.WriteLine("Logged: Error occurred");
        }

        // Example 4: Logging in a different class
        var userService = new UserService(logger);
        await userService.ProcessUser("user123");
        Console.WriteLine("Logged: User processed");

        // Example 5: Logging with different log levels
        await logger.LogAsync(LogLevel.Debug, "Debug message");
        await logger.LogAsync(LogLevel.Warning, "Warning message");
        await logger.LogAsync(LogLevel.Critical, "Critical message");
        Console.WriteLine("Logged: Different log levels");

        // Wait a moment to ensure all logs are written
        await Task.Delay(1000);
        Console.WriteLine("All logs should be written to Elasticsearch now");
    }
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
            await _logger.LogAsync(
                LogLevel.Information,
                "User processed successfully",
                additionalData: new { UserId = userId, Status = "Completed" }
            );
        }
        catch (Exception ex)
        {
            // Log error
            await _logger.LogAsync(
                LogLevel.Error,
                "Failed to process user",
                ex,
                new { UserId = userId, Status = "Failed" }
            );
            throw;
        }
    }
}