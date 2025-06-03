using Microsoft.Extensions.Logging;

namespace LogExtensionMethod.Services;

/// <summary>
/// Example service demonstrating internal logging usage
/// </summary>
public class UserService
{
    private readonly ILogger _logger;

    public UserService(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Example method demonstrating internal logging
    /// </summary>
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