# .NET 8 API Gateway (YARP)

This is part of a multi-language API Gateway Proof of Concept (POC) project. The goal is to demonstrate and compare how an API Gateway and microservices can be implemented in .NET 8 (using YARP), Go (Echo), and Node.js (Express).

## Overview

This implementation uses .NET 8 and YARP (Yet Another Reverse Proxy) to provide a modern, high-performance API Gateway. It proxies requests to two mock microservices: User Service and Wallet Service, each running as a separate ASP.NET Core Web API.

## Features

- Reverse proxy with YARP
- Health check endpoint (`/ping`)
- Swagger/OpenAPI documentation
- CORS support
- Minimal API style
- JSON-based configuration
- Error handling and logging
- Consistent endpoints and ports with Go and Node.js implementations

## Prerequisites

- .NET 8 SDK
- PowerShell (for running all services together)

## Installation

1. Clone the repository
2. Navigate to the project directory:
   ```bash
   cd api-gateways/dotnet-gateway
   ```
3. Restore dependencies:
   ```bash
   dotnet restore
   ```

## Running the Services

### Run All Services

Using the provided PowerShell script:

```powershell
.\run-all.ps1
```

### Run Individual Services

- Gateway (Port 8080):
  ```bash
  dotnet run --project src/Gateway/Gateway.csproj
  ```
- User Service (Port 5202):
  ```bash
  dotnet run --project src/UserService/UserService.csproj
  ```
- Wallet Service (Port 5201):
  ```bash
  dotnet run --project src/WalletService/WalletService.csproj
  ```

## API Endpoints

All endpoints are accessible via the gateway on port 8080:

### User Service

- `POST /api/user/login` → `{ "message": "login successful", "token": "mock-jwt-token" }`
- `GET /api/user/profile` → `{ "id": 1, "name": "John Doe", "email": "john@example.com" }`

### Wallet Service

- `GET /api/wallet/balance` → `{ "balance": 150.75, "currency": "USD" }`
- `POST /api/wallet/charge` → `{ "status": "charged" }`

### Health Check

- `GET /ping` → `pong`

## Project Structure

```
dotnet-gateway/
├── src/
│   ├── Gateway/           # API Gateway using YARP
│   ├── UserService/       # User service implementation
│   └── WalletService/     # Wallet service implementation
├── ApiGateway.sln        # Solution file
└── run-all.ps1           # Script to run all services
```

## Configuration

The gateway is configured using `appsettings.json`:

- `ReverseProxy`: YARP configuration
  - `Routes`: Route patterns and their targets
  - `Clusters`: Service destinations

## Technology Comparison

| Feature        | .NET 8 (YARP) | Go (Echo)     | Node.js (Express)     |
| -------------- | ------------- | ------------- | --------------------- |
| Language       | C#            | Go            | JavaScript            |
| Reverse Proxy  | YARP          | Custom (Echo) | http-proxy-middleware |
| API Style      | Minimal API   | REST (Echo)   | REST (Express)        |
| Swagger        | Yes           | Yes           | Yes                   |
| CORS           | Yes           | Yes           | Yes                   |
| Health Check   | Yes           | Yes           | Yes                   |
| Port           | 8080          | 8080          | 8080                  |
| User Service   | 5202          | 5202          | 5202                  |
| Wallet Service | 5201          | 5201          | 5201                  |

## See Also

- [Go API Gateway README](../golang-gateway/README.md)
- [Node.js API Gateway README](../nodejs-gateway/README.md)

## License

This project is licensed under the MIT License.
