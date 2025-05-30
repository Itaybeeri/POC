# Node.js API Gateway

A modern API Gateway built with Node.js and Express that provides routing, rate limiting, and security features for microservices. This implementation matches the functionality of the Golang API Gateway.

## Features

- Request routing and proxying
- Rate limiting
- CORS support
- Security headers (Helmet)
- Request logging
- Health check endpoint (/ping)
- Environment-based configuration

## Prerequisites

- Node.js (v14 or higher)
- npm (v6 or higher)

## Installation

1. Clone the repository
2. Install dependencies:
   ```bash
   npm install
   ```
3. Create a `.env` file in the root directory with the following variables:
   ```
   PORT=8080
   NODE_ENV=development
   WALLET_SERVICE_URL=http://localhost:5201
   USER_SERVICE_URL=http://localhost:5202
   RATE_LIMIT_WINDOW_MS=900000
   RATE_LIMIT_MAX_REQUESTS=100
   ```

## Usage

### Development

```bash
npm run dev
```

### Production

```bash
npm start
```

### Run All Services

```bash
npm run start:all
```

## Services

### API Gateway (Port 8080)

- Main entry point for all API requests
- Routes requests to appropriate microservices
- Provides health check endpoint

### User Service (Port 5202)

Endpoints:

- `GET /` - Service status
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

- `GET /` - Service status
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

## Security Features

- Rate limiting to prevent abuse
- CORS protection
- Security headers with Helmet
- Environment-based error messages

## Project Structure

```
nodejs-gateway/
├── src/
│   ├── app.js                 # API Gateway main application
│   └── mock-services/
│       ├── user-service.js    # User service implementation
│       └── wallet-service.js  # Wallet service implementation
├── .env                       # Environment variables
├── package.json              # Project configuration
└── run-all.js               # Script to run all services
```

## Available Scripts

- `npm start` - Start the API Gateway
- `npm run dev` - Start the API Gateway with nodemon (development)
- `npm run start:user` - Start only the User Service
- `npm run start:wallet` - Start only the Wallet Service
- `npm run start:all` - Start all services (Gateway, User, and Wallet)
