using Microsoft.Extensions.Logging;

namespace LogExtensionMethod;

public class Sample
{
    private readonly ILogger _logger;

    public Sample(ILogger logger)
    {
        _logger = logger;
    }

    public async Task DoSomething()
    {
        try
        {
            // Log the start of the operation
            await _logger.LogInfraAsync(
                LogLevel.Information,
                "Starting operation in Sample class",
                additionalData: new { Operation = "DoSomething", Status = "Started" }
            );

            // Simulate some work
            await Task.Delay(100);

            // Log success
            await _logger.LogInfraAsync(
                LogLevel.Information,
                "Operation completed successfully",
                additionalData: new { Operation = "DoSomething", Status = "Completed" }
            );
        }
        catch (Exception ex)
        {
            // Log error
            await _logger.LogInfraAsync(
                LogLevel.Error,
                "Operation failed",
                ex,
                new { Operation = "DoSomething", Status = "Failed" }
            );
            throw;
        }
    }
}