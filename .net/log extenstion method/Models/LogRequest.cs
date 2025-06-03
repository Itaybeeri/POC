namespace LogExtensionMethod.Models;

/// <summary>
/// Represents a log request from external applications
/// </summary>
public class LogRequest
{
    /// <summary>
    /// The log level (Trace, Debug, Information, Warning, Error, Critical, None)
    /// </summary>
    public string Level { get; set; } = "Information";

    /// <summary>
    /// The log message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Optional exception details
    /// </summary>
    public Exception? Exception { get; set; }

    /// <summary>
    /// Optional additional data to be logged
    /// </summary>
    public object? AdditionalData { get; set; }
}