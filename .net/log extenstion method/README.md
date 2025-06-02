# .NET 8 Elasticsearch Logger

This is a custom logging extension method for .NET 8 that writes logs to Elasticsearch. It provides a simple way to integrate Elasticsearch logging into any .NET application.

## Overview

This implementation uses .NET 8 and the official Elasticsearch .NET client to provide a modern, high-performance logging solution. It includes a custom extension method for ILogger that writes structured logs to Elasticsearch with support for additional data and exception logging.

## Features

- Custom ILogger extension method
- Structured logging to Elasticsearch
- Support for additional data objects
- Exception logging with stack traces
- Automatic index creation
- Unique document IDs to prevent overwrites
- Immediate document refresh for searchability
- Docker support for Elasticsearch and Kibana

## Prerequisites

- .NET 8 SDK
- Docker Desktop (for Elasticsearch and Kibana)
- PowerShell (for running services)

## Installation

1. Clone the repository
2. Navigate to the project directory:
   ```bash
   cd log-extension-method
   ```
3. Restore dependencies:
   ```bash
   dotnet restore
   ```

## Running the Services

### Run Elasticsearch and Kibana

Using Docker Compose:

```bash
docker-compose up -d
```

### Run the Application

```bash
dotnet run
```

## Configuration

The logger is configured using `appsettings.json`:

```json
{
  "Elasticsearch": {
    "Url": "http://localhost:9200",
    "IndexName": "logs",
    "ILMPolicy": {
      "Name": "logs-policy",
      "Rollover": {
        "MaxAge": "30d",
        "MaxSize": "50gb"
      },
      "Delete": {
        "MinAge": "90d"
      }
    }
  }
}
```

## Usage

### Basic Logging

```csharp
await logger.LogAsync(LogLevel.Information, "Application started");
```

### Logging with Additional Data

```csharp
await logger.LogAsync(
    LogLevel.Information,
    "User logged in",
    additionalData: new { UserId = "123", Action = "Login" }
);
```

### Logging Exceptions

```csharp
try
{
    // Your code here
}
catch (Exception ex)
{
    await logger.LogAsync(
        LogLevel.Error,
        "An error occurred",
        ex,
        new { ProcessId = "456", Status = "Failed" }
    );
}
```

## Project Structure

```
log-extension-method/
├── InfraLogger.cs        # Custom logger implementation
├── Program.cs            # Example usage
├── appsettings.json      # Configuration
├── docker-compose.yml    # Elasticsearch and Kibana setup
└── LogExtensionMethod.csproj
```

## Log Structure

Each log entry in Elasticsearch contains:

- Unique ID (Guid)
- Timestamp
- Log Level
- Message
- Exception (if any)
- Additional Data (if any)

## Viewing Logs

1. Access Kibana at `http://localhost:5601`
2. Go to "Stack Management" (gear icon)
3. Under "Kibana", click on "Index Patterns"
4. Create a new index pattern for "logs"
5. Go to "Discover" to view your logs

## Docker Services

| Service       | Port | Description                     |
| ------------- | ---- | ------------------------------- |
| Elasticsearch | 9200 | Main Elasticsearch service      |
| Kibana        | 5601 | Web interface for Elasticsearch |

## Troubleshooting

If you encounter any issues:

1. Check if the containers are running:

```bash
docker ps
```

2. View container logs:

```bash
docker logs elasticsearch
docker logs kibana
```

3. Verify Elasticsearch is running:

```bash
curl http://localhost:9200
```

## License

This project is licensed under the MIT License.
