# Logging Extension Method POC

This is a proof of concept for a dual-logging system that separates application logs from infrastructure logs.

## Features

- Two separate logging systems:
  - `AppsLogger`: For application-level logging (external applications)
  - `InfraLogger`: For infrastructure-level logging (internal use)
- Elasticsearch integration for log storage
- Configurable through appsettings.json
- Enable/disable logging systems independently

## Configuration

The logging systems are configured in `appsettings.json`:

```json
{
  "AppsLogger": {
    "Enabled": true,
    "Elasticsearch": {
      "Url": "http://localhost:9200",
      "IndexName": "apps-logs"
    }
  },
  "InfraLogger": {
    "Enabled": true,
    "Elasticsearch": {
      "Url": "http://localhost:9200",
      "IndexName": "infra-logs"
    }
  }
}
```

## API Endpoints

### Application Logging

```http
POST /api/log
Content-Type: application/json

{
    "level": "Information",
    "message": "Test message",
    "additionalData": {
        "test": "value"
    }
}
```

Valid log levels:

- Trace
- Debug
- Information
- Warning
- Error
- Critical
- None

### Internal Logging

```http
GET /api/internal
```

## Usage Examples

### External Application Logging

```csharp
await logger.LogAppAsync(
    LogLevel.Information,
    "User action completed",
    additionalData: new { UserId = "123", Action = "login" }
);
```

### Internal Infrastructure Logging

```csharp
await logger.LogInfraAsync(
    LogLevel.Information,
    "Database connection established",
    additionalData: new { ConnectionId = "db-123" }
);
```

## Requirements

- .NET 8.0
- Elasticsearch running on localhost:9200

## Testing

1. Start Elasticsearch
2. Run the application
3. Test the endpoints:

   ```bash
   # Test internal logging
   curl http://localhost:5000/api/internal

   # Test application logging
   curl -X POST http://localhost:5000/api/log \
     -H "Content-Type: application/json" \
     -d "{\"level\":\"Information\",\"message\":\"Test message\"}"
   ```
