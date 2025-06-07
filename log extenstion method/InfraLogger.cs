using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Elastic.Clients.Elasticsearch;
using System;
using System.Threading.Tasks;

namespace LogExtensionMethod;

public static class InfraLogger
{
    private static ElasticsearchClient? _elasticClient;
    private static string? _indexName;
    private static bool _isEnabled;

    public static void Configure(IConfiguration configuration)
    {
        _isEnabled = configuration.GetValue<bool>("InfraLogger:Enabled");

        if (!_isEnabled)
            return;

        var settings = new ElasticsearchClientSettings(new Uri(configuration["InfraLogger:Elasticsearch:Url"]!))
            .DefaultIndex(configuration["InfraLogger:Elasticsearch:IndexName"]!);

        _elasticClient = new ElasticsearchClient(settings);
        _indexName = configuration["InfraLogger:Elasticsearch:IndexName"]!;

        // Create index if it doesn't exist
        CreateIndexIfNotExists();
    }

    private static void CreateIndexIfNotExists()
    {
        if (_elasticClient == null || _indexName == null)
        {
            throw new InvalidOperationException("InfraLogger has not been configured. Call Configure() first.");
        }

        var indexExists = _elasticClient.Indices.Exists(_indexName);

        if (!indexExists.Exists)
        {
            var createIndexResponse = _elasticClient.Indices.Create(_indexName);

            if (!createIndexResponse.IsValidResponse)
            {
                throw new Exception($"Failed to create index: {createIndexResponse.DebugInformation}");
            }
        }
    }

    public static async Task LogInfraAsync(this ILogger logger, LogLevel level, string message, Exception? exception = null, object? additionalData = null)
    {
        if (!_isEnabled)
            return;

        if (_elasticClient == null || _indexName == null)
        {
            throw new InvalidOperationException("InfraLogger has not been configured. Call Configure() first.");
        }

        var logEntry = new
        {
            Id = Guid.NewGuid().ToString(),
            Timestamp = DateTime.UtcNow,
            Level = level.ToString(),
            Message = message,
            Exception = exception?.ToString(),
            AdditionalData = additionalData,
            Source = "Infrastructure"
        };

        var response = await _elasticClient.IndexAsync(logEntry, idx => idx
            .Index(_indexName)
            .Id(logEntry.Id)
            .Refresh(Refresh.True)
        );

        if (!response.IsValidResponse)
        {
            throw new Exception($"Failed to write to Elasticsearch: {response.DebugInformation}");
        }
    }
}