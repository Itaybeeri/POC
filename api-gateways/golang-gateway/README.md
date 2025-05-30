# Golang API Gateway

A modern API Gateway built with Go and Echo framework that provides routing and security features for microservices.

## Features

- Request routing and proxying
- Request logging
- Health check endpoint (/ping)
- JSON-based configuration
- Error handling and recovery

## Prerequisites

- Go 1.16 or higher
- Make (optional, for using Makefile)

## Installation

1. Clone the repository
2. Install dependencies:
   ```bash
   go mod download
   ```
3. Configure the services in `config.json`:
   ```json
   {
     "port": "8080",
     "routes": [
       {
         "path": "/api/wallet/*",
         "target": "http://localhost:5201"
       },
       {
         "path": "/api/user/*",
         "target": "http://localhost:5202"
       }
     ]
   }
   ```

## Usage

### Run API Gateway

```bash
go run gateway.go
```

### Run All Services

Using the provided scripts:

- Windows: `run_all.bat`
- Linux/Mac: `run_all.sh`

## Services

### API Gateway (Port 8080)

- Main entry point for all API requests
- Routes requests to appropriate microservices
- Provides health check endpoint

### User Service (Port 5202)

Endpoints:

- `GET /ping` - Health check
- `POST /api/user/login` - User login
  ```json
  {
    "message": "login successful",
    "token": "mock-jwt-token"
  }
  ```
- `GET /api/user/profile` - Get user profile
  ```json
  {
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com"
  }
  ```

### Wallet Service (Port 5201)

Endpoints:

- `GET /api/wallet/balance` - Get wallet balance
  ```json
  {
    "balance": 150.75,
    "currency": "USD"
  }
  ```
- `POST /api/wallet/charge` - Charge wallet
  ```json
  {
    "status": "charged"
  }
  ```

## API Gateway Endpoints

All requests should be made to the API Gateway (port 8080):

- User Service:

  - `GET /api/user/profile`
  - `POST /api/user/login`

- Wallet Service:

  - `GET /api/wallet/balance`
  - `POST /api/wallet/charge`

- Health Check:
  - `GET /ping`

## Project Structure

```
golang-gateway/
├── gateway.go           # API Gateway main application
├── config.json         # Gateway configuration
├── go.mod             # Go module file
├── go.sum             # Go module checksum
├── user/              # User service
│   └── user.go
├── wallet/            # Wallet service
│   └── wallet.go
├── run_all.bat        # Windows script to run all services
└── run_all.sh         # Unix script to run all services
```

## Dependencies

- `github.com/labstack/echo/v4` - Web framework
- `github.com/labstack/echo/v4/middleware` - Echo middleware

## Configuration

The gateway is configured using `config.json`:

- `port`: The port the gateway will listen on
- `routes`: Array of route configurations
  - `path`: The path pattern to match
  - `target`: The target service URL

## Running Services

### Individual Services

1. User Service:

   ```bash
   cd user
   go run user.go
   ```

2. Wallet Service:

   ```bash
   cd wallet
   go run wallet.go
   ```

3. API Gateway:
   ```bash
   go run gateway.go
   ```

### All Services

Use the provided scripts:

- Windows: `run_all.bat`
- Linux/Mac: `run_all.sh`

## Error Handling

- The gateway includes built-in error handling and recovery
- Failed requests are logged with appropriate status codes
- Service unavailability is handled gracefully
