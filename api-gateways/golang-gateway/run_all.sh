#!/bin/bash

# Navigate to each directory and run the service

# Run Wallet Service
go run wallet/wallet.go &
WALLET_PID=$!

# Run User Service
go run user/user.go &
USER_PID=$!

# Run API Gateway
go run gateway/gateway.go &
GATEWAY_PID=$!

# Wait for all
wait $WALLET_PID $USER_PID $GATEWAY_PID
